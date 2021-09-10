<template>
  <v-dialog
    v-model="isDialogOpened"
    persistent
    max-width="700">
    <v-card>
      <v-toolbar
        dense
        elevation="0.50">
        <v-toolbar-title>
          <h5 class="font-weight-regular">
            Assessment
          </h5>
        </v-toolbar-title>
      </v-toolbar>
      <v-card-text>
        <!-- loading assessment -->
        <v-row
          v-if="isGettingAssessment"
          class="mt-3">
          <v-col
            v-for="i of new Array(14)"
            :key="i"
            cols="12"
            sm="6"
            class="mb-6">
            <v-skeleton-loader type="text@2"></v-skeleton-loader>
          </v-col>
        </v-row><!-- loading assessment -->

        <!-- assessment loaded -->
        <v-row v-if="!isGettingAssessment">
          <v-col
            cols="12"
            sm="6">
            <v-text-field
              v-model="assessment.title"
              label="Title"
              hide-details
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-select
              v-model="assessment.categoryDescriptor"
              label="Category"
              hide-details
              :items="assessmentCategories"
              item-text="assessmentCategoryType"
              item-value="codeValue"
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-text-field
              v-model="assessment.form"
              label="Assessment Form"
              hide-details
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-text-field
              v-model="assessment.version"
              label="Version"
              hide-details
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <DatePicker
              v-model="assessment.revisionDate"
              :label-text.sync="revisonLabel"
              :is-disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-text-field
              v-model="assessment.maxRawScore"
              label="Max Raw Score"
              hide-details
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-text-field
              v-model="assessment.nomenclature"
              label="Nomenclature"
              hide-details
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-select
              v-model="assessment.periodDescriptor"
              label="Period Descriptor"
              :items="assessmentPeriods"
              item-text="description"
              item-value="codeValue"
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-select
              v-model="assessment.assessmentFamilyReference.title"
              label="Assessment Family Tree"
              :items="assessmentFamily"
              item-text="title"
              item-value="title"
              :disabled="readonly" />
          </v-col>
          <v-col
            v-if="hide"
            cols="12"
            sm="6">
            <v-text-field
              label="Assessment Family Reference"
              hide-details
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-select
              v-model="assessment.scores[0].assessmentReportingMethodType"
              label="Assessment Reporting Method Type"
              :items="assessmentReportingMethods"
              item-text="description"
              item-value="codeValue"
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-select
              v-model="assessment.scores[0].resultDatatypeType"
              label="Result Data Type"
              :items="assessmentResultData"
              item-text="description"
              item-value="codeValue"
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-text-field
              v-model="assessment.scores[0].minimumScore"
              label="Minimum Score"
              hide-details
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-text-field
              v-model="assessment.scores[0].maximumScore"
              label="Maximum Score"
              hide-details
              :disabled="readonly" />
          </v-col>
          <v-col
            cols="12"
            sm="6">
            <v-checkbox
              v-model="assessment.adaptiveAssessment"
              label="Adaptive Assesment"
              hide-details
              :disabled="readonly" />
          </v-col>
        </v-row><!-- assessment loaded -->
      </v-card-text>
      <v-card-actions>
        <v-spacer />

        <v-btn
          v-if="readonly && seeButton"
          color="primary"
          class="text-capitalize ml-2"
          :disabled="cantEdit"
          @click.native="edit">
          <v-icon left>
            mdi-pencil
          </v-icon>
          Edit
        </v-btn>

        <v-btn
          v-if="!readonly && seeButton"
          color="primary"
          class="text-capitalize ml-2"
          :disabled="isGettingAssessment || isUpdatingAssessment"
          :loading="isUpdatingAssessment"
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
/* eslint-disable */

import { mapActions, mapGetters } from 'vuex';
import DatePicker from '../widgets/DatePicker';
import { Assessment } from '../../models/models';
import api from '@/api';

