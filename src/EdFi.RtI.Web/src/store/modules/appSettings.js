/*eslint-disable*/
const api = require("axios");

export default {
    namespaced: true,

    state: {
        moduleName: 'appSettings',
    },

    actions: {
        getAdminsDescriptors({ state }){
            return api.get(`${state.moduleName}/admins`);
        },

        getTeachersDescriptors({ state }){
            return api.get(`${state.moduleName}/teachers`);
        },
    },
}