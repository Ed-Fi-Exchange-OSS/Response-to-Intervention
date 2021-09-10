/*eslint-disable*/

const api = require("axios");

export default {
    namespaced: true,

    state: {
        moduleName: 'scoringInterventions',
        filters: {
            staffUniqueId: '',
            sectionId: '',
            showInactiveStudents: false,
        },
    }, // state

    getters: {
        getFilters: state => state.filters,
    }, // getters

    mutations: {
        setFilters(state, filters) {
            state.filters = filters;
        },
    }, // mutations

    actions: {
        createAssociation({ state }, { studentUniqueId /* string */, interventionId /* string */ }) {
            const body = { studentUniqueId, interventionId };
            return api.post(`${state.moduleName}`, body);
        },

        deleteAssociation({ state }, association) {
            return api.put(`${state.moduleName}/delete`, association);
        },

        getScorings({ state }, filters) {
            return api.post(`${state.moduleName}/scorings?getFromCache=false&storeInCache=false`, filters);
        },

        getScoringsByStudent({ state }, studentUniqueId) {
            return api.get(`${state.moduleName}/scorings/${studentUniqueId}`);
        },

        updateScorings({ state }, scorings) {
            return api.put(`${state.moduleName}`, scorings);
        },
    }, // actions
};
