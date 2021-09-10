<template>
  <div>
    <!-- toolbar -->
    <v-app-bar>
      <v-btn
        icon
        :to="'/assesments'">
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-toolbar-title>Add Assessment</v-toolbar-title>
    </v-app-bar><!-- toolbar -->

    <v-container class="py-8">
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

      <v-expansion-panels
        multiple
        focusable
        :value="getFormExpandedValue()">
        <!-- basic data configuration -->
        <v-expansion-panel>
          <v-expansion-panel-header>Basic Data Configuration</v-expansion-panel-header>
          <v-expansion-panel-content>
            <v-form v-model="isForm1Valid">
              <v-row>
                <v-col
                  cols="12"
                  sm="6">
                  <v-text-field
                    v-model="assessment.title"
                    label="Title *"
                    :rules="rules.assessments.title" />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-select
                    v-model="assessment.categoryDescriptor"
                    label="Category *"
                    :items="assessmentCategories"
                    item-text="assessmentCategoryType"
                    item-value="codeValue"
                    :rules="rules.assessments.title" />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-text-field
                    v-model="assessment.form"
                    label="Assessment Form" />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-text-field
                    v-model="assessment.version"
                    label="Version" />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <DatePicker
                    v-model="assessment.revisionDate"
                    :label-text.sync="revisionLabel" />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-text-field
                    v-model="assessment.maxRawScore"
                    label="Max Raw Score" />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-text-field
                    v-model="assessment.nomenclature"
                    label="Nomenclature" />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-select
                    v-model="assessment.periodDescriptor"
                    label="Period Descriptor *"
                    :items="assessmentPeriods"
                    item-text="description"
                    item-value="codeValue"
                    :rules="rules.assessments.title"/>
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-select
                    v-model="assessment.assessmentFamilyReference.title"
                    label="Assessment Family Tree"
                    :items="assessmentFamily"
                    item-text="title"
                    item-value="title" />
                </v-col>
                <v-col
                  v-if="hide"
                  cols="12"
                  sm="6">
                  <v-text-field
                    label="Assessment Family Tree"
                    hide-details />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-checkbox
                    v-model="assessment.adaptiveAssessment"
                    label="Adaptive Assessment" />
                </v-col>
              </v-row>
            </v-form>
          </v-expansion-panel-content>
        </v-expansion-panel><!-- basic data configuration -->

        <!-- scoring configuration -->
        <v-expansion-panel :disabled="!isForm1Valid">
          <v-expansion-panel-header>Scoring Configuration</v-expansion-panel-header>
          <v-expansion-panel-content>
            <v-form v-model="isForm2Valid">
              <v-row>
                <v-col
                  cols="12"
                  sm="6">
                  <v-select
                    v-model="assessment.scores[0].assessmentReportingMethodType"
                    label="Assessment Reporting Method Type *"
                    :items="assessmentReportingMethods"
                    item-text="description"
                    item-value="codeValue"
                    :rules="rules.assessments.title" />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-select
                    v-model="assessment.scores[0].resultDatatypeType"
                    label="Result Data Type *"
                    :items="assessmentResultData"
                    item-text="description"
                    item-value="codeValue"
                    :rules="rules.assessments.title" />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-text-field
                    v-model="assessment.scores[0].minimumScore"
                    label="Minimum Score" />
                </v-col>
                <v-col
                  cols="12"
                  sm="6">
                  <v-text-field
                    v-model="assessment.scores[0].maximumScore"
                    label="Maximum Score" />
                </v-col>
              </v-row>
            </v-form>
          </v-expansion-panel-content>
        </v-expansion-panel><!-- scoring configuration -->
      </v-expansion-panels>

      <div class="d-flex justify-end mt-6">
        <!--<v-btn color="primary" class="text-capitalize ml-2" :disabled="isAddingAssessment" :loading="isAddingAssessment" @click="addAssessment">Add</v-btn>-->
        <v-btn color="primary" class="text-capitalize ml-2"
          :disabled="!isFormValid() || isAddingAssessment"
          :loading="isAddingAssessment"
          @click="addAssessment">
          Save
        </v-btn>
        <v-btn
          color="error"
          class="text-capitalize ml-2"
          :disabled="isAddingAssessment"
          to="/assesments">
          Cancel
        </v-btn>
      </div>
    </v-container>

    <YesNoDialog ref="yesNoDialog" />
    <!-- dialogs -->
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex"
import { forms } from "../../mixins/mixins"
import { Assessment } from "../../models/models"
import DatePicker from "../../components/widgets/DatePicker"
import YesNoDialog from "../../components/dialogs/YesNoDialog"
import api from '@/api'

