<template>
  <v-dialog
    v-model="isDialogOpened"
    max-width="400"
    persistent>
    <v-card>
      <v-toolbar
        dense
        elevation="0.50">
        <v-toolbar-title>
          <h5 class="font-weight-regular">
            Add Intervention
          </h5>
        </v-toolbar-title>
      </v-toolbar>
      <v-card-text>
        <v-select
          v-model="selectedInterventionId"
          class="mb-5 mt-3"
          label="Select an intervention"
          hide-details
          item-text="identificationCode"
          item-value="id"
          :items="interventions"
          :disabled="isLoadingInterventions"
          :loading="isLoadingInterventions" />

        <DatePicker
          v-model="beginDate"
          :label-text.sync="beginLabel" />

        <DatePicker
          v-model="endDate"
          :label-text.sync="endLabel" />
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn
          color="primary"
          class="text-capitalize ml-2"
          :disabled="isLoadingInterventions"
          @click.native="onAdd">
          Add
        </v-btn>
        <v-btn
          color="error"
          class="text-capitalize ml-2"
          @click.native="onCancel">
          Cancel
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import { mapActions } from "vuex"
import DatePicker from "../widgets/DatePicker"

export default {

  name: "AddScoringInterventionDialog",

  data: () => ({
    isDialogOpened: false,
    resolve: null,
    reject: null,

    interventions: [],
    selectedInterventionId: 0,
    isLoadingInterventions: false,

    beginDate: new Date().toISOString().substr(0, 10),
    endDate: new Date().toISOString().substr(0, 10),
    beginLabel: "Begin Date*",
    endLabel: "End Date"
  }), // data

  mounted () {
    this.getInterventionsRequest()
  }, // mounted

  methods: {
    ...mapActions({
      getInterventions: "interventions/searchWithParams"
    }),

    excludeInterventions (interventions) {
      const interventionIds = new Set(interventions)
      this.interventions = this.interventions.filter((inter) => !interventionIds.has(inter.id))
    },

    getInterventionsRequest () {
      this.isLoadingInterventions = true
      this.selectedInterventionId = 0

      this.getInterventions({ getFromCache: true })
      .then((result) => {
        this.interventions = result.data;
        this.selectDefaultIntervention();
      })
      .catch((error) => console.error("Could not get interventions.", error))
      .finally(() => this.isLoadingInterventions = false)
    },

    isSelectedInterventionValid () {
      return this.selectedInterventionId && this.selectedInterventionId > 0
    },

    open ({ excludeInterventions }) {
      this.isDialogOpened = true

      if (excludeInterventions)
      {this.excludeInterventions(excludeInterventions)}

      this.selectDefaultIntervention()

      return new Promise((resolve, reject) => {
        this.resolve = resolve
        this.reject = reject
      })
    },

    onAdd () {
      this.isDialogOpened = false
      this.resolve({
        intervention: this.interventions.find((inter) => inter.id == this.selectedInterventionId),
        beginDate: new Date(),
        endDate: new Date()
      })
    },

    onCancel () {
      this.isDialogOpened = false
      this.reject()
    },

    selectDefaultIntervention () {
      if (this.interventions.length > 0)
      {this.selectedInterventionId = this.interventions[0].id}
    },
  }, // methods

  components: {
    DatePicker
  } // components
}
</script>
