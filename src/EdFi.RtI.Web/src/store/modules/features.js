/* eslint-disable */

import api from "axios";

export default {
    namespaced: true,
    
    state: {
        moduleName: "features",
    }, // state
    
    getters: {
        
    }, // getters
    
    mutations: {
        
    }, // mutations
    
    actions: {
        async getFeaturesSettings({ state }) {
            const url = `${state.moduleName}`;
            const response = await api.get(url);
            return response.data;
        },

        updateFeatures({ state }, { showAssessments, showInterventions }) {
            const url = `${state.moduleName}`;
            const request = { showAssessments, showInterventions };
            return api.put(url, request);
        },
    }, // actions
    
}
