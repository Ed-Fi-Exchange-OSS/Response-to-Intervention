/*eslint-disable*/

const api = require("axios");

const cacheItems = {
    isCacheInitialized: 'edfi.isCacheInitialized',
    schools: 'edfi.schools',
    staffs: 'edfi.staffs',
    sections: 'edfi.sections',
    categories: 'edfi.categories',
    students: 'edfi.students',
};

export default {

    namespaced: true,

    state: {
        moduleName: 'cache',
    }, // state

    getters: {
        getSchools: state => {
            let str = localStorage.getItem(cacheItems.schools);

            if (!str || str.trim().length == 0)
                return [];

            return JSON.parse(str);
        },

        getStaffs: state => {
            let str = localStorage.getItem(cacheItems.staffs);

            if (!str || str.trim().length == 0)
                return [];

            return JSON.parse(str);
        },

        getSections: state => {
            let str = localStorage.getItem(cacheItems.sections);

            if (!str || str.trim().length == 0)
                return [];

            return JSON.parse(str);
        },

        getCategories: state => {
            let str = localStorage.getItem(cacheItems.categories);

            if (!str || str.trim().length == 0)
                return [];

            return JSON.parse(str);
        },

        getStudents: state => {
            let str = localStorage.getItem(cacheItems.students);

            if (!str || str.trim().length == 0)
                return [];

            return JSON.parse(str);
        },

        isCacheInitialized: state => {
            let str = localStorage.getItem(cacheItems.isCacheInitialized);
            return str == 'true';
        },
    }, // getters

    mutations: {
        setSchools(state, schools) {
            let str = JSON.stringify(schools);
            localStorage.setItem(cacheItems.schools, str);
        },

        setStaffs(state, staffs) {
            let str = JSON.stringify(staffs);
            localStorage.setItem(cacheItems.staffs, str);
        },

        setSections(state, sections) {
            let str = JSON.stringify(sections);
            localStorage.setItem(cacheItems.sections, str);
        },

        setCategories(state, categories) {
            let str = JSON.stringify(categories);
            localStorage.setItem(cacheItems.categories, str);
        },

        setStudents(state, students) {
            let str = JSON.stringify(students);
            localStorage.setItem(cacheItems.students, str);
        },

        setIsCacheInitialized(state, value) {
            let str = `${value}`;
            localStorage.setItem(cacheItems.isCacheInitialized, str);
        },

        clearCache() {
            localStorage.removeItem(cacheItems.schools);
            localStorage.removeItem(cacheItems.staffs);
            localStorage.removeItem(cacheItems.sections);
            localStorage.removeItem(cacheItems.categories);
            localStorage.removeItem(cacheItems.students);
            localStorage.removeItem(cacheItems.isCacheInitialized);
        }
    }, // mutations

    actions: {
        cacheScoringAssessments({ state }, filters) {
            return api.post(`${state.moduleName}/scoring/assessments`, filters);
        },

        cacheScoringAssessmentsAll({ state }, filtersAndScorings) {
            return api.post(`${state.moduleName}/scoring/assessmentsAll`, filtersAndScorings);
        },

        cacheScoringInterventions({ state }, filters) {
            return api.post(`${state.moduleName}/scoring/interventions`, filters);
        },

        getCache({ state, commit }) {
            console.log('getCache({ state, commit })...');

            return api.get(`${state.moduleName}`).then(result => {
                console.log('Got cache!', result);

                commit('setSchools', result.data.schools);
                commit('setStaffs', result.data.staffs);
                commit('setSections', result.data.sections);
                commit('setCategories', result.data.categories);
                commit('setStudents', result.data.students);

                localStorage.setItem(cacheItems.isCacheInitialized, 'true');
            });
        },

        getSchools({ state }) {
            return api.get(`${state.moduleName}/schools`);
        },

        getStaffs({ state }, schoolId) {
            return api.get(`${state.moduleName}/staffs/${schoolId}`)
                .then(result => mapWithFullName(result));
        },

        getSections({ state }, staffId) {
            return api.get(`${state.moduleName}/sections/${staffId}`);
        },

        getStudents({ state }, sectionId) {
            return api.get(`${state.moduleName}/students/${sectionId}`);
        },

        getCategories({ state }) {
            return api.get(`${state.moduleName}/categories`)
                .then(result => {
                    console.log('Result is', result);
                    return result;
                });
        },

        getCachedSectionsByStaff({ state }) {
            return api.get(`${state.moduleName}/sectionsByStaff`);
        },

        getCachedStaffsBySchool({ state }) {
            return api.get(`${state.moduleName}/staffsBySchoolDetails`);
        },

        getCachedUserProfilesByStaffEmailIdPairs({ state }) {
            return api.get(`${state.moduleName}/userProfiles/staffEmailIdPairs`);
        },

        getCachedSections({ state }) {
            return api.get(`${state.moduleName}/sections`);
        },

        getCachedStudentsBySection({ state }) {
            return api.get(`${state.moduleName}/studentsBySectionDetails`);
        },

        getCachedSchools({ state }) {
            return api.get(`${state.moduleName}/schools`);
        },

        getCachedStaffs({ state }) {
            return api.get(`${state.moduleName}/staffs`)
                .then(result => mapWithFullName(result));
        },

        getCachedStudents({ state }) {
            return api.get(`${state.moduleName}/students`);
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
