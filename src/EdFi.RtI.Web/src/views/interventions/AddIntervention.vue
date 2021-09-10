<template>
  <div>
    <v-app-bar>
      <v-btn
        icon
        :to="'/interventions'">
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-toolbar-title>Add Intervention</v-toolbar-title>

      <v-spacer></v-spacer>
    </v-app-bar>
    <v-container>
      <v-alert
        dense
        text
        type="info">
        Assessments and interventions are available globally, across all campuses.
      </v-alert>
      <v-alert
        dense
        text
        type="warning">
        Fields marked with a (*) are required.
      </v-alert>
      <v-card style="background: rgb(255, 255, 255);">
        <v-container>
          <v-form v-model="isFormValid">
            <v-row>
              <v-col
                cols="12"
                sm="12">
                <v-text-field
                  v-model="intervention.identificationCode"
                  label="Name*"
                  :rules="rules.assessments.title" />
              </v-col>
              <v-col
                cols="12"
                sm="6">
                <v-select
                  v-model="intervention.classType"
                  label="Class Type*"
                  :items="interventionClasses"
                  item-text="description"
                  item-value="codeValue"
                  :rules="rules.assessments.title" />
              </v-col>
              <v-col
                cols="12"
                sm="6">
                <v-select
                  v-model="intervention.deliveryMethodType"
                  label="Delivery Method Type*"
                  :items="deliveryMethods"
                  item-text="description"
                  item-value="codeValue"
                  :rules="rules.assessments.title" />
              </v-col>
              <v-col
                cols="12"
                sm="6">
                <DatePicker
                  v-model="intervention.beginDate"
                  :label-text.sync="beginLabel" />
              </v-col>
              <v-col
                cols="12"
                sm="6">
                <DatePicker
                  v-model="intervention.endDate"
                  :label-text.sync="endLabel" />
              </v-col>
            </v-row>
          </v-form>
        </v-container>
      </v-card>
      <div class="d-flex justify-end mt-6">
        <v-btn
          color="primary"
          class="text-capitalize ml-2"
          :loading="isAddingIntervention"
          :disabled="!isFormValid"
          @click="addIntervention">
          Add
        </v-btn>
        <v-btn
          color="error"
          class="text-capitalize ml-2"
          :to="'/interventions'">
          Cancel
        </v-btn>
      </div>
    </v-container>

    <YesNoDialog ref="yesNoDialog" />
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex"
import { Intervention } from "../../models/models"
import DatePicker from "../../components/widgets/DatePicker"
import YesNoDialog from "../../components/dialogs/YesNoDialog"
import { forms } from "../../mixins/mixins"
import api from '@/api'

export default {

  data: () => ({
    isAddingIntervention: false,
    intervention: new Intervention(),

    beginDate: new Date().toISOString().substr(0, 10),
    endDate: new Date().toISOString().substr(0, 10),
    beginLabel: "Begin Date*",
    endLabel: "End Date",

    justMadePost: false,
    isFormValid: false,

    deliveryMethods: [],
    interventionClasses: [],
  }), // data

  beforeRouteLeave(to, from, next) {
    if (this.justMadePost)
    {next()}
    else {
      this.$refs.yesNoDialog.open({
        title: "Back to interventions list",
        message: "Are you sure you wish to exit without saving?"
      })
        .then(() => {
          console.log("Yes clicked!")
          next()
        })
        .catch((error) => {
          console.log("No clicked!", error)
        })
    }
  }, // beforeRouteLeave

  mounted() {
    this.init()
  }, // mounted

  computed: {
    ...mapGetters({
      getOrganizationId: "session/getOrganizationId",
    })
  }, // computed

  methods: {
    ...mapActions({
      createIntervention: "interventions/create"
    }),

    init() {
      api.descriptors.getDeliveryMethods().then(response => this.deliveryMethods = response)
      api.descriptors.getInterventionClasses().then(response => this.interventionClasses = response)
    },

    addIntervention() {
      this.justMadePost = true
      const notification = this.$snotify.info("Adding intervention...", "", {
        timeout: 0,
        closeOnClick: false
      })
      this.isAddingIntervention = true

      this.intervention.educationOrganizationReference.educationOrganizationId = this.getOrganizationId;

      this.createIntervention(this.intervention)
        .then(() => {
          this.$snotify.success("Intervention added!", "", {
            timeout: 1500
          })
          this.$router.push("/interventions")
        })
        .catch((error) => this.$snotify.error(`Could not add intervention. ${error}`, "", {
          timeout: 4000
        }))
        .finally(() => {
          this.$snotify.remove(notification.id)
          this.isAddingIntervention = false
        })
    },

    showDate (){
      console.log("Begin Date: ", this.beginDate)
      console.log("End Date: ", this.endDate)
    }
  }, //methods

  components: {
    DatePicker,
    YesNoDialog
  }, // components

  mixins: [
    forms
  ], // mixins
}
</script>