export default {
  data: () => ({
    isForm1Valid: false,
    isForm2Valid: false,
    isAddingAssessment: false,
    assessment: new Assessment(),

    assessmentResultData: [
      "Integer",
      "Decimal",
      "Percentage",
      "Percentile",
      "Range",
      "Level"
    ],

    dateTest: new Date().toString(),

    revisionDate: new Date().toISOString().substr(0, 10),
    revisionLabel: "Revision Date",

    readonly: true,
    noti: "",
    hide: false,
    justMadePost: false,

    assessmentCategories: [],
    assessmentPeriods: [],
    assessmentFamily: [],
    assessmentReportingMethods: [],
    assessmentResults: [],
  }), // data

  beforeRouteLeave (to, from, next) {
    if (this.justMadePost)
    {next()}
    else {
      this.$refs.yesNoDialog.open({
        title: "Back to assessments list",
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
  },

  mounted () {
    this.init();
  }, // mounted

  computed: {
    ...mapGetters({
      getOrganizationId: "session/getOrganizationId",
    }),

    isNewAssessment() {
      return this.assessment.id == undefined || this.assessment.id == null || this.assessment.id.trim().length == 0
    },
  },

  methods: {
    ...mapActions({
      createAssessment: "assesments/create"
    }),

    async init() {
      api.descriptors.getAssessmentCategories().then(response => this.assessmentCategories = response)
      api.descriptors.getAssessmentPeriods().then(response => this.assessmentPeriods = response)
      api.assessments.getAssessmentFamilies().then(response => this.assessmentFamily = response)
      api.descriptors.getAssessmentReportingMethods().then(response => this.assessmentReportingMethods = response)
      api.descriptors.getAssessmentItemResults().then(response => this.assessmentResults = response)
    },

    addAssessment() {
      // To add assessment family reference empty, it has to be empty object: {}
      
      this.justMadePost = true;
      this.isAddingAssessment = true;

      const notification = this.$snotify.info("Adding assessment...", "", {
        timeout: 0,
        closeOnClick: false
      });

      if (this.assessment.assessmentFamilyReference.title == "")
        this.assessment.assessmentFamilyReference = {}

      if (this.assessment.version == "")
        this.assessment.version = 0

      if (this.assessment.maxRawScore == "")
        this.assessment.maxRawScore = 0

      this.assessment.educationOrganizationReference.educationOrganizationId = this.getOrganizationId;

      this.createAssessment(this.assessment)
      .then(() => {
        this.$snotify.success("Assessment added!", "", { timeout: 1500 });
        this.$router.push("/assesments");
      })
      .catch((error) => this.$snotify.error(`Could not add assessment. ${error}`, "", { timeout: 4000 }))
      .finally(() => {
        this.$snotify.remove(notification.id);
        this.isAddingAssessment = false;
      });
    },

    getFormExpandedValue () {
      const value = [ 0 ]

      if (this.isForm1Valid)
        value.push(1);

      return value
    },

    isFormValid () {
      return this.isForm1Valid && this.isForm2Valid;
    }
  }, // methods

  components: {
    DatePicker,
    YesNoDialog
  }, // components

  mixins: [
    forms
  ]
}
</script>

