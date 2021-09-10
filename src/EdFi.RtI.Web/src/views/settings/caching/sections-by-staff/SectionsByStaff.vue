<template>
  <div>
    <v-container>
      <AppToolbar
        :buttons="toolbarButtons"
        :disabledButtons="isLoadingSections || isRefreshingCache"
        :icon="'mdi-format-list-bulleted-type'"
        :title="'Sections by Staff'"
      />

      <p class="mt-4">Last refreshed: {{ lastRefreshedDateLocal }}</p>

      <SectionsByStaffListView
        :items="sections"
        :isLoading="isLoadingSections"
      />

      <TableSkeletonLoader
        v-if="isLoadingSections"
        :column-count="4"
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
import SectionsByStaffListView from "./SectionsByStaffListView";
import YesNoDialog from "@/components/dialogs/YesNoDialog";

import { dates } from "@/mixins/mixins";

export default {
  data() {
    return {
      lastRefreshedDate: null,
      sections: [],
      isLoadingLastRefreshedDate: false,
      isLoadingSections: false,
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
    this.getSectionsByStaffRequest();
  },

  computed: {
    lastRefreshedDateLocal() {
      if (!this.lastRefreshedDate) return "";

      return dates.methods.toLocalShort(this.lastRefreshedDate);
    },
  },

  methods: {
    ...mapActions({
      getCachedSectionsByStaff: "cache/getCachedSectionsByStaff",
      getSectionsByStaffLastRefreshedDate:
        "cacheRefresh/getSectionsByStaffLastRefreshedDate",
      refreshSectionsByStaff: "cacheRefresh/refreshSectionsByStaff",
    }),

    getLastRefreshedDateRequest() {
      this.isLoadingLastRefreshedDate = true;

      this.getSectionsByStaffLastRefreshedDate()
        .then((response) => (this.lastRefreshedDate = response.data))
        .catch(() => (this.lastRefreshedDate = "----------"));
    },

    getSectionsByStaffRequest() {
      this.isLoadingSections = true;
      this.sections = [];

      this.getCachedSectionsByStaff()
        .then((response) => (this.sections = response.data))
        .finally(() => (this.isLoadingSections = false));
    },

    onRefreshClicked() {
      this.getLastRefreshedDateRequest();
      this.getSectionsByStaffRequest();
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

      this.refreshSectionsByStaff()
        .then(() => {
          this.$snotify.success("Cache has been refreshed!", "", {
            timeout: 2000,
          });
          this.getLastRefreshedDateRequest();
          this.getSectionsByStaffRequest();
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
    SectionsByStaffListView,
    YesNoDialog,
  }, // components

  mixins: [dates],
};
</script>
