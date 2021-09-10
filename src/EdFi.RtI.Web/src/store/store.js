import Vue from "vue"
import Vuex from "vuex"
import { get } from "lodash"
import axios from "axios"

//import { vuexOidcCreateStoreModule } from "vuex-oidc"

import actions from "@bit/edgraph.shared.store-actions"
import notify from "@bit/edgraph.shared.store-notify"

import assesments from "./modules/assesments"
import interventions from "./modules/interventions"
import session from "./modules/session"
import students from "./modules/students"
import descriptors from "./modules/descriptors"
import catalog from "./modules/catalog"
import scoringAssessments from "./modules/scoringAssessments"
import scoringInterventions from "./modules/scoringInterventions"
import types from "./modules/types"
import cache from "./modules/cache"
import cacheRefresh from "./modules/cacheRefresh"
import edgraph from "./modules/edgraph"
import appSettings from "./modules/appSettings"
import features from "./modules/features"

Vue.use(Vuex)

const showLogs = false

export const store = new Vuex.Store({

  state: {
    darkMode: false,

    me: {
      tenants: []
    },

    selectedTenantId: undefined,

    sidenavItems: [
      {
        title: "Scoring",
        type: "heading"
      },
      {
        icon: "mdi-view-dashboard",
        title: "Assessments",
        path: "/scoring/assessments"
      },
      {
        icon: "mdi-text-box-check",
        title: "Interventions",
        path: "/scoring/interventions"
      },
      {
        title: "Administration",
        type: "heading"
      },
      {
        icon: "mdi-view-dashboard",
        title: "Manage Assessments",
        path: "/assesments"
      },
      {
        icon: "mdi-text-box-check",
        title: "Manage Interventions",
        path: "/interventions"
      },
      {
        icon: "mdi-cogs",
        title: "Settings",
        path: "/settings/caching"
      }
    ]
  }, // state

  mutations: {
    setDarkMode (state, value) {
      state.darkMode = value
    },

    setNavItems (state, navItems) {
      state.sidenavItems = navItems
    },

    setMe (state, me) {
      if (showLogs == true)
      {console.log("setMe", me)}

      state.me = me

    },

    setSelectedTenantId (state, tenantId) {
      state.selectedTenantId = tenantId
    }
  }, // mutations

  actions: {
    ...actions,
    async savePrefs ({ state, dispatch, getters }, [
      code,
      value
    ]){
      console.log(Vue.prototype.$config)
      return await axios.post(`${Vue.prototype.$config.api.basePlatformServiceUri}/tenant/me/preferences`, {
        code,
        value
      })
    },
    // chandeDarkMode({ state, commit }, value /* boolean */) {
    //     commit("setDarkMode", value);
    // },

    async changeDefaultTenant({ state, commit, dispatch }, tenantId) {
        if (showLogs == true)
            console.log("changeDefaultTenant", state.selectedTenantId, tenantId);

        const currentTenantIsNull = state.selectedTenantId == undefined || state.selectedTenantId == null;
        const newTenantIsSet = !currentTenantIsNull && state.selectedTenantId != tenantId;

        const body = {
            code: "selectedTenantId",
            value: tenantId,
        };

        commit("setSelectedTenantId", tenantId);

        await dispatch('session/saveUserPreferences', body, { root: true });

        if (newTenantIsSet) {
            await dispatch('auth/authenticateOidc'); // Re-sign in to update app context (prevents 403 error)
        } else {
            await dispatch('session/getApps', tenantId); // Get apps, app context is the same
        }
    },

    async loadSelectedTenant({ state, dispatch }, next) {
        const tenants = state.me.tenants;
        const savedPrefs = state.me.preferences.find(pref => pref.code == "selectedTenantId");

        // check if there is already tenant selected
        const savedTenantId = get(savedPrefs, "value")

        // check if the tenant is still in list
        const tenantInList = tenants.find(tenant => tenant.tenantId === savedTenantId)

        if (savedTenantId && !tenantInList) {
            if (showLogs == true)
                console.log("❌ TenantId is saved but is not in user's current tenants. Loading apps for default tenant.");
            await dispatch('changeDefaultTenant', "tenant-00000000-0000-0000-0000-000000000000");
        }
        else if (savedTenantId && tenantInList) {
            if (showLogs == true)
                console.log("✅ Tenant found! Loading apps.")
            await dispatch('changeDefaultTenant', savedTenantId);
        }
        else {
            if (showLogs == true)
                console.log("No preference found for tenant!");

            // select automatically the tenant in case its only 1
            if (tenants.length == 1) {
                if (showLogs == true)
                    console.log("Found just 1 tenant. Selecting it by default")
                await dispatch("changeDefaultTenant", tenants[0].tenantId);
            }

            if (tenants.length > 1) {
                if (showLogs == true)
                    console.log("found more than 1 tenants. moving to tenant selector page");
                next.push("/select-tenant");
            }

            if (tenants.length <= 0) {
                if (showLogs == true)
                    console.log("zero tenants associated. moving to NO tenant page")
                next.push("/no-tenant");
            }
        }
    },
  },

  modules: {
    notify,
    assesments,
    interventions,
    session,
    students,
    descriptors,
    catalog,
    scoringAssessments,
    scoringInterventions,
    types,
    cache,
    cacheRefresh,
    edgraph,
    cacheRefresh,
    appSettings,
    features
  } // modules
})
