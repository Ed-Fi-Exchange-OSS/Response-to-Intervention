/*eslint-disable*/

const api = require("axios");

export default {

    namespaced: true,

    state: {
        moduleName: "cacheRefresh",
    }, // state

    getters: {

    }, // getters

    mutations: {

    }, // mutations

    actions: {
        getSectionsByStaffLastRefreshedDate({ state }, email) {
            return api.get(`${state.moduleName}/sectionsByStaff/date`);
        },

        getStaffEmailIdPairsLastRefreshedDate({ state }, email) {
            return api.get(`${state.moduleName}/staffEmailIdPairs/date/${email}`);
        },

        getStaffsBySchoolLastRefreshedDate({ state }, email) {
            return api.get(`${state.moduleName}/staffsBySchool/date`);
        },

        getUserProfilesLastRefreshedDate({ state }) {
            return api.get(`${state.moduleName}/userProfiles/date`);
        },

        getUserProfilesStaffEmailIdPairsLastRefreshedDate({ state }) {
            return api.get(`${state.moduleName}/userProfiles/date/staffEmailIdPairs`);
        },

        getSectionsLastRefreshedDate({ state }) {
            return api.get(`${state.moduleName}/sections/date`);
        },

        getSchoolsLastRefreshedDate({ state }) {
            return api.get(`${state.moduleName}/schools/date`);
        },

        getStaffsLastRefreshedDate({ state }) {
            return api.get(`${state.moduleName}/staffs/date`);
        },

        getStudentsLastRefreshedDate({ state }) {
            return api.get(`${state.moduleName}/students/date`);
        },

        getAssessmentsLastRefreshedDate({ state }) {
            return api.get(`${state.moduleName}/assessments/date`);
        },

        getInterventionsLastRefreshedDate({ state }) {
            return api.get(`${state.moduleName}/interventions/date`);
        },

        refreshSectionsByStaff({ state }) {
            return api.get(`${state.moduleName}/sectionsByStaff`);
        },

        refreshStaffEmailIdPairs({ state }) {
            return api.get(`${state.moduleName}/staffEmailIdPairs`);
        },

        refreshSchools({ state }) {
            return api.get(`${state.moduleName}/schools`);
        },

        refreshStaffs({ state }) {
            return api.get(`${state.moduleName}/staffs`);
        },

        refreshStaffsBySchool({ state }) {
            return api.get(`${state.moduleName}/staffsBySchool`);
        },

        refreshStudents({ state }) {
            return api.get(`${state.moduleName}/students`);
        },

        refreshAssessments({ state }) {
            return api.get(`${state.moduleName}/assessments`);
        },

        refreshInterventions({ state }) {
            return api.get(`${state.moduleName}/interventions`);
        },

        refreshSections({ state }) {
            return api.get(`${state.moduleName}/sections`);
        },

        refreshStudentsBySection({ state }) {
            return api.get(`${state.moduleName}/studentsBySection`);
        },
    }, // actions
};

const mapWithFullName = (result) => {

    // Map staff names to a single fullName
    result.data = result.data.map(staff => {
        staff.fullName = '';

        if (staff.firstName) staff.fullName += staff.firstName + ' ';
        if (staff.middleName) staff.fullName += staff.middleName + ' ';
        if (staff.lastSurname) staff.fullName += staff.lastSurname + ' ';

        staff.fullName = staff.fullName.trim();

        return staff;
    });

    // Order by fullName
    result.data = result.data.sort((a, b) => {
        if (a.fullName < b.fullName) return -1;
        if (a.fullName > b.fullName) return 1;
        return 0;
    });

    return result;
}
