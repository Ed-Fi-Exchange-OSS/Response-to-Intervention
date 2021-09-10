<template>
  <v-dialog
    v-model="isDialogOpened"
    max-width="580"
    persistent>
    <v-card>
      <v-toolbar
        dense
        elevation="0.50">
        <v-toolbar-title>
          <h5 class="font-weight-regular">
            Delete Assessment(s)
          </h5>
        </v-toolbar-title>
      </v-toolbar>

      <v-card-text>
        <v-select
          v-model="selectedAssessmentId"
          class="mb-5 mt-3"
          label="Select an assessment"
          hide-details
          item-value="associationModel.identifier"
          :items="associations"
          :disabled="isLoadingAssessments"
          :loading="isLoadingAssessments">
          <template v-slot:selection="{ item }">
            {{ item.assessment.title }} | {{ getDateShort(item.associationModel.administrationDate) }}
          </template>
          <template v-slot:item="{ item }">
            {{ item.assessment.title }} | {{ getDateShort(item.associationModel.administrationDate) }}
          </template>
        </v-select>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn
          color="primary"
          class="text-capitalize ml-2"
          :disabled="isLoadingAssessments"
          @click="onDelete">
          Delete
        </v-btn>
        <v-btn
          color="error"
          class="text-capitalize ml-2"
          @click.native="onCancel">
          Cancel
        </v-btn>
      </v-card-actions>
    </v-card>
    <YesNoDialog ref="yesNoDialog" />
  </v-dialog>
</template>

<script>
import { mapActions } from "vuex"
import { dates } from "../../mixins/mixins"
import YesNoDialog from "../../components/dialogs/YesNoDialog"

export default {

  name: "AssessmentListDialog",

  data: () => ({
    isDialogOpened: false,
    resolve: null,
    reject: null,

    associations: [],
    selectedAssessmentId: "",
    isLoadingAssessments: false
  }), // data

  mounted () {

  }, // mounted
  methods: {
    ...mapActions({
    }),

    excludeAssessments(assessments) {
      this.associations = assessments
    },

    isSelectedAssessmentValid () {
      return this.selectedAssessmentId && this.selectedAssessmentId > 0
    },

    open ({ excludeAssessments }) {
      this.isDialogOpened = true

      this.excludeAssessments(excludeAssessments)

      this.selectDefaultAssessment()

      return new Promise((resolve, reject) => {
        this.resolve = resolve
        this.reject = reject
      })
    },

    onDelete () {
      this.$refs.yesNoDialog.open({
        title: "Before delete",
        message: "Are you SURE you want to delete this assessment? All student scores for this assessment will be lost. This cannot be undone."
      })
        .then(() => {
          const association = this.associations.find((assess) => assess.associationModel.identifier == this.selectedAssessmentId)
          this.resolve({
            association: association
          })
          this.isDialogOpened = false
        })
        .catch((error) => {
          // TODO Handle error
        })
    },

    onCancel () {
      this.reject()
      this.isDialogOpened = false
    },

    selectDefaultAssessment () {
      if (this.associations.length > 0)
      {this.selectedAssessmentId = this.associations[0].id}

    },

    getDateShort (dateStr) {
      return dates.methods.toLocalShort(dateStr)
    }
  }, // methods

  components: {
    YesNoDialog
  }, // components

  mixins: [
    dates
  ]
}
</script>
