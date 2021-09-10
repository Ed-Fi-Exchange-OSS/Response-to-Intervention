<template>
  <div>
    <v-container>
      <AppToolbar
        :buttons="toolbarButtons"
        :disabledButtons="isLoadingStaffsBySchool || isRefreshingCache"
        :icon="'mdi-badge-account-outline'"
        :title="'Staffs by School'"
      />

      <p class="mt-4">Last refreshed: {{ lastRefreshedDateLocal }}</p>

      <StaffsBySchoolListView
        :items="staffsBySchool"
        :isLoading="isLoadingStaffsBySchool"
      />

      <TableSkeletonLoader
        v-if="isLoadingStaffsBySchool"
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
import StaffsBySchoolListView from "./StaffsBySchoolListView";
import YesNoDialog from "@/components/dialogs/YesNoDialog";

import { dates } from "@/mixins/mixins";

export default {
  data() {
    return {
      lastRefreshedDate: null,
      staffsBySchool: [],
      isLoadingLastRefreshedDate: false,
      isLoadingStaffsBySchool: false,
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
    this.getStaffsBySchoolRequest();
  },

  computed: {
    lastRefreshedDateLocal() {
      if (!this.lastRefreshedDate) return "";

      return dates.methods.toLocalShort(this.lastRefreshedDate);
    },
  },

  methods: {
    ...mapActions({
      getCachedStaffsBySchool: "cache/getCachedStaffsBySchool",
      getStaffsBySchoolLastRefreshedDate:
        "cacheRefresh/getStaffsBySchoolLastRefreshedDate",
      refreshStaffsBySchool: "cacheRefresh/refreshStaffsBySchool",
    }),

    getLastRefreshedDateRequest() {
      this.isLoadingLastRefreshedDate = true;
      this.staffsBySchool = [];

      this.getStaffsBySchoolLastRefreshedDate()
        .then((response) => (this.lastRefreshedDate = response.data))
        .catch(() => (this.lastRefreshedDate = false));
    },

    getStaffsBySchoolRequest() {
      this.isLoadingStaffsBySchool = true;

      this.getCachedStaffsBySchool()
        .then((response) => (this.staffsBySchool = response.data))
        .catch(() => {})
        .finally(() => (this.isLoadingStaffsBySchool = false));
    },

    onRefreshClicked() {
      this.getLastRefreshedDateRequest();
      this.getStaffsBySchoolRequest();
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

      this.refreshStaffsBySchool()
        .then(() => {
          this.$snotify.success("Cache has been refreshed!", "", {
            timeout: 2000,
          });
          this.getLastRefreshedDateRequest();
          this.getStaffsBySchoolRequest();
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
    StaffsBySchoolListView,
    YesNoDialog,
  }, // components

  mixins: [dates],
};
</script>
