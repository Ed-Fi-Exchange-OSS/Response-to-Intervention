/*eslint-disable*/

const api = require("axios");

export default {
    namespaced: true,

    state: {
        moduleName: 'catalog',
        currentSchoolId: '',
        currentSchool: undefined, // any
        searchParams: {
            getAll: false,
            pageNumber: 1,
            pageSize: 10,
            sort: "fullName",
            desc: false
        }
    }, // state

    getters: {
        getCurrentSchoolId: state => state.currentSchoolId,
        getCurrentSchool: state => state.currentSchool,
        getSearchParams: state => state.searchParams,
    }, // getters

    mutations: {
        setCurrentSchoolId(state, schoolId) {
            state.currentSchoolId = schoolId;
        },

        setCurrentSchool(state, school) {
            state.currentSchool = school;
        },

        setSearchParams(state, searchParams) {
            state.searchParams = searchParams;
        },
    }, // mutations

    actions: {
        getSections({ state }) {
            return api.get(`${state.moduleName}/sections`);
        },

        getSectionsByStaff({ state }, staffUniqueId) {
            return api.get(`${state.moduleName}/sections/${staffUniqueId}`);
        },

        getSchools({ state }) {
            return api.get(`${state.moduleName}/schools`);
        },

        getSchoolsByStaff({ state }, staffUniqueId) {
            return api.get(`${state.moduleName}/schools/staff/${staffUniqueId}`);
        },

        getStudents({ state }, { getAll }) {
            state.searchParams.getAll = getAll != null ? getAll : false
            return api.get(`${state.moduleName}/students`, {
                params: state.searchParams
            })
            .then(result => mapWithFullName(result));
        },

        getStaffsBySchool({ state }, schoolId) {
            return api.get(`${state.moduleName}/staffs/school/${schoolId}`)
            .then(result => mapWithFullName(result));
        },
    }, // actions
}

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
