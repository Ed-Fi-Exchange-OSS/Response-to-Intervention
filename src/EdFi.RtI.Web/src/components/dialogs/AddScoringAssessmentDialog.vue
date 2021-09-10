<template>
  <v-dialog
    v-model="isDialogOpened"
    max-width="650"
    persistent>
    <v-card>
      <v-toolbar
        dense
        elevation="0.50">
        <v-toolbar-title>
          <h5 class="font-weight-regular">
            Add Assessment(s)
          </h5>
        </v-toolbar-title>
      </v-toolbar>

      <v-card-text>
        <v-alert
          class="mt-5 mb-5"
          type="warning">
          Make sure you undo or save your changes before adding an assessment. Changes will be automatically saved after adding the assessment.
        </v-alert>

        <v-select
          v-model="selectedAssessmentId"
          class="mb-5"
          label="Select an assessment"
          hide-details
          item-text="title"
          item-value="id"
          :items="assessments"
          :disabled="isLoadingAssessments"
          :loading="isLoadingAssessments" />

        <DatePicker
          v-model="administrationDate"
          :label-text="'Administration Date'" />
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn
          color="primary"
          class="text-capitalize ml-2"
          :disabled="isLoadingAssessments"
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
/* eslint-disable */

import DatePicker from '@/components/widgets/DatePicker';
import { mapActions } from 'vuex';
import { dates } from '../../mixins/mixins';

export default {

    name: 'AddScoringAssessmentDialog',

    data: vm => ({
        isDialogOpened: false,
        resolve: null,
        reject: null,

        assessments: [],
        assessmentsSnapshot: [],
        selectedAssessmentId: 0,
        isLoadingAssessments: false,

        administrationDate: new Date().toString(),
    }), // data

    mounted() {
        this.getAssessmentsRequest();
    }, // mounted

    methods: {
        ...mapActions({
            getAssessments: 'assesments/search',
        }),

        excludeAssessments(assessments) {
            let assessmentIds = new Set(assessments);
            this.assessments = this.assessments.filter(assess => !assessmentIds.has(assess.id));
        },

        getAssessmentsRequest() {
            this.isLoadingAssessments = true;
            this.selectedAssessmentId = 0;

            this.getAssessments({ 
              currentNamespace: true,
              getFromCache: false,
              storeInCache: false,
              sortDesc: false,
              sortField: "lastModifiedDate", 
            }).then(result => {
                this.assessments = result.data;
                this.assessmentsSnapshot = JSON.parse(JSON.stringify(this.assessments))
                this.selectDefaultAssessment();
            })
            .catch(error => {
                console.error('Could not get assessments.', error);
            })
            .finally(() => this.isLoadingAssessments = false);
        },

        isSelectedAssessmentValid() {
            return this.selectedAssessmentId && this.selectedAssessmentId > 0;
        },

        open({ excludeAssessments }) {
            this.isDialogOpened = true;
            this.assessments = this.assessmentsSnapshot
            
            if (excludeAssessments)
                this.excludeAssessments(excludeAssessments);

            this.selectDefaultAssessment();

            return new Promise((resolve, reject) => {
                this.resolve = resolve;
                this.reject = reject;
            });
        },

        onAdd() {
            let selectedAssessment = this.assessments.find(assess => assess.id == this.selectedAssessmentId);
            let selectedDate = dates.methods.embedNowTime(this.administrationDate);

            let result = {
                assessment: selectedAssessment,
                administrationDate: selectedDate.toISOString(),
            };

            this.resolve(result);
            this.isDialogOpened = false;
        },

        onCancel() {
            this.reject();
            this.isDialogOpened = false;
        },

        selectDefaultAssessment() {
            if (this.assessments.length > 0) {
                this.selectedAssessmentId = this.assessments[0].id;
            }
        },
    }, // methods

    components: {
        DatePicker,
    }, // components

    mixins: [ dates ],
}
</script>
