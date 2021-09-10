<template>
  <div :class="$config.app.hideTopNav && 'topnavhidden'">

    <SharedSideNav
      :full="isSideNavExpanded && !isRouteScoring"
      @navBtnClick="isSideNavExpanded = !isSideNavExpanded"/>

    <v-main>
      <SharedTopBar v-if="isStartupModeHosted" title="" :apps="apps"/>
      <TopBar v-if="isStartupModeStandalone"/>

      <router-view :key="$route.fullPath"></router-view>

      <div class="mb-12"></div>
    </v-main>

    <LoadingDialog ref="loadingDialog" />

  </div>
  
</template>

<script>
/* eslint-disable */

import { mapActions, mapGetters, mapMutations, mapState } from "vuex"
import OrganizationDropdown from "../components/widgets/OrganizationDropdown"
import environment from '@/environment'

export default {
	data: () => ({
    isSideNavExpanded: true,
  }), // data
    
  mounted() {
    
  }, // mounted

	computed: {
    isRouteScoring() {
      return this.$route.fullPath.includes("scoring");
    },

		...mapGetters({
      apps: "session/getApps",
    }),
    
    ...mapState([
      "selectedTenantId",
    ]),

    isStartupModeHosted() {
      return environment.app.isStartupModeHosted()
    },

    isStartupModeStandalone() {
      return environment.app.isStartupModeStandalone()
    },
	}, // computed

	methods: {
    ...mapMutations({
      setMe: 'setMe',
      setSelectedTenantId: 'setSelectedTenantId',
      setNavItems: "setNavItems",
    }),

    ...mapActions({
      getApps: 'session/getApps',
      getMe: 'session/getMe',
      loadSelectedTenant: 'loadSelectedTenant',
      signOut: "session/signOut",
      signOutOidc: 'auth/signOutOidc',
      getFeaturesSettings: "features/getFeaturesSettings",
    }),

    getAppsRequest() {
      // Shared topnav is shown, so get apps
      if (this.isModeHosted) {
        
        console.log("Getting apps with tenant id: " + this.selectedTenantId);

        this.getApps(this.selectedTenantId).then(result => {
          console.log("Apps are:", result.data);
        });
      }
      // Shared topnav is hidden, so don't get apps
      else {
        console.log("Do NOT get the apps");
      }
    },
	}, // methods

	components: {
		OrganizationDropdown,
	}, // components
};
</script>
