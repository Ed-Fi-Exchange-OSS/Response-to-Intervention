const keys = {
    oidcUser: 'edgraph.oidcUser',
    featuresSettings: "edfi-rti.featuresSettings",
    profile: 'edfi-rti.userProfile',
};

const roles = {
    sysAdmin: 'LEA System Administrator',
    schoolAdmin: 'School Administrator',
    operationalSupport: 'Operational Support',
    teacher: 'Teacher',
};

const adminRoles = new Set([ roles.sysAdmin, roles.schoolAdmin, roles.operationalSupport ]);
const teacherRoles = new Set([ roles.teacher ]);

const isNullOrWhitespace = (str) => str == undefined || str == null || str.trim().length == 0 || str == "undefined" || str == "null";

export default {
    getAccessToken() {
        const oidcUser = this.getOidcUser();

        if (oidcUser == null)
            return null;
        
        return oidcUser.access_token;
    },

    getOidcUser() {
        const json = sessionStorage.getItem(keys.oidcUser);

        if (isNullOrWhitespace(json))
            return null;
        
        return JSON.parse(json);
    },

    getOrganizationId() {
        const profile = this.getProfile();
        
        if (profile == null)
            return null;
        
        return profile.staffEducationOrganizationAssignmentAssociation.educationOrganizationReference.educationOrganizationId;
    },

    getFeaturesSettings() {
        const json = sessionStorage.getItem(keys.featuresSettings);

        if (isNullOrWhitespace(json))
            return null;

        return JSON.parse(json);
    },

    getProfile() {
        const json = sessionStorage.getItem(keys.profile);

        if (isNullOrWhitespace(json))
            return null;

        return JSON.parse(json);
    },

    getRole() {
        const profile = this.getProfile();

        if (profile == null)
            return null;

        return profile.staffEducationOrganizationAssignmentAssociation.staffClassificationDescriptor;
    },

    getStaff() {
        const profile = this.getProfile();

        if (profile == null)
            return null;
        
        return profile.staff;
    },

    isRoleAdmin() {
        const role = this.getRole();
        return adminRoles.has(role);
    },

    isRoleTeacher() {
        const role = this.getRole();
        return teacherRoles.has(role);
    },

    isSessionStored() {
        const profile = this.getProfile();
        return profile != null;
    },

    removeFeaturesSettings() {
        sessionStorage.removeItem(keys.featuresSettings);
    },

    removeProfile() {
        sessionStorage.removeItem(keys.profile);
    },

    setFeaturesSettings(featuresSettings) {
        const json = JSON.stringify(featuresSettings);
        sessionStorage.setItem(keys.featuresSettings, json);
    },

    setProfile(profile) {
        const json = JSON.stringify(profile);
        sessionStorage.setItem(keys.profile, json);
    },
};
