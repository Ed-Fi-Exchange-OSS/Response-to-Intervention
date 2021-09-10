<template>
  <div>
    <v-data-table
      class="elevation-1"
      :headers="headers"
      :items="items"
      :options.sync="listViewOptions"
      :server-items-length="totalCount"
      :hide-default-footer="isLoading"
      :footer-props="{ 'items-per-page-options': [ 10, 20, 30, 40, 50 ] }">
      <template v-slot:no-data>
        <div></div>
      </template>

      <template v-slot:item.revisionDate="{ item }">
        {{ getLastModifiedDateStr(item) }}
      </template>

      <template v-slot:item.actions="{ item }">
        <v-btn
          small
          icon
          @click="viewAssessment(item)">
          <v-icon>mdi-eye</v-icon>
        </v-btn>

        <v-btn
          small
          icon
          @click="deleteAssessment(item)"
          v-if="!readonly || readonly == false">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>

    <!-- dialogs -->
    <div>
      <AssessmentDetailsDialog 
        ref="assessmentDetailsDialog"
        :showEditButton="!readonly">
      </AssessmentDetailsDialog>

      <YesNoDialog ref="yesNoDialog" />
    </div><!-- dialogs -->
  </div>
</template>

<style>
    .unable{
        background: lightgray;
    }
</style>

<script>
import AssessmentDetailsDialog from "../../components/dialogs/AssessmentDetailsDialog"
import { dates } from "../../mixins/mixins"
import YesNoDialog from "../../components/dialogs/YesNoDialog"
import { mapActions } from "vuex"

export default {
  
  props: [
    "items",
    "isLoading",
    "searchParams",
    "readonly",
    "totalCount"
  ],

  data: () => ({
    headers: [
      {
        text: "Title",
        value: "title",
        sortable: true
      },
      {
        text: "Category",
        value: "categoryDescriptor",
        sortable: true
      },
      {
        text: "Max Raw Score",
        value: "maxRawScore",
        sortable: true
      },
      {
        text: "Revision Date",
        value: "revisionDate",
        sortable: true
      },
      {
        text: "Namespace",
        value: "namespaceProperty",
      },
      {
        sortable: false,
        value: "actions"
      }
    ],
    listViewOptions: {
      page: 1,
      itemsPerPage: 10
    }
  }), // data

  methods: {
    ...mapActions({
      deleteAssessmentByID: "assesments/delete"
    }),

    getLastModifiedDateStr (assesment) {
      return dates.methods.toLocalShort(assesment.revisionDate)
    },

    viewAssessment (assessment) {
      this.$refs.assessmentDetailsDialog.open({
        assessment
      })
        .then(() => {
          this.$emit("search-params-changed", this.searchParams)
        })
    },

    deleteAssessment (assessment) {
      this.$refs.yesNoDialog.open({
        title: "Confirm deletion?",
        message: "Are you sure you wish to delete this assessment?"
      })
        .then(() => {
          let deleteWorks = true
          const notification = this.$snotify.info("Deleting assessment...", "", {
            timeout: 0,
            closeOnClick: false
          })
          this.deleteAssessmentByID(assessment.id)
            .then((result) => {
              result.data.forEach((element) => {
                if (element.id == assessment.id)
                {deleteWorks = false}
              })
              if (deleteWorks){
                this.$emit("search-params-changed", this.searchParams)
                this.$snotify.success("Assessment deleted!", "", {
                  timeout: 1500
                })
              } else
              {this.$snotify.info("There are student scores associated with this assessment. Deleting it would result in the loss of student data.", "Could not delete", {
                timeout: 5000
              })}
            })
            .catch((error) => this.$snotify.error(`Could not delete assessment. ${error}`, "", {
              timeout: 4000
            }))
            .finally(() => {
              this.$snotify.remove(notification.id)
            })
        })
    },

    isAssessmentFromThisEnvironment (assessment){
      return process.env.VUE_APP_EDFI_NAMESPACE == assessment.namespaceProperty
    }
  }, //

  watch: {
    listViewOptions: {
      handler() {
        this.searchParams.pageIndex = this.listViewOptions.page;
        this.searchParams.pageSize = this.listViewOptions.itemsPerPage;
        this.searchParams.sortField = this.listViewOptions.sortBy[0];
        this.searchParams.sortDesc = this.listViewOptions.sortDesc[0];

        if (this.listViewOptions.sortBy[0] == undefined || this.listViewOptions.sortDesc[0] == undefined){
          this.searchParams.sortField = "revisionDate";
          this.searchParams.sortDesc = false;
        }

        this.$emit("search-params-changed", this.searchParams);
      }
    }
  },

  components: {
    AssessmentDetailsDialog,
    YesNoDialog
  }, // watch

  mixins: [
    dates
  ] // components
}
</script>