export default {
    props: [
      "showEditButton",
    ],

    name: 'AssessmentDetailsDialog',

    data() {
        return {
            isDialogOpened: false,
            isGettingAssessment: false,
            isUpdatingAssessment: false,
            resolve: null,
            reject: null,
            assessmentId: 0,
            readonly: true,
            revisonLabel: "Revision Date",

            assessmentResultData: [
                "Integer",
                "Decimal",
                "Percentage",
                "Percentile",
                "Range",
                "Level"
            ],

            assessment: new Assessment(),
            assessmentAux: new Assessment(),

            seeButton: true,

            assessmentScores: {
                assessmentReportingMethodType:"",
                resultDatatypeType:"",
                minimumScore:"",
                maximumScore:""
            },

            hide: false,
            cantEdit: false,

            assessmentCategories: [],
            assessmentPeriods: [],
            assessmentFamily: [],
            assessmentReportingMethods: [],
            assessmentResults: [],
        };
    }, // data

    mounted() {
      this.init()
    }, // mounted

    methods: {
        ...mapActions({
            getAssesmentById: 'assesments/getById',
            assessmentUpdate: 'assesments/update',
        }),

        async init() {
          api.descriptors.getAssessmentCategories().then(response => this.assessmentCategories = response)
          api.descriptors.getAssessmentPeriods().then(response => this.assessmentPeriods = response)
          api.assessments.getAssessmentFamilies().then(response => this.assessmentFamily = response)
          api.descriptors.getAssessmentReportingMethods().then(response => this.assessmentReportingMethods = response)
          api.descriptors.getAssessmentItemResults().then(response => this.assessmentResults = response)
        },

        /**
         * Dialog can either be opened with an assessmentId or with an assessment.
         * If opened with an assessmentId, dialog will make request to get assessment.
         * If opened with an assessment, dialog will show assessment without making request.
         * Readonly is optional, but will default to true.
         */
        open({ assessmentId, assessment, readonly }) {
            this.isDialogOpened = true;

            if (assessment) {
                this.assessment = assessment;
                this.assessmentId = assessment.id;
                this.getAssessment();
            }

            if (assessmentId) {
                this.assessmentId = assessmentId;
                this.getAssessment();
            }

            if (!this.showEditButton)
                this.seeButton = false;

            return new Promise((resolve, reject) => {
                this.resolve = resolve;
                this.reject = reject;
            });
        },

        update(){ //TODO To update assessment family reference empty, it has to be null
            let notification = this.$snotify.info('Saving assessment...', '', { timeout: 0, closeOnClick: false });
            this.isUpdatingAssessment = true;

            this.assessment.maxRawScore = parseInt(this.assessment.maxRawScore);
            this.assessment.version = parseInt(this.assessment.version);

            this.assessmentUpdate(this.assessment).
            then(() => this.$snotify.success('Assessment saved!', '', { timeout: 1500 }))
            .catch(error => this.$snotify.error(`Could not update assessment. ${error}`, '', { timeout: 4000 }))
            .finally(() => {
                this.$snotify.remove(notification.id);
                this.isUpdatingAssessment = false;
                this.isDialogOpened = false;
                this.readonly = true;
                this.seeButton = true;
            });
        },

        getAssessment() {
            this.isGettingAssessment = true;
            this.cantEdit = true;

            this.getAssesmentById(this.assessmentId)
            .then(result => this.asessment = result.data)
            .finally(() => {
                this.isGettingAssessment = false
                
                if(!process.env.VUE_APP_EDFI_NAMESPACE == this.assessment.namespaceProperty)
                    this.cantEdit = true;
                else
                    this.cantEdit = false;
            });
            if (this.assessment.scores.length < 1)
                this.assessment.scores.push(this.assessmentScores);

            if (this.assessment.assessmentFamilyReference == null) {
                this.assessment.assessmentFamilyReference = this.assessmentAux.assessmentFamilyReference;
            }
        },

        close() {
            this.readonly = true;
            this.isDialogOpened = false;
            this.seeButton = true;
        },

        edit() {
            this.readonly = false;
        },

        save() {
            // TODO Update assessment
            this.resolve();
            
            this.isDialogOpened = false;
            this.readonly = true;
        },
    }, // methods

    components: {
        DatePicker,
    }, // components
}
</script>
