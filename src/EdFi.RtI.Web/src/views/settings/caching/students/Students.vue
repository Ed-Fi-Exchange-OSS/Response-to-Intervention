<template>
  <div>
    <v-container>
      <AppToolbar
        :title="toolbarTitle"
        :icon="toolbarIcon"
        :buttons="buttons"
        :disabledButtons="isLoadingStudents"
        :showSearch="false"
        class="mb-5"
      >
      </AppToolbar>

      <p class="mt-4">Last refreshed: {{ lastRefreshedDateLocal }}</p>

      <StudentsListView :items="students" :isLoading="isLoadingStudents">
      </StudentsListView>

      <TableSkeletonLoader
        v-if="isLoadingStudents"
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
import StudentsListView from "./StudentsListView.vue";
import { mapActions } from "vuex";
import YesNoDialog from "@/components/dialogs/YesNoDialog.vue";
import LoadingDialog from "@/components/dialogs/LoadingDialog.vue";
import { dates } from "@/mixins/mixins";

export default {
  data() {
    return {
      lastRefreshedDate: null,
      isLoadingLastRefreshedDate: false,
      isLoadingStudents: false,
      isSearching: false,
      toolbarTitle: "Students",
      toolbarIcon: "mdi-account",
      students: [],
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
    this.getStudentsRequest();
  },

  computed: {
    lastRefreshedDateLocal() {
      if (!this.lastRefreshedDate) return "";

      return dates.methods.toLocalShort(this.lastRefreshedDate);
    },
  },

  methods: {
    ...mapActions({
      getCachedStudents: "cache/getCachedStudents",
      refreshStudents: "cacheRefresh/refreshStudents",
      getStudentsLastRefreshedDate: "cacheRefresh/getStudentsLastRefreshedDate",
    }),

    getLastRefreshedDateRequest() {
      this.isLoadingLastRefreshedDate = true;
      this.students = [];

      this.getStudentsLastRefreshedDate()
        .then((response) => (this.lastRefreshedDate = response.data))
        .catch(() => (this.lastRefreshedDate = false));
    },

    getStudentsRequest() {
      this.isLoadingStudents = true;
      this.getCachedStudents()
        .then((result) => {
          this.students = result.data.slice(0, 100); // TODO Show all?
          console.log("Refresh Data: ", result);
        })
        .finally(() => (this.isLoadingStudents = false));
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

          this.refreshStudents()
            .then((result) => {
              this.$refs.loadingDialog.close();
              this.$snotify.success("Cache has been refreshed!", "", {
                timeout: 2000,
              });
              console.log("Refreshed cache: ", result);
              this.getLastRefreshedDateRequest();
              this.getStudentsRequest();
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
    StudentsListView,
    TableSkeletonLoader,
    YesNoDialog,
    LoadingDialog,
  },

  mixins: [dates],
};
</script>