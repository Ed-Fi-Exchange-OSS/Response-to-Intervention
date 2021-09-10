<template>
  <v-dialog
    v-model="isDialogOpened"
    max-width="600"
    persistent>
    <v-card>
      <v-toolbar
        dense
        elevation="0.50">
        <v-toolbar-title>
          <h5 class="font-weight-regular">
            #{{ student.studentUniqueId }}: {{ student.firstName }} {{ student.lastSurname }}
          </h5>
        </v-toolbar-title>
      </v-toolbar>
      <v-card-text>
        <div v-if="student.electronicMails && student.electronicMails.length > 0">
          <v-subheader>
            Email Address
          </v-subheader>
          <v-divider></v-divider>
          <v-text-field
            v-for="item in student.electronicMails"
            :key="item.electronicMails"
            v-model="item.electronicMailAddress"
            value="Text value"
            disabled></v-text-field>
        </div>

        <div v-if="student.characteristics && student.characteristics.length > 0 && student.characteristics && student.characteristics.descriptor != null">
          <v-subheader>
            Characteristics
          </v-subheader>
          <v-divider></v-divider>
          <v-text-field
            v-for="item in student.characteristics"
            :key="item.descriptor"
            v-model="item.descriptor"
            value="Text value"
            disabled></v-text-field>
        </div>

        <div v-if="student.telephones && student.telephones.length > 0">
          <v-subheader>
            Phone Numbers
          </v-subheader>
          <v-divider></v-divider>
          <v-text-field
            v-for="item in student.telephones"
            :key="item.telephoneNumber"
            v-model="item.telephoneNumber"
            value="Text value"
            disabled></v-text-field>
        </div>

        <div v-if="student.disabilities != null">
          <v-subheader>
            Disabilities
          </v-subheader>
          <v-divider></v-divider>
          <v-text-field
            v-model="student.disabilities"
            value="Text value"
            disabled></v-text-field>
        </div>

        <div v-if="student.birthDate != null">
          <v-subheader>
            Birth Date
          </v-subheader>
          <v-divider></v-divider>
          <v-text-field
            v-model="student.birthDate"
            value="Text value"
            disabled></v-text-field>
        </div>

        <div v-if="student.indicators && student.indicators.length > 0">
          <v-subheader>
            Indicators
          </v-subheader>
          <v-divider></v-divider>
          <v-text-field
            v-for="item in student.indicators"
            :key="item"
            value="Text value"
            disabled></v-text-field>
        </div>

        <div v-if="student.sexType != null">
          <v-subheader>
            Sex
          </v-subheader>
          <v-divider></v-divider>
          <v-text-field
            v-model="student.sexType"
            value="Text value"
            disabled></v-text-field>
        </div>

        <div v-if="student.programParticipations && student.programParticipations.length > 0">
          <v-subheader>
            Programs
          </v-subheader>
          <v-divider></v-divider>
          <v-text-field
            v-for="item in student.programParticipations"
            :key="item"
            value="Text value"
            disabled></v-text-field>
        </div>

        <div v-if="student.races && student.races.length > 0">
          <v-subheader>
            Race(s)
          </v-subheader>
          <v-divider></v-divider>
          <v-text-field
            v-for="item in student.races"
            :key="item.raceType"
            v-model="item.raceType"
            value="Text value"
            disabled></v-text-field>
        </div>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn
          color="error"
          class="text-capitalize ml-2"
          @click.native="onCancel">
          Close
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import { dates } from "../../mixins/mixins"
export default {
  name: "StudentProfileDialog", // methods

  mixins: [
    dates
  ],

  data: () => ({
    isDialogOpened: false,
    resolve: null,
    reject: null,

    example: [
      "DD",
      "edff",
      "HHJJ"
    ],
    studentUniqueId: 0,
    student: {
    }
  }), // data

  computed: {
    studentEmail() {
      if (!this.student) {return ""}
      if (!this.student.electronicMails) {return ""}
      if (this.student.electronicMails.length == 0) {return ""}
      return this.student.electronicMails[0].electronicMailAddress
    },

    studentCharacteristics () {
      if (!this.student) {return ""}
      if (!this.student.characteristics) {return ""}
      if (this.student.characteristics.length == 0) {return ""}
      return this.student.characteristics[0]
    },

    studentPhone () {
      if (!this.student) {return ""}
      if (!this.student.telephones) {return ""}
      if (this.student.telephones.length == 0) {return ""}
      return this.student.telephones[0].telephoneNumber
    }
  }, // computed

  methods: {
    open ({ studentUniqueId, student }) {
      this.isDialogOpened = true

      if (studentUniqueId) {
        this.studentUniqueId = studentUniqueId
        // TODO Get student by request
      }
      else {
        this.studentUniqueId = student.studentUniqueId
        this.student = student
        this.student.birthDate = this.getDateFormat(this.student.birthDate)
      }

      return new Promise((resolve, reject) => {
        this.resolve = resolve
        this.reject = reject
      })
    },

    onCancel () {
      if (this.reject) {this.reject()}
      this.isDialogOpened = false
    },

    getDateFormat (date) {
      return dates.methods.toLocalShort(date)
    }
  }
}
</script>
