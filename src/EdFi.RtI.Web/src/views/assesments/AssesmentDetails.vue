<template>
  <div>
    <!-- toolbar -->
    <v-app-bar>
      <v-btn
        icon
        :to="'/assesments'">
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-toolbar-title>Assessment Details</v-toolbar-title>

      <v-spacer></v-spacer>

      <v-btn
        v-if="isView"
        :disabled="isDeletingAssesment"
        icon
        :to="`/assesments/edit/${assesment.id}`">
        <v-icon>mdi-pencil</v-icon>
      </v-btn>

      <v-btn
        v-if="isView"
        :disabled="isDeletingAssesment"
        icon
        @click="openConfirmDeleteDialog">
        <v-icon>mdi-delete</v-icon>
      </v-btn>
    </v-app-bar><!-- toolbar -->

    <v-container>
      <!-- loading assesment -->
      <v-row
        v-if="isLoadingAssesment"
        class="mt-3">
        <v-col
          v-for="i of new Array(14)"
          :key="i"
          cols="12"
          sm="6"
          class="mb-6">
          <v-skeleton-loader type="text@2"></v-skeleton-loader>
        </v-col>
      </v-row><!-- loading assesment -->

      <!-- assessment loaded -->
      <v-row v-if="!isLoadingAssesment">
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="assesment.title"
            label="Title"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-select
            v-model="assesment.categoryDescriptor"
            label="Category"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="assesment.form"
            label="Assesment Form"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="assesment.version"
            label="Version"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="assesment.revisionDate"
            label="Revision Date"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="assesment.maxRawScore"
            label="Max Raw Score"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="assesment.nomenclature"
            label="Nomenclature"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-select
            v-model="assesment.periodDescriptor"
            label="Period Descriptor"
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="assesment.assessmentFamilyReference"
            label="Assesment Family Reference"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-select
            v-model="assesment.assesmentReportingMethodType"
            label="Assesment Reporting Method Type"
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
                hide-details
                :disabled="isView"
                label="Begin Date*"
                persistent-hint
                prepend-icon="mdi-calendar"
                readonly
                v-bind="attrs"
                @blur="date = parseDate(dateFormatted)"
                v-on="on"></v-text-field>
            </template>
            <v-date-picker
              v-model="date"
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
                @click="$refs.menu.save(date)">
                OK
              </v-btn>
            </v-date-picker>
          </v-menu>
        </v-col>
        <!--<v-col cols="12" sm="6">
                    <v-select
                        label="Result Data Type"
                        :disabled="isView"
                        v-model="assesment.resultDataType"/>
                </v-col>-->
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="assesment.minimumScore"
            label="Minimum Score"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            v-model="assesment.maximumScore"
            label="Maximum Score"
            hide-details
            :disabled="isView" />
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-checkbox
            v-model="assesment.adaptiveAssessment"
            label="Adaptive Assesment"
            hide-details
            :disabled="isView" />
        </v-col>
      </v-row><!-- assessment loaded -->

      <div
        v-if="isEdit"
        class="d-flex justify-end">
        <v-btn
          color="primary"
          class="text-capitalize ml-2"
          :disabled="isLoadingAssesment || isSavingAssesment"
          :loading="isSavingAssesment"
          @click="saveAssesment">
          Save
        </v-btn>
        <v-btn
          color="error"
          class="text-capitalize ml-2"
          :disabled="isLoadingAssesment || isSavingAssesment"
          :to="`/assesments/details/${assesment.id}`">
          Cancel
        </v-btn>
      </div>
    </v-container>

    <!-- yes/no dialog -->
    <YesNoDialog ref="yesNoDialog" />
  </div>
</template>

<script>
import { mapActions } from "vuex"
import YesNoDialog from "../../components/dialogs/YesNoDialog"

export default {
  data: (vm) => ({
    isEdit: false,
    isView: false,
    isDeletingAssesment: false,
    isLoadingAssesment: false,
    isSavingAssesment: false,

    assesmentId: 0,
    assesment: {
    },

    date: new Date().toISOString().substr(0, 10),
    dateFormatted: vm.formatDate(new Date().toISOString().substr(0, 10)),
    menu: false,
    modal: false,
    menu2: false
  }), // mounted

  computed: {
    computedDateFormatted () {
      return this.formatDate(this.date)
    }
  },

  watch: {
    date () {
      this.dateFormatted = this.formatDate(this.date)
    }
  }, // data

  mounted () {
    this.isEdit = this.$route.path.includes("edit")
    this.isView = this.$route.path.includes("details")

    this.assesmentId = this.$route.params.id
    this.searchAssesment()
  },

  methods: {
    ...mapActions({
      getAssesmentById: "assesments/getById",
      deleteAssesmentById: "assesments/delete",
      updateAssesment: "assesments/update"
    }),

    searchAssesment () {
      this.isLoadingAssesment = true

      this.getAssesmentById(this.assesmentId)
        .then((result) => {
          this.assesment = result.data
          console.log("Assesment", result)
        })
        .finally(() => this.isLoadingAssesment = false)
    },

    openConfirmDeleteDialog () {
      this.$refs.yesNoDialog.open({
        title: "Delete Assesment",
        message: "Are you sure you want to delete this assesment?"
      })
        .then(() => {
          console.log("Yes clicked!")
          this.deleteAssesment()
        })
        .catch((error) => {
          console.log("No clicked!", error)
        })
    },

    deleteAssesment () {
      const notification = this.$snotify.info("Deleting assesment...", "", {
        timeout: 0,
        closeOnClick: false
      })//Here
      this.isDeletingAssesment = true

      this.deleteAssesmentById(1)
        .then(() => {
          this.$snotify.success("Assesment deleted!", "", {
            timeout: 1500
          })
          this.$router.push("/assesments")
        })
        .catch((error) => this.$snotify.error(`Could not delete assesment. ${error}`, "", {
          timeout: 4000
        }))
        .finally(() => {
          this.$snotify.remove(notification.id)
          this.isDeletingAssesment = false
        })
    },

    saveAssesment () {
      const notification = this.$snotify.info("Updating assesment...", "", {
        timeout: 0,
        closeOnClick: false
      })
      this.isSavingAssesment = true

      this.updateAssesment(this.assesment)
        .then(() => {
          this.$snotify.success("Assesment updated!", "", {
            timeout: 1500
          })
          this.$router.push(`/assesments/details/${this.assesment.id}`)
        })
        .catch((error) => this.$snotify.error(`Could not update assesment. ${error}`, "", {
          timeout: 4000
        }))
        .finally(() => {
          this.$snotify.remove(notification.id)
          this.isSavingAssesment = true
        })
    },

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
    }
  }, // methods

  components: {
    YesNoDialog
  } // components
}
</script>

