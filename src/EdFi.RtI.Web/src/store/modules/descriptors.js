/*eslint-disable*/

const api = require("axios");

const cacheItems = {
    categories: 'edfi.categories',
    periods: 'edfi.periods',
};

export default {
    namespaced: true,

    state: {
        moduleName: 'descriptor',
        searchParams: {
            pageSize: 10,
            pageNumber: 1,
        },
    }, // state

    getters: {
        getSearchParams: state => state.searchParams,

        getAssessmentCategories: () => {
            let str = localStorage.getItem(cacheItems.categories);

            if (!str || str.trim().length == 0)
                return [];

            return JSON.parse(str);
        },

        getAssessmentPeriods: () => {
            let str = localStorage.getItem(cacheItems.periods);

            if (!str || str.trim().length == 0)
                return [];

            return JSON.parse(str);
        },
    }, // getters

    mutations: {
        setSearchParams(state, searchParams) {
            state.searchParams = searchParams;
        },

        setAssessmentCategories(state, assessmentCategories) {
            let str = JSON.stringify(assessmentCategories);
            localStorage.setItem(cacheItems.categories, str);
        },

        setAssessmentPeriods(state, assessmentPeriods) {
            let str = JSON.stringify(assessmentPeriods);
            localStorage.setItem(cacheItems.periods, str);
        },
    }, // mutations

    actions: {
        getAssessmentCategories({ state, commit }, { instanceId, schoolYear }) {
            return api.get(`${state.moduleName}/assessmentCategory`, { params: { instanceId, schoolYear } })
                .then(result => {
                    commit("setAssessmentCategories", result.data);
                    return result.data;
                });
        },

        getAssessmentPeriods({ state, commit }, { instanceId, schoolYear }) {
            return api.get(`${state.moduleName}/assessmentPeriod`, { params: { instanceId, schoolYear } })
                .then(result => {
                    commit("setAssessmentPeriods", result.data);
                    return result.data;
                });
        },

        getStaffDescriptors({ state }) {
            return api.get(`${state.moduleName}/staffDescriptors`);
        },
    }, // actions
}
