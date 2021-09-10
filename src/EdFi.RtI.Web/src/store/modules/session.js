/* eslint-disable */

const api = require("axios");
import axios from 'axios';
import Vue from 'vue';

const roles = {
    sysAdmin: 'LEA System Administrator', // admin
    sysAdmin: 'LEA Administrator', // admin
    schoolAdmin: 'School Administrator', // admin
    operationalSupport: 'Operational Support', // admin
    teacher: 'Teacher',
};

const storageItems = {
    session: {
        oidcUser: 'edgraph.oidcUser',
        profile: 'edfi-rti.userProfile',
    },
};

export default {
    namespaced: true,

    state: {
        apps: [],
    },

    getters: {
        getAccessToken: (state, getters) => {
            const oidcUser = getters.getOidcUser;
            if (!oidcUser) return null;
            return oidcUser.access_token;
        },

        getApps: state => state.apps,

        getOidcUser: () => {
            let oidcUserStr = sessionStorage.getItem(storageItems.session.oidcUser);
            if (!oidcUserStr) return undefined;
            return JSON.parse(oidcUserStr);
        },

        getProfile: () => {
            let profileStr = sessionStorage.getItem(storageItems.session.profile);
            if (!profileStr) return undefined;
            return JSON.parse(profileStr);
        },

        getStaff: (state, getters) => {
            let profile = getters.getProfile;
            if (!profile) return undefined;
            return profile.staff;
        },

        getRole: (state, getters) => {
            let profile = getters.getProfile;
            
            if (!profile)
                return undefined;
            
            return profile.staffEducationOrganizationAssignmentAssociation.staffClassificationDescriptor;
        },

        getOrganizationId: (state, getters) => {
            let profile = getters.getProfile;
            
            if (!profile)
                return undefined;
            
            return profile.staffEducationOrganizationAssignmentAssociation.educationOrganizationReference.educationOrganizationId;
        },

        isRoleAdmin: (state, getters) => { // isCurrentUserAdmin
            let role = getters.getRole;

            if (!role)
                return false;
            
            return role.includes(roles.sysAdmin) || role.includes(roles.schoolAdmin) || role.includes(roles.operationalSupport);
        },

        isRoleTeacher: (state, getters) => { // isCurrentUserTeacher
            let role = getters.getRole;

            if (!role)
                return false;

            return role.includes(roles.teacher);
        },

    }, // getters

    mutations: {
        setApps(state, apps) {
            console.log("setApps()", apps);
            state.apps = apps;
        },

        setProfile({ state }, profile) {
            let profileStr = JSON.stringify(profile);
            sessionStorage.setItem(storageItems.session.profile, profileStr);
        },
    }, // mutations

    actions: {
        getApps({ state, commit, getters }, tenantId) {
            console.log("getApps()", tenantId);

            const accessToken = getters.getAccessToken;

            if (!accessToken) {
                console.log("No access token! Setting apps to empty array");
                return new Promise((resolve, reject) => {
                    commit('setApps', []);
                    reject("Access token non existent");
                });
            }

            const baseUri = Vue.prototype.$config.api.basePlatformServiceUri;
            const requestUrl = `${baseUri}/tenant/tenants/${tenantId}/applicationtiles`;
            
            return axios.get(requestUrl)
            .then(result => {
                commit('setApps', result.data.data);
                return result;
            });
        },

        getMe({ getters }) {
            const accessToken = getters.getAccessToken;

            if (!accessToken) {
                return new Promise((resolve, reject) => {
                    reject("Access token non existent");
                });
            }

            const baseUri = Vue.prototype.$config.api.basePlatformServiceUri;
            const requestUrl = `${baseUri}/tenant/me`;

            return axios.get(requestUrl);
        },

        getStaffEducationOrganizationAssignmentAssociations() {
            return api.get('me/staff-education-organization-assignment-associations');
        },

        getUsers() {
            return api.get('me/users');
        },

        getUserSessionProfile({ state }, staffId) {
            return api.get(`me/session-profile/${staffId}`);
        },

        getUserSessionProfileAll({ state }) {
            return api.get(`me/session-profile`);
        },

        getUserSessionProfileByEmail({ state, commit }, email) {
            return api.get(`me/session-profile/email/${email}`);
        },

        saveUserPreferences({ getters }, { code, value }) {
            console.log('saveUserPreferences()');

            const accessToken = getters.getAccessToken;
            
            if (!accessToken) {
                return new Promise((resolve, reject) => {
                    reject("Access token non existent");
                });
            }

            const baseUri = Vue.prototype.$config.api.basePlatformServiceUri;
            const requestUrl = `${baseUri}/tenant/me/preferences`;
            const body = { code, value };

            return axios.post(requestUrl, body);
        },

        signIn({ state, commit }, profile) {
            commit('setProfile', profile);
            return new Promise(resolve => setTimeout(resolve, 2000));
        },

        signOut({ state }) {
            sessionStorage.removeItem(storageItems.session.profile);
            return new Promise(resolve => setTimeout(resolve, 2000));
        },

        getAdminUsers({ state }){
            return api.get(`me/session-admins`)
        },

        getTeacherUsers({ state }){
            return api.get(`me/session-teachers`)
        },
    }, // actions
}
