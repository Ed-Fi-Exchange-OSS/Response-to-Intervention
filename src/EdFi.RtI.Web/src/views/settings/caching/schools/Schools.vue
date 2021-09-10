<template>
  <div>
    <v-container>
      <AppToolbar
        v-model="searchInput"
        :title="toolbarTitle"
        :icon="toolbarIcon"
        :buttons="buttons"
        :disabledButtons="isLoadingSchools"
        :showSearch="false"
        class="mb-5"
      >
      </AppToolbar>

      <p class="mt-4">Last refreshed: {{ lastRefreshedDateLocal }}</p>

      <SchoolsListView :items="schools" :isLoading="isLoadingSchools">
      </SchoolsListView>

      <TableSkeletonLoader
        v-if="isLoadingSchools"
        :column-count="0"
        :row-count="6"
      />

      <YesNoDialog ref="yesNoDialog" />
      <LoadingDialog ref="loadingDialog" />
    </v-container>
  </div>
</template>

<script>
import AppToolbar from "@/components/widgets/AppToolbar.vue";
import TableSkeletonLoader from "@/components/widgets/TableSkeletonLoader";
import SchoolsListView from "./SchoolsListView.vue";
import { mapActions } from "vuex";
import YesNoDialog from "@/components/dialogs/YesNoDialog.vue";
import LoadingDialog from "@/components/dialogs/LoadingDialog.vue";
import { dates } from "@/mixins/mixins";

export default {
  data() {
    return {
      lastRefreshedDate: null,
      isLoadingLastRefreshedDate: false,
      isLoadingSchools: false,
      isSearching: false,
      searchInput: "",
      toolbarTitle: "Schools",
      schools: [],
      toolbarIcon: "mdi-book-open-variant",
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
    this.getSchoolsRequest();
  },

  computed: {
    lastRefreshedDateLocal() {
      if (!this.lastRefreshedDate) return "";

      return dates.methods.toLocalShort(this.lastRefreshedDate);
    },
  },

  methods: {
    ...mapActions({
      getCachedSchools: "cache/getCachedSchools",
      refreshSchools: "cacheRefresh/refreshSchools",
      getSchoolsLastRefreshedDate: "cacheRefresh/getSchoolsLastRefreshedDate",
    }),

    getLastRefreshedDateRequest() {
      this.isLoadingLastRefreshedDate = true;
      this.schools = [];

      this.getSchoolsLastRefreshedDate()
        .then((response) => (this.lastRefreshedDate = response.data))
        .catch(() => (this.lastRefreshedDate = false));
    },

    getSchoolsRequest() {
      this.isLoadingSchools = true;
      this.getCachedSchools()
        .then((result) => {
          this.schools = result.data;
          console.log("Refresh Data: ", result);
        })
        .finally(() => (this.isLoadingSchools = false));
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

          this.refreshSchools()
            .then((result) => {
              this.$refs.loadingDialog.close();
              this.$snotify.success("Cache has been refreshed!", "", {
                timeout: 2000,
              });
              this.getLastRefreshedDateRequest();
              this.getSchoolsRequest();
              console.log("Refreshed cache: ", result);
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
    SchoolsListView,
    TableSkeletonLoader,
    YesNoDialog,
    LoadingDialog,
  },

  mixins: [dates],
};
</script>