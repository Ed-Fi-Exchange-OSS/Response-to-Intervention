<template>
  <div>

    <v-container>

        <AppToolbar
            :title="toolbarTitle"
            :icon="toolbarIcon"
            :buttons="buttons"
            :showSearch="false"
            :disabledButtons="!isShowingTable"
            class="mb-5">
        </AppToolbar>

      <ScoringInterventionsFilters
        class="mb-3"
        @staff-selected="onStaffSelected"
        @section-selected="onSectionSelected" />

      <ScoringInterventionsTable
        v-if="isShowingTable"
        :scorings="scorings"
        :interventions="interventions"
        :is-loading="false"
        @add-student-intervention-association="addStudentInterventionAssociation"
        @delete-student-intervention-association="deleteStudentInterventionAssociation"
        @data-set-changed="onDataSetChanged"/>

      <TableSkeletonLoader
        v-if="isShowingSkeleton"
        :column-count="columns"
        :row-count="rows" />

      <!-- loading message -->
      <div
        v-if="isShowingLoadingMessage"
        class="mt-6">
        <h5
          class="font-weight-regular text-center"
          style="color: gray">
          Please wait<br>
          You can select a teacher as soon as the filters are loaded
        </h5>
      </div><!-- loading message -->

    </v-container>

    <!-- dialogs -->
    <div>
      <AddScoringInterventionDialog ref="addScoringInterventionDialog" />
      <LoadingDialog ref="loadingDialog" />
      <YesNoDialog ref="yesNoDialog" />
    </div><!-- dialogs -->
  </div>
</template>

<script>
/* eslint-disable */

import AddScoringInterventionDialog from '@/components/dialogs/AddScoringInterventionDialog';
import LoadingDialog from '@/components/dialogs/LoadingDialog';
import ScoringInterventionsFilters from './ScoringInterventionsFilters';
import ScoringInterventionsTable from './ScoringInterventionsTable';
import ScoringInterventionsToolbar from './ScoringInterventionsToolbar';
import { StudentInterventionAssociation } from '../../../models/api/StudentInterventionAssociation';
import TableSkeletonLoader from '@/components/widgets/TableSkeletonLoader';
import YesNoDialog from '@/components/dialogs/YesNoDialog';
import AppToolbar from '../../../components/widgets/AppToolbar.vue';

import { mapActions, mapGetters } from 'vuex';
import { csv } from '../../../mixins/mixins';

