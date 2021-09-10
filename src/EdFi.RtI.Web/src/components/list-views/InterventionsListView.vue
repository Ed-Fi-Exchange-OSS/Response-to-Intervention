<template>
  <div>
    <v-data-table
      :headers="headers"
      :items="items"
      :options.sync="listViewOptions"
      :server-items-length="interventionsTotalCount"
      :hide-default-footer="isLoading"
      :footer-props="{ 'items-per-page-options': [ 10, 20, 30, 40, 50 ] }">
      <template v-slot:no-data>
        <div></div>
      </template>

      <template v-slot:item.beginDate="{ item }">
        {{ getBeginDateStr(item) }}
      </template>

      <template v-slot:item.endDate="{ item }">
        {{ getEndDateStr(item) }}
      </template>

      <template v-slot:item.actions="{ item }">
        <v-btn
          small
          icon
          @click="viewIntervention(item)">
          <v-icon>mdi-eye</v-icon>
        </v-btn>

        <v-btn
          small
          icon
          @click="deleteIntervention(item)">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>

    <!-- dialogs -->
    <div>
      <InterventionDetailsDialog ref="interventionDetailsDialog" />
      <YesNoDialog ref="yesNoDialog" />
    </div><!-- dialogs -->
  </div>
</template>

<script>
import { dates } from "../../mixins/mixins"
import InterventionDetailsDialog from "../../components/dialogs/InterventionDetailsDialog"
import YesNoDialog from "../../components/dialogs/YesNoDialog"
import { mapActions } from "vuex"

export default {

  props: [
    "items",
    "isLoading",
    "searchParams",
    "interventionsTotalCount",
  ],

  data: () => ({
    headers: [
      {
        text: "Intervention Name",
        value: "identificationCode",
        sortable: true
      },
      {
        text: "Begin Date",
        value: "beginDate",
        sortable: true
      },
      {
        text: "End Date",
        value: "endDate",
        sortable: true
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
  }),

  methods: {
    ...mapActions({
      deleteInterventionByID: "interventions/delete"
    }),

    getEndDateStr (intervention) {
      return dates.methods.toLocalShort(intervention.endDate)
    },
    getBeginDateStr (intervention) {
      return dates.methods.toLocalShort(intervention.beginDate)
    },
    viewIntervention (intervention) {
      this.$refs.interventionDetailsDialog.open({
        intervention
      })
        .then((inter) => {
          this.$emit("search-params-changed", this.searchParams)
        })
    },

    deleteIntervention (intervention){
      this.$refs.yesNoDialog.open({
        title: "Confirm deletion?",
        message: "Are you sure you wish to delete this intervention?"
      })
        .then(() => {
          let deleteWorks = true
          const notification = this.$snotify.info("Deleting intervention...", "", {
            timeout: 0,
            closeOnClick: false
          })
          this.deleteInterventionByID(intervention.id)
            .then((result) => {
              result.data.forEach((element) => {
                if (element.id == intervention.id)
                {deleteWorks = false}
              })

              if (deleteWorks){
                this.$emit("search-params-changed", this.searchParams)
                this.$snotify.success("Intervention deleted!", "", {
                  timeout: 1500
                })
              } else
              {this.$snotify.info("There are student scores associated with this intervention. Deleting it would result in the loss of student data.", "Could not delete", {
                timeout: 5000
              })}
            })
            .catch((error) => this.$snotify.error(`Could not delete intervention. ${error}`, "", {
              timeout: 4000
            }))
            .finally(() => {
              this.$snotify.remove(notification.id)
            })
        })
    }
  },

  watch: {
    listViewOptions: {
      handler() {
        this.searchParams.pageIndex = this.listViewOptions.page;
        this.searchParams.pageSize = this.listViewOptions.itemsPerPage;
        this.searchParams.sortField = this.listViewOptions.sortBy[0];
        this.searchParams.sortDesc = this.listViewOptions.sortDesc[0];

        if (this.listViewOptions.sortBy[0] == undefined || this.listViewOptions.sortDesc[0] == undefined){
          this.searchParams.sortField = "beginDate"
          this.searchParams.sortDesc = false;
        }

        this.$emit("search-params-changed", this.searchParams)
      }
    }
  },

  components: {
    InterventionDetailsDialog,
    YesNoDialog
  }, // components

  mixins: [
    dates
  ]
}
</script>
