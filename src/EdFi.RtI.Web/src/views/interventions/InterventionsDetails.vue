<template>
  <div>
    <v-app-bar>
      <v-btn
        icon
        :to="'/interventions'">
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-toolbar-title>Intervention Details</v-toolbar-title>

      <v-spacer></v-spacer>

      <v-btn
        v-if="isView"
        icon
        :disabled="isDeletingIntervention"
        :to="`/interventions/edit/${intervention.interventionId}`">
        <v-icon>mdi-pencil</v-icon>
      </v-btn>

      <v-btn
        v-if="isView"
        icon
        :disabled="isDeletingIntervention">
        <v-icon>mdi-delete</v-icon>
      </v-btn>
    </v-app-bar>
    <v-container>
      <!-- loading assesment -->
      <v-row
        v-if="isLoadingIntervention"
        class="mt-3">
        <v-col
          v-for="i of new Array(5)"
          :key="i"
          cols="12"
          sm="6"
          class="mb-6">
          <v-skeleton-loader type="text@2"></v-skeleton-loader>
        </v-col>
      </v-row><!-- loading assesment -->

      <v-row v-if="!isLoadingIntervention">
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="intervention.name"
            label="Name"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="intervention.classType"
            label="Class Type"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-menu
            ref="menu"
            v-model="menu"
            :close-on-content-click="false"
            :return-value.sync="date"
            transition="scale-transition"
            offset-y
            min-width="290px">
            <template v-slot:activator="{ on, attrs }">
              <v-text-field
                v-model="dateFormatted"
                label="Begin Date*"
                persistent-hint
                prepend-icon="mdi-calendar"
                readonly
                v-bind="attrs"
                @blur="date = parseDate(dateFormatted)"
                v-on="on"></v-text-field>
            </template>
            <v-date-picker
              v-model="beginDate"
              no-title
              scrollable>
              <v-spacer></v-spacer>
              <v-btn
                text
                color="primary"
                @click="menu = false">
                Cancel
              </v-btn>
              <v-btn
                text
                color="primary"
                @click="$refs.menu.save(beginDate)">
                OK
              </v-btn>
            </v-date-picker>
          </v-menu>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-menu
            ref="menu3"
            v-model="menu3"
            :close-on-content-click="false"
            :return-value.sync="date2"
            transition="scale-transition"
            offset-y
            min-width="290px">
            <template v-slot:activator="{ on, attrs }">
              <v-text-field
                v-model="dateFormatted2"
                label="End Date"
                persistent-hint
                prepend-icon="mdi-calendar"
                readonly
                v-bind="attrs"
                @blur="date2 = parseDate2(dateFormatted2)"
                v-on="on"></v-text-field>
            </template>
            <v-date-picker
              v-model="endDate"
              no-title
              scrollable>
              <v-spacer></v-spacer>
              <v-btn
                text
                color="primary"
                @click="menu3 = false">
                Cancel
              </v-btn>
              <v-btn
                text
                color="primary"
                @click="$refs.menu3.save(endDate)">
                OK
              </v-btn>
            </v-date-picker>
          </v-menu>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="intervention.deliveryMethodType"
            label="Delivery Method Type"
            hide-details
            :disabled="isView" />
        </v-col>
      </v-row>

      <div
        v-if="isEdit"
        class="d-flex justify-end">
        <v-btn
          color="primary"
          class="text-capitalize ml-2"
          :to="'/interventions'">
          Save
        </v-btn>
        <v-btn
          color="error"
          class="text-capitalize ml-2"
          :to="`/interventions/details/${intervention.interventionId}`">
          Cancel
        </v-btn>
      </div>
    </v-container>
  </div>
</template>

<script>
import { mapActions } from "vuex"
export default {
  data: (vm) => ({
    isEdit: false,
    isView: false,
    isDeletingIntervention: false,
    isLoadingIntervention: true,
    intervention: {
    },

    date: new Date().toISOString().substr(0, 10),
    dateFormatted: vm.formatDate(new Date().toISOString().substr(0, 10)),
    menu: false,
    modal: false,
    menu2: false,

    date2: new Date().toISOString().substr(0, 10),
    dateFormatted2: vm.formatDate2(new Date().toISOString().substr(0, 10)),
    menu3: false,
    modal2: false,
    menu4: false
  }),

  computed: {
    computedDateFormatted () {
      return this.formatDate(this.date)
    },
    computedDateFormatted2 () {
      return this.formatDate2(this.date2)
    }
  },

  watch: {
    date () {
      this.dateFormatted = this.formatDate(this.date)
    },
    date2 () {
      this.dateFormatted2 = this.formatDate2(this.date2)
    }
  },

  mounted (){
    this.isEdit = this.$route.path.includes("edit")
    this.isView = this.$route.path.includes("details")
    this.getInterventionById().then((result) => {
      this.intervention = result.data
    })
  },

  methods: {
    ...mapActions({
      getInterventionById: "interventions/getById"
    }),

    formatDate (date) {
      if (!date) {return null}

      const [
        year,
        month,
        day
      ] = date.split("-")
      return `${month}/${day}/${year}`
    },
    parseDate (date) {
      if (!date) {return null}

      const [
        month,
        day,
        year
      ] = date.split("/")
      return `${year}-${month.padStart(2, "0")}-${day.padStart(2, "0")}`
    },

    formatDate2 (date) {
      if (!date) {return null}

      const [
        year,
        month,
        day
      ] = date.split("-")
      return `${month}/${day}/${year}`
    },
    parseDate2 (date) {
      if (!date) {return null}

      const [
        month,
        day,
        year
      ] = date.split("/")
      return `${year}-${month.padStart(2, "0")}-${day.padStart(2, "0")}`
    }
  }
}
</script>
