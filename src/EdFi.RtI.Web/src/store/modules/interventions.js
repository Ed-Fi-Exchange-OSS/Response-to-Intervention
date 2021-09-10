const api = require("axios")

export default {
  namespaced: true,

  state: {
    moduleName: "intervention",
    searchParams: {
      getFromCache: true,
      pageIndex : 1,
      pageSize: 10,
      storeInCache: false,
      searchValue: "",
      sortDesc: false,
      sortField: "beginDate",
    }
  }, // state

  getters: {
    getSearchParams: (state) => state.searchParams
  }, // getters

  mutations: {
    setSearchParams (state, searchParams) {
      state.searchParams = searchParams
    }
  }, // mutations

  actions: {
    create ({ state }, intervention) {
      intervention.safeParse()
      return api.post(`${state.moduleName}`, intervention)
    },

    getById ({ state }, interventionId) {
      return api.get(`${state.moduleName}/${interventionId}`)
    },

    getTotalCount({ state }) {
      return api.get(`${state.moduleName}/count`);
    },

    update ({ state }, intervention) {
      return api.put(`${state.moduleName}/${intervention.id}`, intervention)
    },

    delete ({ state }, interventionId){
      return api.delete(`${state.moduleName}/deleteInterventionById/${interventionId}`)
    },

    search({ state }){
      return api.post(`${state.moduleName}/search`, state.searchParams);
    },

    searchWithParams({ state }, searchParams) {
      return api.post(`${state.moduleName}/search`, searchParams);
    },
  } // actions
}
