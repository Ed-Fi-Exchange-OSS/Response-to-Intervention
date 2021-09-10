<template>
  <div>
    <v-container>
      <AppToolbar
        :buttons="toolbarButtons"
        :disabledButtons="isLoadingProfiles || isRefreshingCache"
        :icon="'mdi-account-group'"
        :title="'User Profiles'"
      />

      <p class="mt-4">Last refreshed: {{ lastRefreshedDateLocal }}</p>

      <UserProfilesListView :items="profiles" :isLoading="isLoadingProfiles" />

      <TableSkeletonLoader
        v-if="isLoadingProfiles"
        :column-count="3"
        :row-count="6"
      />
    </v-container>

    <LoadingDialog ref="loadingDialog" />
    <YesNoDialog ref="yesNoDialog" />
  </div>
</template>

<script>
import { mapActions } from "vuex";
import AppToolbar from "@/components/widgets/AppToolbar";
import LoadingDialog from "@/components/dialogs/LoadingDialog";
import TableSkeletonLoader from "@/components/widgets/TableSkeletonLoader";
import UserProfilesListView from "./UserProfilesListView";
import YesNoDialog from "@/components/dialogs/YesNoDialog";

import { dates } from "@/mixins/mixins";

export default {
  data() {
    return {
      lastRefreshedDate: null,
      profiles: [],
      isLoadingLastRefreshedDate: false,
      isLoadingProfiles: false,
      isRefreshingCache: false,
      toolbarButtons: [
        {
          icon: "mdi-arrow-left",
          text: "Back",
          click: () => this.$router.push("/settings/caching"),
        },
        {
          icon: "mdi-database-refresh",
          text: "Refresh Cache",
          click: () => this.onRefreshCacheClicked(),
        },
      ],
    };
  }, // data

  mounted() {
    this.getLastRefreshedDateRequest();
    this.getUserProfilesRequest();
  },

  computed: {
    lastRefreshedDateLocal() {
      if (!this.lastRefreshedDate) return "";

      return dates.methods.toLocalShort(this.lastRefreshedDate);
    },
  },

  methods: {
    ...mapActions({
      getCachedUserProfilesByStaffEmailIdPairs:
        "cache/getCachedUserProfilesByStaffEmailIdPairs",
      getUserProfilesStaffEmailIdPairsLastRefreshedDate:
        "cacheRefresh/getUserProfilesStaffEmailIdPairsLastRefreshedDate",
      refreshStaffEmailIdPairs: "cacheRefresh/refreshStaffEmailIdPairs",
    }),

    getLastRefreshedDateRequest() {
      this.isLoadingLastRefreshedDate = true;

      this.getUserProfilesStaffEmailIdPairsLastRefreshedDate()
        .then((response) => (this.lastRefreshedDate = response.data))
        .catch(() => (this.lastRefreshedDate = false));
    },

    getUserProfilesRequest() {
      this.isLoadingProfiles = true;

      this.getCachedUserProfilesByStaffEmailIdPairs()
        .then((response) => (this.profiles = response.data))
        .finally(() => (this.isLoadingProfiles = false))
        .catch(() => {});
    },

    onRefreshClicked() {
      this.getLastRefreshedDateRequest();
      this.getUserProfilesRequest();
    },

    onRefreshCacheClicked() {
      this.$refs.yesNoDialog
        .open({
          title: "Refresh Cache",
          message:
            "Are you sure you want to refresh the cache? This might take a few minutes.",
        })
        .then(() => this.refreshCache())
        .catch(() => {});
    },

    refreshCache() {
      this.isRefreshingCache = true;
      this.$refs.loadingDialog.open({
        title: "Refreshing Cache",
        message: "Refreshing cache. This might take a few minutes.",
      });

      this.refreshStaffEmailIdPairs()
        .then(() => {
          this.$snotify.success("Cache has been refreshed!", "", {
            timeout: 2000,
          });
          this.getLastRefreshedDateRequest();
          this.getUserProfilesRequest();
        })
        .catch((error) =>
          this.$snotify.error(`Could not refresh cache: ${error}`, "", {
            timeout: 4000,
          })
        )
        .finally(() => {
          this.isRefreshingCache = false;
          this.$refs.loadingDialog.close();
        });
    },
  }, // methods

  components: {
    AppToolbar,
    LoadingDialog,
    TableSkeletonLoader,
    UserProfilesListView,
    YesNoDialog,
  }, // components

  mixins: [dates],
};
</script>