export default {

    data() {
        return {
            scorings: [],
            scoringsHistory: [],
            interventions: [],
            isLoadingScorings: false,
            isShowingTable: false,
            isShowingSkeleton: false,
            isShowingLoadingMessage: true,
            isShowingErrorMessage: false,
            columns: 5,
            rows: 8,
            filters: {
                staffUniqueId: undefined, // string
                sectionId: undefined, // string
                showInactiveStudents: undefined, // bool
            },
            toolbarTitle: 'Scoring Interventions',
            toolbarIcon: 'mdi-text-box-check',
            buttons: [
                {
                    icon: 'mdi-rotate-right',
                    text: 'Refresh',
                    click: () => this.onRefreshClicked(),
                },
                {
                    icon: 'mdi-file-export-outline',
                    text: 'Export',
                    click: () => this.onExportClicked(),
                },
                // {
                //     icon: 'mdi-undo',
                //     text: 'Undo',
                //     click: () => this.onUndoClicked(),
                //     disabled: () => !this.isUndoEnabled,
                // },
                // {
                //     icon: 'mdi-content-save',
                //     text: 'Save',
                //     click: () => this.onSaveClicked(),
                // },
            ],
        }
    }, // data
    
    computed: {
        isScoringHistoryEmpty() {
            return !this.scoringsHistory || this.scoringsHistory.length == 0;
        },

        isUndoEnabled() {
            return !this.isScoringHistoryEmpty;
        },
    }, // computed

    mounted() {
        this.getInterventionsRequest();
    }, // mounted

    methods: {
        ...mapActions({
            cacheScoringInterventions: 'cache/cacheScoringInterventions',
            createAssociation: 'scoringInterventions/createAssociation',
            deleteAssociation: 'scoringInterventions/deleteAssociation',
            getInterventions: 'interventions/searchWithParams',
            getScorings: 'scoringInterventions/getScorings',
            getScoringsByStudent: 'scoringInterventions/getScoringsByStudent',
            getStudentsBySection: 'students/getBySection',
        }),

        addScoring({ selectedScoring, selectedIntervention }) {
            console.log('addScoring');
            console.log('selectedScoring', selectedScoring);
            console.log('selectedIntervention', selectedIntervention);
            console.log('');

            let newScoring = {
                intervention: selectedIntervention.intervention,
                associationModel: new StudentInterventionAssociation(),
            };

            selectedScoring.associations.push(newScoring);

            console.log('Adding new scoring:', selectedScoring);

            this.addScoring(selectedScoring)
            .then(updatedScoring => {
                console.log('Scoring added!', updatedScoring);
            })
            .catch(error => {
                console.log('Could not add scoring:', error);
            })
            .finally(() => {});
        },

        addStudentInterventionAssociation({ student, intervention }) {
            let foundScoring = this.scorings.find(scoring => scoring.student.studentUniqueId == student.studentUniqueId);
            let studentInterventionAssociationDTO = {
                associationModel: {}, // StudentInterventionAssociation coming from backend after creating it
                intervention,
            };

            foundScoring.associations.push(studentInterventionAssociationDTO);

            const body = {
                studentUniqueId: student.studentUniqueId,
                interventionId: intervention.id,
            };

            this.createAssociation(body)
            .then(result => {
                studentInterventionAssociationDTO.associationModel = result.data;
            })
            .catch(error => {
                console.error('Could not create association:', error);
            });
        },

        deleteStudentInterventionAssociation({ student, intervention }) {
            console.log('deleteStudentInterventionAssociation()');
            console.log('student', student);
            console.log('intervention', intervention);

            let foundScoring = this.scorings.find(scoring => scoring.student.studentUniqueId == student.studentUniqueId);
            console.log('foundScoring', foundScoring);

            let foundAssociation = foundScoring.associations.find(association => association.intervention.id == intervention.id);
            console.log('foundAssociation', foundAssociation);

            let studentInterventionAssociation = foundAssociation.associationModel;
            console.log('Deleting association', studentInterventionAssociation);

            foundScoring.associations = foundScoring.associations.filter(association => association.intervention.id != intervention.id);

            const body = {
                studentInterventionAssociationId: studentInterventionAssociation.id,
                studentUniqueId: student.studentUniqueId,
                interventionIdentificationCode: intervention.identificationCode,
            };

            this.deleteAssociation(body)
            .then(result => {
                console.log('Deleted association!');
                // TODO Remove from scorings
            })
            .catch(error => {
                console.error('Could not delete association:', error);
            });
        },

        getInterventionsRequest() {
            const searchParams = {
                getFromCache: true,
            };

            this.getInterventions(searchParams)
            .then(result => this.interventions = result.data)
            .catch(error => console.error('Could not get interventions:', error));
        },

        getScoringsRequest() {
            this.isShowingSkeleton = true;
            this.isShowingTable = false;
            this.isLoadingScorings = false;
            this.columns = 5;
            this.rows = 8;
            this.scorings = [];

            this.getScorings(this.filters)
            .then(result => this.scorings = result.data)
            .catch(error => console.error('Could not get scoring interventions:', error))
            .finally(() => {
                this.isShowingSkeleton = false;
                this.isShowingTable = true;
                this.isLoadingScorings = false;
            });
        },

        getScoringsSnapshot() {
            if (!this.scorings)
                return [];

            return JSON.parse(JSON.stringify(this.scorings));
        },

        onAddInterventionClicked(selectedRow) {
            let excludeInterventions = selectedRow.interventionIds;

            this.$refs.addScoringInterventionDialog.open({ excludeInterventions })
            .then(selectedIntervention => this.addScoring({ selectedScoring: selectedRow.item, selectedIntervention }))
            .catch(() => {});
        },

        onAddInterventionAllClicked() {
            this.$refs.addScoringInterventionDialog.open({ excludeInterventions: [] })
            .then(selectedIntervention => {
                this.scorings.forEach(scoring => {
                    this.addScoring({ selectedScoring: scoring, selectedIntervention });
                });
            }).catch(() => {});
        },

        onDataSetChanged(scorings) {
            if (this.isScoringHistoryEmpty) {
                let snapshot = this.getScoringsSnapshot();
                this.scoringsHistory.push(snapshot);
            }
            
            this.scoringsHistory.push(scorings);
        },

        onRefreshClicked() {
            this.getScoringsRequest();
        },

        onExportClicked() {
            console.log('onExportClicked()');
            csv.methods.exportInterventions(this.scorings, this.interventions);
        },

        onStaffSelected(selectedStaff) {
            console.log('onStaffSelected', selectedStaff);
            this.isShowingLoadingMessage = false;
            
            this.columns = 5;
            this.rows = 8;
            this.scorings = [];
            this.isLoadingScorings = true;
            this.isShowingTable = false;
            this.isShowingSkeleton = true;
        },

        onSectionSelected(selectedSection) {
            console.log('onSectionSelected', selectedSection);
            this.filters.sectionId = selectedSection.id;
            this.getScoringsRequest();
        },

        onSaveClicked() {
            this.$refs.yesNoDialog.open({ title: 'Save Changes', message: 'Are you sure you want to save these changes? You won\'t be able to undo them.' })
            .then(() => this.onSaveConfirm())
            .catch(error => {});
        },

        onSaveConfirm() {
            this.$refs.loadingDialog.open({ title: 'Saving changes'});

            this.cacheScoringInterventions(this.filters)
            .then(() => {
                console.log('Changes have been saved!');
                this.$snotify.success('Changes have been saved!', '', { timeout: 2000 });
                this.scoringsHistory = [];
            })
            .catch(error => {
                console.error('Could not save changes :(');
                this.$snotify.error(`Could not save changes: ${error}`, '', { timeout: 4000 });
            })
            .finally(() => this.$refs.loadingDialog.close());
        },

        onUndoClicked() {
            if (this.isScoringHistoryEmpty)
                return;
            
            this.scoringsHistory.pop();

            if (this.isScoringHistoryEmpty)
                return;
            
            let topIndex = this.scoringsHistory.length - 1;
            this.scorings = this.scoringsHistory[topIndex];
        },
    }, // methods

    components: {
        AddScoringInterventionDialog,
        LoadingDialog,
        ScoringInterventionsFilters,
        ScoringInterventionsTable,
        ScoringInterventionsToolbar,
        TableSkeletonLoader,
        YesNoDialog,
        AppToolbar,
    }, // components

    mixins: [ csv ],
}
</script>
