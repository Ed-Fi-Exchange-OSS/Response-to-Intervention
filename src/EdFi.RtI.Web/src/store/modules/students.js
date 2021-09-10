/*eslint-disable*/

const api = require("axios");

export default {
    namespaced: true,

    state: {
        moduleName: 'student',
        searchParams: {
            pageSize: 10,
            pageNumber: 1,
        },
    }, // state

    getters: {
        getSearchParams: state => state.searchParams,
    }, // getters

    mutations: {
        setSearchParams(state, searchParams) {
            state.searchParams = searchParams;
        },
    }, // mutations

    actions: {
        getBySection({ state }, sectionId) {
            return api.get(`${state.moduleName}/section/${sectionId}`);
        },
    }, // actions
}
