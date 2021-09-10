const api = require("axios")

const cacheItems = {
  results: "edfi.results",
  reporting: "edfi.reporting",
  delivery: "edfi.delivery",
  class: "edfi.class,"
}

export default {
  namespaced: true,

  state: {
    moduleName: "types",
    searchParams: {
      getAll: false,
      pageSize: 10,
      pageNumber: 1
    }
  }, // state

  getters: {
    getSearchParams: (state) => state.searchParams,

    getAssessmentResults: () => {
      const str = localStorage.getItem(cacheItems.results)

      if (!str || str.trim().length == 0)
      {return []}

      return JSON.parse(str)
    },

    getAssessmentReportingMethods: () => {
      const str = localStorage.getItem(cacheItems.reporting)

      if (!str || str.trim().length == 0)
      {return []}

      return JSON.parse(str)
    },

    getDeliveryMethods: () => {
      const str = localStorage.getItem(cacheItems.delivery)

      if (!str || str.trim().length == 0)
      {return []}

      return JSON.parse(str)
    },

    getInterventionClasses: () => {
      const str = localStorage.getItem(cacheItems.class)

      if (!str || str.trim().length == 0)
      {return []}

      return JSON.parse(str)
    }
  }, // getters

  mutations: {
    setSearchParams (state, searchParams) {
      state.searchParams = searchParams
    },

    setAssessmentResults (state, assessmentResults) {
      const str = JSON.stringify(assessmentResults)
      localStorage.setItem(cacheItems.results, str)
    },

    setAssessmentReportingMethods (state, assessmentReportingMethods) {
      const str = JSON.stringify(assessmentReportingMethods)
      localStorage.setItem(cacheItems.reporting, str)
    },

    setDeliveryMethods (state, deliveryMethods){
      const str = JSON.stringify(deliveryMethods)
      localStorage.setItem(cacheItems.delivery, str)
    },

    setInterventionClasses (state, interventionClasses){
      const str = JSON.stringify(interventionClasses)
      localStorage.setItem(cacheItems.class, str)
    }
  }, // mutations

  actions: {
    getAssessmentResult ({ state, commit }, { getAll }){
      state.searchParams.getAll = getAll != null ? getAll : false
      return api.get(`${state.moduleName}/assessmentResult`, {
        params: state.searchParams
      })
        .then((result) => {
          commit("setAssessmentResults", result.data)
          return result.data
        })
    },

    getAssessmentReportingMethod ({ state, commit }, { getAll }){
      state.searchParams.getAll = getAll != null ? getAll : false
      return api.get(`${state.moduleName}/assessmentReportingMethod`, {
        params: state.searchParams
      })
        .then((result) => {
          commit("setAssessmentReportingMethods", result.data)
          return result.data
        })
    },

    getDeliveryMethod ({ state, commit }, { getAll }){
      state.searchParams.getAll = getAll != null ? getAll : false
      return api.get(`${state.moduleName}/deliveryMethod`, {
        params: state.searchParams
      })
        .then((result) => {
          commit("setDeliveryMethods", result.data)
          return result.data
        })
    },

    getInterventionClass ({ state, commit }, { getAll }){
      state.searchParams.getAll = getAll != null ? getAll : false
      return api.get(`${state.moduleName}/interventionClass`, {
        params: state.searchParams
      })
        .then((result) => {
          commit("setInterventionClasses", result.data)
          return result.data
        })
    }
  }//actions
}
