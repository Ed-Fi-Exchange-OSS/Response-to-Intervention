<template>
    <div>
        <v-card>
            <v-data-table
                fixed-header
                height="800"
            :headers="headers"
            :items="items"
            :loading="isLoading"
            :options.sync="listViewOptions"
            :server-items-length="items.length"
            :hide-default-footer="true">

            <template v-slot:header.intervention="{ header }">
                <div style="min-width: 90px">
                <p class="text-center ma-0">
                    {{ header.intervention.identificationCode }}
                </p>
                <p
                    class="text-center ma-0 font-weight-light"
                    style="color: grey">
                    {{ getDateShort(header.intervention.beginDate) }}
                </p>
                </div>
            </template>

            <template v-slot:item.studentName="{ item }">
                <v-chip
                small
                color="primary"
                @click="viewStudent(item)">
                {{ item.student.fullName }}
                </v-chip>
            </template>

            <template v-slot:item.intervention="{ item, header }">
                <v-simple-checkbox
                :disabled="isLoading"
                :ripple="false"
                :value="isInterventionAssociated(item, header)"
                @click="interventionAssociationChecked($event, item, header)" />
            </template>
            </v-data-table>
        </v-card>

    <!-- dialogs -->
    <div>
      <AssessmentDetailsDialog ref="assessmentDetailsDialog" />
      <StudentProfileDialog ref="studentProfileDialog" />
    </div><!-- dialogs -->
  </div>
</template>

<script>
/* eslint-disable */

import AssessmentDetailsDialog from '@/components/dialogs/AssessmentDetailsDialog';
import StudentProfileDialog from '@/components/dialogs/StudentProfileDialog';
import { mapActions } from 'vuex';
import { dates } from '../../../mixins/mixins';

export default {

    props: {
        scorings: Array,
        interventions: Array,
        isLoading: Boolean,
        searchParams: Object,
    }, // props

    data: () => ({
        listViewOptions: {
            page: 1,
            itemsPerPage: 10,
        },

        isShowingInterventionsSideMenu: false,
    }), // data

    computed: {
        headers() {
            let headers = [
                {
                    sortable: false,
                    text: 'Student Name',
                    value: 'studentName',
                },
            ];

            this.interventions.forEach(intervention => {
                headers.push({
                    align: 'center',
                    sortable: false,
                    value: 'intervention',
                    intervention,
                });
            });

            return headers;
        },

        items() {
            return this.scorings;
        },
    }, // computed

    mounted() {

    }, // mounted

    methods: {

        emitCurrentDataSetChange() {
            console.log('emitCurrentDataSetChange()');
            let dataSetCopy = JSON.parse(JSON.stringify(this.scorings));
            this.$emit('data-set-changed', dataSetCopy);
        },

        /**
         * Each dynamic slot in the v-data-table must correspond to one of the associations
         * contained in a scoring item. If none if the associations of the provided item
         * has an assessment with an id matching the slot id, or slot assessment, then there
         * is no relation between the student and the assessment, therefore, this slot for
         * this item (or student) should be displayed as empty in the table.
         */
        getAssociationFromSlot(slot, item) {
            let assessmentId = slot.intervention.id;
            return item.associations.find(association => association.intervention.id == assessmentId);
        },

        getDateShort(dateStr) {
            return dates.methods.toLocalShort(dateStr);
        },

        /**
         * Checks if the provided scoring item has any association with the intervention from the header
         */
        isInterventionAssociated(item, header) {
            let headerInterventionId = header.intervention.id;
            let studentInterventionAssociationIds = item.associations.map(association => association.intervention.id);
            let uniqueAssociationIds = new Set(studentInterventionAssociationIds);
            return uniqueAssociationIds.has(headerInterventionId);
        },

        interventionAssociationChecked($event, item, header) {
            let student = item.student;
            let intervention = header.intervention;

            if (this.isInterventionAssociated(item, header))
                this.$emit('delete-student-intervention-association', { student, intervention });
            else
                this.$emit('add-student-intervention-association', { student, intervention });

            this.emitCurrentDataSetChange();
        },

        /**
         * Not all students are related to an assessment contained inside a slot.
         * If this is the case, then the slot should be displayed as empty.
         */
        isSlotAvailableForItem(slot, item) {
            let association = this.getAssociationFromSlot(slot, item);
            return association != null;
        },

        onAddInterventionClicked(item) {
            let studentUniqueId = item.student.studentUniqueId;
            let interventionIds = item.associations.map(association => association.intervention.id);

            this.$emit('add-intervention', {
                item,
                studentUniqueId,
                interventionIds: [...new Set(interventionIds)],
            });
        },

        viewAssessment(item) {
            let assessmentId = 'f102d4cfd225451eb8356db5e395e74d';
            this.$refs.assessmentDetailsDialog.open({ assessmentId });
        },

        viewStudent(item) {
            this.$refs.studentProfileDialog.open({ student: item.student })
            .catch(() => {});
        },
    }, // methods

    components: {
        AssessmentDetailsDialog,
        StudentProfileDialog,
    }, // components

    mixins: [ dates ],
};
</script>
