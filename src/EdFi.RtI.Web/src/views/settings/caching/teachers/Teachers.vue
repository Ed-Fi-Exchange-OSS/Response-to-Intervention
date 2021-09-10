<template>
  <div>
    <v-container>
      <AppToolbar
        v-model="searchInput"
        :title="toolbarTitle"
        :icon="toolbarIcon"
        :buttons="buttons"
        :disabledButtons="isLoadingStaffs"
        :showSearch="false"
        class="mb-5"
      >
      </AppToolbar>

      <p class="mt-4">Last refreshed: {{ lastRefreshedDateLocal }}</p>

      <TeachersListView :items="staffs" :isLoading="isLoadingStaffs">
      </TeachersListView>

      <TableSkeletonLoader
        v-if="isLoadingStaffs"
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
import TeachersListView from "./TeachersListView.vue";
import { mapActions } from "vuex";
import YesNoDialog from "@/components/dialogs/YesNoDialog.vue";
import LoadingDialog from "@/components/dialogs/LoadingDialog.vue";
import { dates } from "@/mixins/mixins";

export default {
  data() {
    return {
      lastRefreshedDate: null,
      isLoadingLastRefreshedDate: false,
      isLoadingStaffs: false,
      isSearching: false,
      searchInput: "",
      toolbarTitle: "Staffs",
      toolbarIcon: "mdi-account-tie",
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

      staffs: [],
    };
  }, // data

  mounted() {
    this.getLastRefreshedDateRequest();
    this.getStaffsRequest();
  },

  computed: {
    lastRefreshedDateLocal() {
      if (!this.lastRefreshedDate) return "";

      return dates.methods.toLocalShort(this.lastRefreshedDate);
    },
  },

  methods: {
    ...mapActions({
      getCachedStaffs: "cache/getCachedStaffs",
      refreshStaffs: "cacheRefresh/refreshStaffs",
      getStaffsLastRefreshedDate: "cacheRefresh/getStaffsLastRefreshedDate",
    }),

    getLastRefreshedDateRequest() {
      this.isLoadingLastRefreshedDate = true;
      this.staffs = [];

      this.getStaffsLastRefreshedDate()
        .then((response) => (this.lastRefreshedDate = response.data))
        .catch(() => (this.lastRefreshedDate = false));
    },

    getStaffsRequest() {
      this.isLoadingStaffs = true;
      this.getCachedStaffs()
        .then((result) => {
          this.staffs = result.data.slice(0, 100);
          console.log("Refresh Data: ", result);
        })
        .finally(() => (this.isLoadingStaffs = false));
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

          this.refreshStaffs()
            .then((result) => {
              this.$refs.loadingDialog.close();
              this.$snotify.success("Cache has been refreshed!", "", {
                timeout: 2000,
              });
              console.log("Refreshed cache: ", result);
              this.getLastRefreshedDateRequest();
              this.getStaffsRequest();
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
    TeachersListView,
    TableSkeletonLoader,
    YesNoDialog,
    LoadingDialog,
  },

  mixins: [dates],
};
</script>