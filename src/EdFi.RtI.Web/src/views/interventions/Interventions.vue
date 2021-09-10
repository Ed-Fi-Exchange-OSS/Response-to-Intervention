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

      <TableSkeletonLoader
        v-if="isLoadingInterventions"
        :column-count="0"
        :row-count="6" />

      <InterventionsListView
        :class="{ 'hidden': isLoadingInterventions == true }"
        :items="interventions"
        :interventions-total-count="interventionsTotalCount"
        :is-loading="isLoadingInterventions"
        :search-params="searchParams"
        @search-params-changed="onSearchParamsChanged">
      </InterventionsListView>

      <YesNoDialog ref="yesNoDialog"/>
      <LoadingDialog ref="loadingDialog"/>
    </v-container>
  </div>
</template>

<style scoped>
.hidden {
  visibility: hidden;
}
</style>

<script>
import InterventionsListView from "@/components/list-views/InterventionsListView"
import TableSkeletonLoader from "../../components/widgets/TableSkeletonLoader"
import AppToolbar from '../../components/widgets/AppToolbar.vue';
import { mapActions, mapGetters, mapMutations } from "vuex";
import YesNoDialog from "@/components/dialogs/YesNoDialog.vue";
import LoadingDialog from '@/components/dialogs/LoadingDialog.vue';

export default {
  data() {
    return {
      isLoadingInterventions: false,
      isSearching: false,
      interventions: [],
      interventionsTotalCount: 0,
      searchInput: "",
      toolbarTitle: 'Interventions',
      toolbarIcon: 'mdi-text-box-check',
      buttons: [
          {
              icon: 'mdi-rotate-right',
              text: 'Refresh',
              click: () => {
                  this.searchInterventions();
              },
          },
          {
              icon: 'mdi-plus',
              text: 'Add',
              click: () => {
                  this.$router.push('/interventions/add');
              },
          },
          {
              icon: 'mdi-database-refresh',
              text: 'Refresh Cache',
              click: () => {
                  this.refresh();
              },
          },
      ],
    }
  },

  computed: {
    ...mapGetters({
      searchParams: "interventions/getSearchParams"
    }),
  },

  mounted () {

  },

  methods: {
    ...mapMutations({
      setSearchParams: "interventions/setSearchParams"
    }),

    ...mapActions({
      getInterventions: "interventions/search",
      getInterventionsTotalCount: "interventions/getTotalCount",
      refreshInterventions: 'cacheRefresh/refreshInterventions',
    }),

    onSearchParamsChanged (searchParams) {
      this.setSearchParams(searchParams)
      this.searchInterventions()
    },

    onSearchValueChanged (searchValue) {
      this.searchParams.searchValue = searchValue;
      this.onSearchParamsChanged(this.searchParams);
    },

    searchInterventions() {
      this.interventions = []
      this.isLoadingInterventions = true

      this.getInterventions(this.searchParams)
      .then(result => {
        this.interventions = result.data

        if (this.interventionsTotalCount == 0) {
          this.interventionsTotalCount = this.interventions.length;
        }
        
        this.setInterventionsTotalCountRequest();
      })
      .finally(() => this.isLoadingInterventions = false)
    },

    setInterventionsTotalCountRequest() {
      this.getInterventionsTotalCount()
      .then(response => this.interventionsTotalCount = response.data)
      .catch(() => this.interventionsTotalCount = this.interventions.length);
    },

    onSearch (){
      console.log("Searching", this.searchInput)
    },
    
    refresh(){
      this.$refs.yesNoDialog.open({
        title: "Refresh cache",
        message: "Are you sure you want to refresh the cache? This might take a few minutes."
      })
      .then(() =>{
        this.$refs.loadingDialog.open({ title: 'Refreshing cache' })

        this.refreshInterventions().
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
  },

  components: {
    InterventionsListView,
    TableSkeletonLoader,
    AppToolbar,
    YesNoDialog,
    LoadingDialog,
  }
}
</script>
