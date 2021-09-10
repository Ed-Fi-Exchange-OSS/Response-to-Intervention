/*eslint-disable*/
const api = require("axios");
const cacheItems = {
    assessmentFamily: 'edfi.assessmentFamily',
};

export default {
    namespaced: true,

    state: {
        moduleName: 'assessment',
        searchParams: {
            currentNamespace: true,
            getFromCache: true,
            pageIndex : 1,
            pageSize: 10,
            storeInCache: true,
            searchValue: "",
            sortDesc: false,
            sortField: "lastModifiedDate",
        },
    }, // state

    getters: {
        getSearchParams: state => state.searchParams,

        getAssessmentFamily: () => {
            let str = localStorage.getItem(cacheItems.assessmentFamily);

            if(!str || str.trim().length == 0)
                return [];
            
            return JSON.parse(str);
        },
    }, // getters

    mutations: {
        setSearchParams(state, searchParams) {
            state.searchParams = searchParams;
        },

        setAssessmentFamily(state, assessmentFamily) {
            let str = JSON.stringify(assessmentFamily);
            localStorage.setItem(cacheItems.assessmentFamily, str);
        },
    }, // mutations

    actions: {
        create({ state }, assessment) {
            console.log('Before safe parse:', assessment);
            assessment.safeParse();
            
            console.log('After safe parse:', assessment);
            return api.post(`${state.moduleName}`, assessment);
        },

        getById({ state }, assessmentId) {
            return api.get(`${state.moduleName}/${assessmentId}`);
        },

        getTotalCount({ state }, searchParams) {
            return api.post(`${state.moduleName}/count`, searchParams);
        },

        update({ state }, assessment) {
            console.log("Its Update");
            console.log('Before safe parse:', assessment);

            console.log('After safe parse:', assessment);
            return api.put(`${state.moduleName}/${assessment.id}`, assessment);
        },

        delete({ state }, assessmentId){
            console.log("Id before delete: ", assessmentId);
            return api.delete(`${state.moduleName}/deleteAssessmentById/${assessmentId}`);
        },

        getAssessmentFamily({state, commit}){
            return api.get(`${state.moduleName}/assessmentFamily`)
            .then(result => {
                commit("setAssessmentFamily", result.data);
                return result.data;
            });
        },

        search({ state }){
            console.log("Before show: ", state.searchParams);
            return api.post(`${state.moduleName}/search`, state.searchParams);
        },
    }, // actions
}
