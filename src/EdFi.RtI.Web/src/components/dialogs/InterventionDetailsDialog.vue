<template>
  <v-dialog
    v-model="isDialogOpened"
    max-width="700"
    persistent>
    <v-card>
      <v-toolbar
        dense
        elevation="0.50">
        <v-toolbar-title>
          <h5 class="font-weight-regular">
            Intervention
          </h5>
        </v-toolbar-title>
      </v-toolbar>
      <v-card-text>
        <!-- loading intervention -->
        <v-row
          v-if="isGettingIntervention"
          class="mt-3">
          <v-col
            v-for="i of new Array(1)"
            :key="i"
            cols="12"
            sm="12"
            class="mb-6">
            <v-skeleton-loader type="text@2"></v-skeleton-loader>
          </v-col>
          <v-col
            v-for="i of new Array(4)"
            :key="i"
            cols="12"
            sm="6"
            class="mb-6">
            <v-skeleton-loader type="text@2"></v-skeleton-loader>
          </v-col>
        </v-row><!-- loading intervention -->

        <!-- intervention loaded -->
        <v-row v-if="!isGettingIntervention">
          <v-col
            cols="12"
            sm="12">
            <v-text-field
              v-model="intervention.identificationCode"
              label="Name"
              hide-details
              :disabled="true" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-select
              v-model="intervention.classType"
              label="Class Type"
              :disabled="readonly"
              :items="interventionClasses"
              item-text="description"
              item-value="codeValue" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-select
              v-model="intervention.deliveryMethodType"
              label="Delivery Method Type"
              :disabled="readonly"
              :items="deliveryMethods"
              item-text="description"
              item-value="codeValue" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <DatePicker
              v-model="intervention.beginDate"
              :label-text.sync="beginLabel"
              :is-disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <DatePicker
              v-model="intervention.endDate"
              :label-text.sync="endLabel"
              :is-disabled="readonly" />
          </v-col>
        </v-row><!-- intervention loaded -->
      </v-card-text>
      <v-card-actions>
        <v-spacer />

        <v-btn
          v-if="readonly"
          color="primary"
          class="text-capitalize ml-2"
          :disabled="isGettingIntervention"
          @click.native="edit">
          <v-icon left>
            mdi-pencil
          </v-icon>
          Edit
        </v-btn>

        <v-btn
          v-if="!readonly"
          color="primary"
          class="text-capitalize ml-2"
          :disabled="isGettingIntervention || isUpdatingIntervention"
          :loading="isUpdatingIntervention"
          @click="update">
          <v-icon left>
            mdi-content-save
          </v-icon>
          Save
        </v-btn>

        <v-btn
          color="error"
          class="text-capitalize ml-2"
          @click.native="close">
          <v-icon left>
            mdi-close
          </v-icon>
          Close
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import api from '@/api'
import { mapActions, mapGetters } from "vuex"
import { Intervention } from "../../models/models"
import DatePicker from "../widgets/DatePicker"

export default {

  name: "InterventionDetailsDialog",

  props: [
    "items",
    "isLoading",
    "searchParams"
  ], // props

  data () {
    return {
      isDialogOpened: false,
      isGettingIntervention: false,
      isUpdatingIntervention: false,
      resolve: null,
      reject: null,
      interventionId: 0,
      intervention: new Intervention(),
      readonly: true,

      beginLabel: "Begin Date*",
      endLabel: "End Date",

      deliveryMethods: [],
      interventionClasses: [],
    }
  }, // data

  mounted() {
    this.init()
  }, // mounted

  methods: {
    ...mapActions({
      getInterventionById: "interventions/getById",
      interventionUpdate: "interventions/update"
    }),

    init() {
      api.descriptors.getDeliveryMethods().then(response => this.deliveryMethods = response)
      api.descriptors.getInterventionClasses().then(response => this.interventionClasses = response)
    },

    /**
         * Dialog can either be opened with an interventionId or with an intervention.
         * If opened with an interventionId, dialog will make request to get intervention.
         * If opened with an intervention, dialog will show intervention without making request.
         * Readonly is optional, but will default to true.
         */

    open({ interventionId, intervention, readonly }) {
      this.isDialogOpened = true

      if (intervention) {
        this.intervention = intervention
        this.interventionId = intervention.id
        this.getIntervention()
      }

      if (interventionId) {
        this.interventionId = interventionId
        this.getIntervention(this.interventionId)
      }

      if (readonly)
      {this.readonly = readonly}

      return new Promise((resolve, reject) => {
        this.resolve = resolve
        this.reject = reject
      })
    },

    getIntervention () {
      this.isGettingIntervention = true

      this.getInterventionById(this.interventionId)
        .then((result) => {
          this.intervention = result.data
        })
        .finally(() => {
          this.isGettingIntervention = false
        })
    },

    close () {
      this.readonly = true
      this.isDialogOpened = false
    },

    edit () {
      this.readonly = false
    },

    save () {
      // TODO Update intervention

      this.resolve(this.intervention)

      this.isDialogOpened = false
      this.readonly = true
    },
    //Intervention saved!
    update (){
      const notification = this.$snotify.info("Saving intervention...", "", {
        timeout: 0,
        closeOnClick: false
      })
      this.isUpdatingIntervention = true

      this.interventionUpdate(this.intervention)
        .then(() => {
          this.$snotify.success("Intervention saved!", "", {
            timeout: 1500
          })
        })
        .catch((error) => this.$snotify.error(`Could not update intervention. ${error}`, "", {
          timeout: 4000
        }))
        .finally(() => {
          this.$snotify.remove(notification.id)
          this.isUpdatingIntervention = false
          this.save()
        })
    }
  }, // methods

  components: {
    DatePicker
  } // components
}
</script>
