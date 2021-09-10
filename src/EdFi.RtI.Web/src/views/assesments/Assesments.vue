<template>
  <div>
    <v-container>
      <AppToolbar
        v-model="searchInput"
        @input="onSearchValueChanged"
        :title="toolbarTitle"
        :icon="toolbarIcon"
        :buttons="buttons"
        :showSearch="true"
        class="mb-5">
      </AppToolbar>

      <div class="text-center mb-5">
        <v-chip
          :color="currentNamespace == true
            ? 'accent'
            : null"
          class="ma-2"
          v-on:click ="currentNamespace = true"
          @click="onGetByNamespace"
        >
          Local
        </v-chip>

        <v-chip
          :color="currentNamespace == false
            ? 'accent'
            : null"
          class="ma-2"
          v-on:click ="currentNamespace = false"
          @click="onGetByNamespace"
        >
          External
        </v-chip>
      </div>

      <TableSkeletonLoader
        v-if="isLoadingAssesments"
        :column-count="0"
        :row-count="6" />

      <AssesmentsListView
        :class="{ 'hidden': isLoadingAssesments == true }"
        :items="assesments"
        :is-loading="isLoadingAssesments"
        :total-count="assessmentsTotalCount"
        :search-params="searchParams"
        @search-params-changed="onSearchParamsChanged"
        :readonly="!currentNamespace">
      </AssesmentsListView>

    </v-container>

    <YesNoDialog ref="yesNoDialog"/>
    <LoadingDialog ref="loadingDialog"/>

  </div>
</template>

<style scoped>
.hidden {
  visibility: hidden;
}
</style>

<script>
import AssesmentsListView from "@/components/list-views/AssesmentsListView.vue"
import TableSkeletonLoader from "../../components/widgets/TableSkeletonLoader.vue"
import AppToolbar from '../../components/widgets/AppToolbar.vue';
import { mapActions, mapGetters, mapMutations } from "vuex";
import YesNoDialog from "@/components/dialogs/YesNoDialog.vue";
import LoadingDialog from '@/components/dialogs/LoadingDialog.vue';

export default {
  data() {
      return{
        currentNamespace: true,
        isLoadingAssesments: false,
        isSearching: false,
        assesments: [],
        assessmentsTotalCount: 0,
        searchInput: "",
        toolbarTitle: 'Assessments',
        toolbarIcon: 'mdi-view-dashboard',
        buttons: [
            {
                icon: 'mdi-rotate-right',
                text: 'Refresh',
                click: () => this.searchAssessments(),
            },
            {
                icon: 'mdi-plus',
                text: 'Add',
                click: () => this.$router.push('/assesments/add'),
                disabled: () => !this.currentNamespace,
            },
            {
                icon: 'mdi-database-refresh',
                text: 'Refresh Cache',
                click: () => this.refresh(),
            },
        ],
    }
  }, // data

  computed: {
    ...mapGetters({
      searchParams: "assesments/getSearchParams"
    }),
  }, // computed

  mounted () {

  }, // mounted

  methods: {
    ...mapMutations({
      setSearchParams: "assesments/setSearchParams"
    }),

    ...mapActions({
      getAssesments: "assesments/search",
      getAssesmentsTotalCount: "assesments/getTotalCount",
      refreshAssessments: 'cacheRefresh/refreshAssessments'
    }),

    onSearch () {
      console.log("onSearch()", this.searchInput)
    },

    onSearchParamsChanged (searchParams) {
      this.setSearchParams(searchParams)
      this.searchAssessments()
    },

    onSearchValueChanged (searchValue) {
      this.searchParams.searchValue = searchValue;
      this.onSearchParamsChanged(this.searchParams);
    },

    onGetByNamespace(){
      this.searchParams.pageIndex = 1;
      this.searchParams.pageSize = 10;
      this.searchParams.currentNamespace = this.currentNamespace;
      console.log("Name space: ", this.searchParams);
      this.onSearchParamsChanged(this.searchParams);
    },

    searchAssessments() {
      this.assesments = [];
      this.isLoadingAssesments = true;

      this.getAssesments(this.searchParams)
      .then((result) => {
        this.assesments = result.data;

        if (this.assessmentsTotalCount == 0) {
          this.assessmentsTotalCount = this.assesments.length;
        }

        this.setAssessmentsTotalCountRequest();
      })
      .finally(() => this.isLoadingAssesments = false);
    },

    setAssessmentsTotalCountRequest() {
      this.getAssesmentsTotalCount(this.searchParams)
      .then(result => this.assessmentsTotalCount = result.data)
      .catch(() => this.assessmentsTotalCount = this.assesments.length);
    },

    refresh(){
      this.$refs.yesNoDialog.open({
        title: "Refresh cache",
        message: "Are you sure you want to refresh the cache? This might take a few minutes."
      })
      .then(() =>{
        this.$refs.loadingDialog.open({ title: 'Refreshing cache' })

        this.refreshAssessments().
        then(result => {
          this.$refs.loadingDialog.close();
          this.$snotify.success('Cache has been refreshed!', '', {timeout: 2000});
          console.log("Refreshed cache: ", result);
          this.onSearchParamsChanged(this.searchParams);
        })
        .catch(error => {
          this.$refs.loadingDialog.close();
          this.$snotify.error(`Could not refresh cache: ${error}`, '', { timeout: 2000 });
          console.error('Could not refresh:', error);
        });
      });
    },
  }, // methods

  components: {
    AssesmentsListView,
    TableSkeletonLoader,
    AppToolbar,
    YesNoDialog,
    LoadingDialog,
  } // components
}
</script>
