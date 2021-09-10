<template>
  <div>
    <v-container>
      <AppToolbar
        :title="toolbarTitle"
        :icon="toolbarIcon"
        :buttons="buttons"
        :disabledButtons="isLoading"
        :showSearch="false"
        class="mb-5"
      >
      </AppToolbar>

      <p class="mt-4">Last refreshed: {{ lastRefreshedDateLocal }}</p>

      <SectionsListVuew :items="cachedData" :isLoading="isLoading">
      </SectionsListVuew>

      <TableSkeletonLoader v-if="isLoading" :column-count="0" :row-count="6" />

      <YesNoDialog ref="yesNoDialog" />
      <LoadingDialog ref="loadingDialog" />
    </v-container>
  </div>
</template>

<script>
import AppToolbar from "@/components/widgets/AppToolbar.vue";
import TableSkeletonLoader from "@/components/widgets/TableSkeletonLoader";
import SectionsListVuew from "./SectionsListView.vue";
import { mapActions } from "vuex";
import YesNoDialog from "@/components/dialogs/YesNoDialog.vue";
import LoadingDialog from "@/components/dialogs/LoadingDialog.vue";
import { dates } from "@/mixins/mixins";

export default {
  data() {
    return {
      lastRefreshedDate: null,
      isLoadingLastRefreshedDate: false,
      isLoading: false,
      toolbarTitle: "Sections",
      toolbarIcon: "mdi-note-multiple",
      cachedData: [],
      buttons: [
        {
          icon: "mdi-arrow-left",
          text: "Back",
          click: () => {
            this.$router.push("/settings/caching");
          },
        },
        {
          icon: "mdi-database-refresh",
          text: "Refresh Cache",
          click: () => {
            this.refresh();
          },
        },
      ],
    };
  }, // data

  mounted() {
    this.getLastRefreshedDateRequest();
    this.getSectionsRequest();
  },

  computed: {
    lastRefreshedDateLocal() {
      if (!this.lastRefreshedDate) return "";

      return dates.methods.toLocalShort(this.lastRefreshedDate);
    },
  },

  methods: {
    ...mapActions({
      getCachedSections: "cache/getCachedSections",
      refreshSections: "cacheRefresh/refreshSections",
      getSectionsLastRefreshedDate: "cacheRefresh/getSectionsLastRefreshedDate",
    }),

    getLastRefreshedDateRequest() {
      this.isLoadingLastRefreshedDate = true;
      this.cachedData = [];

      this.getSectionsLastRefreshedDate()
        .then((response) => (this.lastRefreshedDate = response.data))
        .catch(() => (this.lastRefreshedDate = false));
    },

    getSectionsRequest() {
      this.isLoading = true;
      this.getCachedSections()
        .then((result) => {
          this.cachedData = result.data.slice(0, 100);
          console.log("Refresh Data: ", result);
        })
        .finally(() => (this.isLoading = false));
    },

    refresh() {
      this.$refs.yesNoDialog
        .open({
          title: "Refresh cache",
          message:
            "Are you sure you want to refresh the cache? This might take a few minutes.",
        })
        .then(() => {
          this.$refs.loadingDialog.open({ title: "Refreshing cache" });

          this.refreshSections()
            .then((result) => {
              this.$refs.loadingDialog.close();
              this.$snotify.success("Cache has been refreshed!", "", {
                timeout: 2000,
              });
              console.log("Refreshed cache: ", result);
              this.getLastRefreshedDateRequest();
              this.getSectionsRequest();
            })
            .catch((error) => {
              this.$refs.loadingDialog.close();
              this.$snotify.error(`Could not refresh cache: ${error}`, "", {
                timeout: 2000,
              });
              console.error("Could not refresh:", error);
            });
        });
    },
  },

  components: {
    AppToolbar,
    TableSkeletonLoader,
    YesNoDialog,
    LoadingDialog,
    SectionsListVuew,
  },

  mixins: [dates],
};
</script>