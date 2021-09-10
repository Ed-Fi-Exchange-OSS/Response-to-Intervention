/* eslint-disable */

import axios from 'axios';

export default {

    namespaced: true,

    state: {
        apps: [],
    }, // state

    getters: {
        getApps: state => state.apps,
    }, // getters

    mutations: {
        setApps(state, apps) {
            state.apps = apps;
        },
    }, // mutations

    actions: {
        getAppsRequest({ state, commit }) {
            console.log('getAppsRequest()');

            const oidcUserStr = sessionStorage.getItem('edgraph.oidcUser');
            const oidcUser = JSON.parse(oidcUserStr);
            const accessToken = oidcUser.access_token;

            return axios.get('https://api.edgraph.dev/tenant/tenants/tenant-00000000-0000-0000-0000-000000000000/applicationtiles', {

            })
            .then(result => {
                commit('setApps', result.data.data);
                return result;
            });
        },
    }, // actions
}
