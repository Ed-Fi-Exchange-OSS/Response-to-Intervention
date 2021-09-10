/*eslint-disable*/

const api = require("axios");

export default {
    namespaced: true,

    state: {
        moduleName: "scoringAssessments",
        searchParams: {
            category: "All",
            getFromCache: true,
            showInactiveStudents: null,
            storeInCache: true,
            uniqueSectionCode: "",
            onlyFromCurrentNamespace: null,
        },
    }, // state

    getters: {
        getSearchParams: state => state.SearchParams,
    }, // getters

    mutations: {
        setSearchParams(state, SearchParams) {
            state.SearchParams = SearchParams;
        },
    }, // mutations

    actions: {
        /**
         * The provided scoring object should always have the new
         * assessment in the end of the associations array.
         */
        addScoring({ state }, scoring) {
            return api.post(`${state.moduleName}`, scoring);
        },

        createAssociation({ state }, { studentUniqueId /* string */, assessmentId /* string */, administrationDate /* string */ }) {
            const body = { studentUniqueId, assessmentId, administrationDate };
            return api.post(`${state.moduleName}/association`, body);
        },

        deleteStudentAssessment({state}, {studentAssessmentId, uniqueSectionCode, identifier}){
            return api.delete(`${state.moduleName}?studentAssessmentId=${studentAssessmentId}&uniqueSectionCode=${uniqueSectionCode}&identifier=${identifier}`);
        },

        deleteStudentAssessmentAll({state}, {uniqueSectionCode, assessmentId, date}){
            return api.delete(`${state.moduleName}/deleteAll?uniqueSectionCode=${uniqueSectionCode}&assessmentId=${assessmentId}&date=${date}`);
        },
        
        search({ state }, searchParams) {
            return api.post(`${state.moduleName}/search`, searchParams)
            .then(result => {
                result.data = mapWithGuiFields(result.data);
                return result;
            });
        },

        updateScorings({ state }, scorings) {
            return api.put(`${state.moduleName}`, scorings);
        },
    }, // actions
}

const mapWithGuiFields = (scorings) => {
    return scorings.map(scoring => {
        scoring.associations = scoring.associations.map(association => {
            association.isInputDisabled = true;
            return association;
        });
        return scoring;
    });
};
