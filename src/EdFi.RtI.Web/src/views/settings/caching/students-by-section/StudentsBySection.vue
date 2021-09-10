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

      <ListVuew :items="cachedData" :isLoading="isLoading"> </ListVuew>

      <TableSkeletonLoader v-if="isLoading" :column-count="0" :row-count="6" />

      <YesNoDialog ref="yesNoDialog" />
      <LoadingDialog ref="loadingDialog" />
    </v-container>
  </div>
</template>

<script>
import AppToolbar from "@/components/widgets/AppToolbar.vue";
import TableSkeletonLoader from "@/components/widgets/TableSkeletonLoader";
import ListVuew from "./StudentsBySectionListView";
import { mapActions } from "vuex";
import YesNoDialog from "@/components/dialogs/YesNoDialog.vue";
import LoadingDialog from "@/components/dialogs/LoadingDialog.vue";

export default {
  data() {
    return {
      isLoading: false,
      toolbarTitle: "Students by Section",
      toolbarIcon: "mdi-account-details",
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
    this.isLoading = true;
    this.getCachedStudentsBySection()
      .then((result) => this.cachedData = result.data)
      .finally(() => (this.isLoading = false));
  },

  computed: {},

  methods: {
    ...mapActions({
      getCachedStudentsBySection: "cache/getCachedStudentsBySection",
      refreshStudentsBySection: "cacheRefresh/refreshStudentsBySection",
    }),

    refresh() {
      this.$refs.yesNoDialog
        .open({
          title: "Refresh cache",
          message:
            "Are you sure you want to refresh the cache? This might take a few minutes.",
        })
        .then(() => {
          this.$refs.loadingDialog.open({ title: "Refreshing cache" });

          this.refreshStudentsBySection()
            .then((result) => {
              this.$refs.loadingDialog.close();
              this.$snotify.success("Cache has been refreshed!", "", {
                timeout: 2000,
              });
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
    TableSkeletonLoader,
    YesNoDialog,
    LoadingDialog,
    ListVuew,
  },
};
</script>