<template>
  <v-app id="app" :class="{ 'bg-light': isLightMode, 'bg-dark': isDarkMode }">

    <!-- global components -->
    <div>
      <vue-snotify></vue-snotify>
      <!-- 
      <SharedIdleDialog/>
       -->
    </div>

    <!-- loading app -->
    <div v-if="isLoadingApp" class="d-flex align-center" style="height: 100vh">
      <div style="width: 100%">
        <!-- 
        <SharedLoader/>
         -->
      </div>
    </div>
    
    <!-- app loaded -->
    <div :class="{ hidden: isLoadingApp }">
      <router-view :key="$route.fullPath"></router-view>
    </div>

    <StatusFooter/>

  </v-app>
</template>

<style scoped>
.hidden {
  visibility: hidden;
}

.bg-light {
  background-color: #F2F2F2 !important;
}

.bg-dark {
  background-color: #0a0a0a !important;
}
</style>

<script>

import { mapActions, mapGetters, mapMutations, mapState } from "vuex"
import navItemsGetter from "@/utils/navItemsGetter";
import session from './utils/session'
import environment from './environment';
import api from './api';
import storage from './storage';
import { notNullNorWhitespace } from './utils/validators';

export default {

  data: () => ({
    isLoadingApp: false,
    edFiVersion: '',
  }), // data

  mounted() {
    //this.signOutOidc(); // TODO Comment for dev/prod. Uncomment to sign out manually (testing purposes)
    this.init()
  }, // mounted

  computed: {
    ...mapGetters({
      oidcIsAuthenticated: "auth/oidcIsAuthenticated",
    }),

    isLightMode () {
      return this.darkMode == undefined || this.darkMode == null || this.darkMode == false
    },

    isDarkMode () {
      return this.darkMode == true
    },

    ...mapState([
      "selectedTenantId",
      "darkMode"
    ])
  }, // computed

  methods: {
    ...mapActions({
      getApps: "session/getApps",
      getMe: "session/getMe",
      loadSelectedTenant: "loadSelectedTenant",
      getAssessmentReportingMethod: "types/getAssessmentReportingMethod",
      getAssessmentResult: "types/getAssessmentResult",
      getDeliveryMethod: "types/getDeliveryMethod",
      getInterventionClass: "types/getInterventionClass",
      getAssessmentCategories: "descriptors/getAssessmentCategories",
      getAssessmentPeriods: "descriptors/getAssessmentPeriods",
      getAssessmentFamily: "assesments/getAssessmentFamily",
      signOutOidc: "auth/signOutOidc",
      getFeaturesSettings: "features/getFeaturesSettings",
    }),

    ...mapMutations({
      setMe: "setMe",
      setNavItems: "setNavItems",
    }),

    async init() {
      if (environment.app.isStartupModeHosted()) {
        this.initHosted()
        return
      }

      if (environment.app.isStartupModeStandalone()) {
        this.initStandalone()
        return
      }

      throw `init method has not been implemented for startupMode \"${environment.app.getStartupMode()}\"`
    },

    async initHosted() {
      this.isLoadingApp = true
      
      if (this.oidcIsAuthenticated) {
        const hasEdFiVersion = await this.hasEdFiVersion()

        if (!hasEdFiVersion) {
          this.isLoadingApp = false
          return
        }

        const meResponse = await this.getMe()
        this.setMe(meResponse.data)

        this.$store.dispatch("loadUserPrefs")

        await this.loadSelectedTenant(this.$router)

        console.log("getting my user profile...")
        const myUserProfile = await api.me.getUserProfile()
        console.log("myUserProfile:", myUserProfile)
        storage.local.setUserProfile(myUserProfile)
        console.log("Set user profile!");

        // This must be used after getting the user profile
        const navItems = await navItemsGetter();
        this.setNavItems(navItems);
        
        this.isLoadingApp = false;
      }
    },

    async initStandalone() {
      // TODO What to initialize in Standalone mode?
      this.isLoadingApp = true
      this.isLoadingApp = false
    },

    async hasEdFiVersion() {
      const edFiVersion = await api.settings.getDefaultEdFiVersion()
      return notNullNorWhitespace(edFiVersion)
    }
  }, // methods

  watch: {
    oidcIsAuthenticated() {
      if (this.oidcIsAuthenticated) {
        this.init()
      } else {
        console.log("User not authenticated. Wait for sign in page...")
      }
    },

    darkMode (v) {
      this.$vuetify.theme.dark = !!v
    },
  }, // watch
}
</script>

<style>
.app-border-red {
  border: 2px solid red;
}

.app-border-green {
  border: 2px solid green;
}

.app-border-blue {
  border: 2px solid blue;
}

.app-transition-ease {
  transition: all ease 0.2s;
}

.app-scale-hover:hover {
  transform: scale(1.1);
}

.app-cursor-pointer:hover {
  cursor: pointer;
}

.app-w-100 {
  width: 100%;
}

.app-center-x {
  display: flex;
  justify-content: center;
}

.cursor-pointer:hover {
  cursor: pointer;
}
</style>
