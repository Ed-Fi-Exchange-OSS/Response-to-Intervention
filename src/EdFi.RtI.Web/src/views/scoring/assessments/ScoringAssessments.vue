<template>
    <div>

        <v-container>

            <AppToolbar
                class="mb-5"
                :title="toolbarTitle"
                :icon="toolbarIcon"
                :buttons="buttons"
                :showSearch="false"
                :disabledButtons="!isShowingTable"/>

            <ScoringAssessmentsFilters
                class="mb-3"
                :categories="categoriesFromScorings"
                :is-categories-disabled="isShowingLoadingMessage || isShowingSkeleton"
                @staff-selected="onStaffSelected"
                @section-selected="onSectionSelected"
                @category-selected="onCategorySelected"/>

            <!-- namespace -->
            <div v-if="!isShowingLoadingMessage"
                class="d-flex justify-center mb-3">
                <v-chip :color="searchParams.onlyFromCurrentNamespace == true ? 'accent' : null"
                    :class="{ 'font-weight-bold': searchParams.onlyFromCurrentNamespace == true }"
                    :disabled="!isSectionSelected"
                    @click="onNamespaceClicked(true)">
                    Local
                </v-chip>
                <div class="mx-2"></div>
                <v-chip :color="searchParams.onlyFromCurrentNamespace == false ? 'accent' : null"
                    :class="{ 'font-weight-bold': searchParams.onlyFromCurrentNamespace == false }"
                    :disabled="!isSectionSelected"
                    @click="onNamespaceClicked(false)">
                    External
                </v-chip>
            </div><!-- namespace -->

            <ScoringAssessmentsTable
                v-if="isShowingTable"
                :scorings="scoringsByCategory"
                :is-loading="false"
                :readonly="isReadonly"
                @add-assessment="onAddAssessmentClicked"
                @delete-assessment="onDeleteAssessmentClicked"
                @data-set-changed="onDataSetChanged"/>

            <TableSkeletonLoader
                v-if="isShowingSkeleton"
                :column-count="columns"
                :row-count="rows" />

            <!-- loading message -->
            <div v-if="isShowingLoadingMessage" class="mt-6">
                <h5 class="font-weight-regular text-center" style="color: gray">
                    Please wait<br>
                    You can select a teacher as soon as the filters are loaded
                </h5>
            </div><!-- loading message -->
        </v-container>

        <!-- dialogs -->
        <div>
            <AddScoringAssessmentDialog ref="addScoringAssessmentDialog" />
            <LoadingDialog ref="loadingDialog" />
            <YesNoDialog ref="yesNoDialog" />
            <AssessmentListDialog ref="assessmentListDialog" />
        </div><!-- dialogs -->

    </div>
</template>

<script>
/* eslint-disable */

import AddScoringAssessmentDialog from '@/components/dialogs/AddScoringAssessmentDialog';
import LoadingDialog from '@/components/dialogs/LoadingDialog';
import ScoringAssessmentsFilters from './ScoringAssessmentsFilters';
import ScoringAssessmentsTable from './ScoringAssessmentsTable';
import ScoringAssessmentsToolbar from './ScoringAssessmentsToolbar';
import TableSkeletonLoader from '../../../components/widgets/TableSkeletonLoader';
import YesNoDialog from '@/components/dialogs/YesNoDialog';
import AssessmentListDialog from '@/components/dialogs/AssessmentListDialog';
import AppToolbar from '../../../components/widgets/AppToolbar.vue';

import { mapActions, mapGetters } from 'vuex';
import { csv, dates } from '../../../mixins/mixins';

export default {

    data() {
        return {
            scorings: [],
            scoringsHistory: [],
            categoriesFromScorings: [ 'All' ],
            associationsToDelete: [],
            associationsToDeleteAll: [],
            isLoadingScorings: false,
            isShowingTable: false,
            isShowingSkeleton: false,
            isShowingLoadingMessage: true,
            isShowingErrorMessage: false,
            columns: 5,
            rows: 8,
            searchParams: {
                staffUniqueId: undefined, // string
                uniqueSectionCode: undefined, // string
                category: undefined, // any
                showInactiveStudents: undefined, // bool
                getFromCache: true,
                storeInCache: false,
                onlyFromCurrentNamespace: true,
            },
            toolbarTitle: 'Scoring Assessments',
            toolbarIcon: 'mdi-view-dashboard',
            buttons: [
                {
                    icon: 'mdi-plus',
                    text: 'Add',
                    click: () => this.onAddAssessmentAllClicked(),
                    disabled: () => this.isReadonly,
                },
                {
                    icon: 'mdi-delete',
                    text: 'Delete',
                    click: () => this.onDeleteAssessmentAllClicked(),
                    disabled: () => this.isReadonly,
                },
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
                {
                    icon: 'mdi-undo',
                    text: 'Undo',
                    click: () => this.onUndoClicked(),
                    disabled: () => this.isread || !this.isUndoEnabled,
                },
                {
                    icon: 'mdi-content-save',
                    text: 'Save',
                    click: () => this.onSaveClicked(),
                    disabled: () => this.isReadonly,
                },
            ],
        }
    }, // data
    
    computed: {
        ...mapGetters({
            currentSchoolId: 'catalog/getCurrentSchoolId',
        }),

        isReadonly() {
            return this.searchParams.onlyFromCurrentNamespace == false;
        },

        isScoringHistoryEmpty() {
            return !this.scoringsHistory || this.scoringsHistory.length == 0;
        },

        isSectionSelected() {
            const currentUniqueSectionCode = this.searchParams.uniqueSectionCode;
            return currentUniqueSectionCode != undefined && currentUniqueSectionCode != null;
        },

        isUndoEnabled() {
            return !this.isScoringHistoryEmpty;
        },

        scoringsByCategory() {
            console.log('scoringsByCategory()');

            if (!this.scorings) {
                console.log('No scorings. Returning empty array');
                return [];
            }

            if (!this.searchParams) {
                console.log('No searchParams. Returning scorings');
                return this.scorings;
            }

            if (!this.searchParams.category) {
                console.log('No filter.category. Returning scorings');
                return this.scorings;
            }

            const category = this.searchParams.category.trim().toLowerCase();

            if (category == 'all') {
                console.log('Category is all. Returning scorings');
                return this.scorings;
            }

            const containsCategory = !this.categoriesFromScorings.some(cat => cat == category);

            if (!containsCategory) {
                console.log(`Category ${category} is not within current categories. Returning scorings`);
                this.categoriesFromScorings = this.getCategoriesFromScorings();
                return this.scorings;
            }

            console.log(`Category is ${category}. Returning filtered scorings`)

            let scoringsSnapshot = this.getScoringsSnapshot(); // avoid modifying original arraay
            
            let filtered = scoringsSnapshot.map(scoring => {
                scoring.associations = scoring.associations.filter(association => {
                    let categoryMatches = association.assessment.categoryDescriptor.trim().toLowerCase().includes(category);
                    return categoryMatches;
                });
                return scoring;
            });
            
            return filtered;
        },
    }, // computed

    mounted() {

    }, // mounted

    beforeRouteLeave(to, from, next) {
        if (this.isScoringHistoryEmpty) {
            next();
        }
        else {
            this.$refs.yesNoDialog.open({ title: 'Unsaved Changes', message: 'You have unsaved changes. Are you sure you want to leave this page without saving?' })
            .then(() => next())
            .catch(() => {});
        }
    }, // beforeRouteLeave

    methods: {
        ...mapActions({
            cacheScoringAssessments: 'cache/cacheScoringAssessments',
            cacheScoringAssessmentsAll: 'cache/cacheScoringAssessmentsAll',
            createAssociation: 'scoringAssessments/createAssociation',
            deleteStudentAssessment: 'scoringAssessments/deleteStudentAssessment',
            deleteStudentAssessmentAll: 'scoringAssessments/deleteStudentAssessmentAll',
            searchScoringAssessments: 'scoringAssessments/search',
            updateScorings: 'scoringAssessments/updateScorings',
        }),

        addScoring({ selectedScoring, selectedAssessment }) {
            const body = {
                studentUniqueId: selectedScoring.student.studentUniqueId,
                assessmentId: selectedAssessment.assessment.id,
                administrationDate: dates.methods.toLocalShort(selectedAssessment.administrationDate.toString()),
            };

            this.$refs.loadingDialog.open({ title: 'Adding assessment' })

            this.createAssociation(body)
            .then(result => {
                this.$snotify.success('Assessment added!', '', {timeout: 2000});
                this.$refs.loadingDialog.close();
                
                let createdAssociation = result.data;

                selectedScoring.associations.push({
                    associationModel: createdAssociation,
                    assessment: selectedAssessment.assessment,
                    isInputDirty: true,
                    isInputDisabled: true,
                });

                this.saveConfirm();
            })
            .catch(error => {
                this.$snotify.error(`Could not add assessment: ${error}`, '', { timeout: 2000 });
                this.$refs.loadingDialog.close();
                console.error('Could not create association:', error);
            });
        },

        addScoringAll({ selectedScoring, selectedAssessment }) {
            const body = {
                studentUniqueId: selectedScoring.student.studentUniqueId,
                assessmentId: selectedAssessment.assessment.id,
                administrationDate: dates.methods.toLocalShort(selectedAssessment.administrationDate.toString()),
            };

            return this.createAssociation(body)
            .then(result => {
                let createdAssociation = result.data;
                selectedScoring.associations.push({
                    associationModel: createdAssociation,
                    assessment: selectedAssessment.assessment,
                    isInputDisabled: true,
                });
            });
        },

        getCategoriesFromScorings() {
            let categoryNames = [ 'All' ];
            
            this.scorings.forEach(scoring => {
                scoring.associations.forEach(association => {
                    let category = association.assessment.categoryDescriptor;
                    categoryNames.push(category);
                });
            });

            let uniqueCategoryNames = [ ...new Set(categoryNames) ];

            return uniqueCategoryNames;
        },

        /**
         * Creates a snapshot of the current scoring items.
         * This is useful because we can get a copy without modifying the original scorings.
         */
        getScoringsSnapshot() {
            if (!this.scorings)
                return [];

            return JSON.parse(JSON.stringify(this.scorings));
        },

        getScoringsRequest() {
            this.isShowingSkeleton = true;
            this.isShowingTable = false;
            this.isLoadingScorings = false;
            this.columns = 5;
            this.rows = 8;
            this.scorings = [];
            this.scoringsHistory = [];

            this.searchScoringAssessments(this.searchParams)
            .then(result => {
                this.scorings = result.data;
                this.categoriesFromScorings = this.getCategoriesFromScorings();
            })
            .catch(error => {
                console.error('Could not get scorings:', error);
            })
            .finally(() => {
                this.isShowingSkeleton = false;
                this.isShowingTable = true;
                this.isLoadingScorings = false;
            });
        },

        onAddAssessmentClicked(selectedRow) {
            let excludeAssessments = selectedRow.assessmentIds;

            this.$refs.addScoringAssessmentDialog.open({ excludeAssessments })
            .then(selectedAssessment => {
                this.addScoring({ selectedScoring: selectedRow.item, selectedAssessment });
            })
            .catch(error => {});
        },

        onAddAssessmentAllClicked() {
            this.$refs.addScoringAssessmentDialog.open({ excludeAssessments: [] })
            .then(result => { // result: { assessment, administrationDate }
                console.log('Result:', result);
                this.handleAddAssessmentAllRequest(result);
            })
            .catch(error => {});
        },

        onDeleteAssessmentClicked(selectedRow) {
            let studentAssessment = selectedRow.item;

            let excludeAssessments = studentAssessment.associations;
            console.log("Estudent check association: ", excludeAssessments);

            this.$refs.assessmentListDialog.open({ excludeAssessments })
            .then(association => {
                console.log("Association result: ", association);

                let associationId = association.association.associationModel.id;

                let code = this.searchParams.uniqueSectionCode;

                let identifier = association.association.associationModel.identifier;

                let studentId = studentAssessment.student.id;

                console.log("Student Id: ", associationId);
                console.log("Student Id: ", identifier);
                console.log("Student Id: ", studentId);

                this.scorings.forEach(element =>{
                    if(element.student.id == studentId){
                        console.log("Entro al if");
                        element.associations = element.associations.filter(association => association.associationModel.identifier != identifier);
                    }
                });

                this.$refs.loadingDialog.open({ title: 'Deleting Assessment(s)' });
               
                this.deleteStudentAssessment({studentAssessmentId: associationId, uniqueSectionCode: code, identifier: identifier})
                .then(() => {
                    this.$snotify.success('Assessment(s) Deleted', '', { timeout: 2000 });
                })
                .catch(error => this.$snotify.error(`Could not delete Assessment(s). ${error}`, '', { timeout: 4000 }))
                .finally(() => this.$refs.loadingDialog.close());
            })
            .catch(error => {
                console.log("Error: ", error);
            });
        },

        onDeleteAssessmentAllClicked() {
            let excludeAssessments = [];
            let lon = 0;

            console.log("Scoring: ", this.scorings);

            this.scorings.forEach(element =>{
                if(element.associations.length > lon){
                    excludeAssessments = element.associations;
                    lon = element.associations.length;
                }
            });

            this.$refs.assessmentListDialog.open({ excludeAssessments })
            .then(association => {
                console.log("Result ALL: ", association);

                let assessmentId = association.association.assessment.id;
                let date = association.association.associationModel.administrationDate;

                this.scorings.forEach(element =>{
                    element.associations.forEach(associ => {
                        if(associ.assessment.id == assessmentId && associ.associationModel.administrationDate == date)
                            element.associations = element.associations.filter(association => association.associationModel.identifier != associ.associationModel.identifier);
                    });
                });

                this.$refs.loadingDialog.open({ title: 'Deleting Assessment(s)' });
               
                this.deleteStudentAssessmentAll({uniqueSectionCode: this.searchParams.uniqueSectionCode, assessmentId: assessmentId, date: date})
                .then(() => {
                    this.$snotify.success('Assessment(s) Deleted', '', { timeout: 2000 });
                })
                .catch(error => this.$snotify.error(`Could not delete Assessment(s). ${error}`, '', { timeout: 4000 }))
                .finally(() => this.$refs.loadingDialog.close());
            })
            .catch(error => {
                console.log("Error: ", error);
            });
        },

        handleAddAssessmentAllRequest(selectedAssessment) {
            this.$refs.loadingDialog.open({ title: 'Adding Assessment(s)' });

            let promises = this.scorings.map(scoring => this.addScoringAll({ selectedScoring: scoring, selectedAssessment }));

            Promise.all(promises)
            .then(() => {
                this.$snotify.success('Assessment(s) Added', '', { timeout: 2000 });
                this.$refs.loadingDialog.close();
                this.saveConfirm();
            })
            .catch(error => {
                this.$snotify.error(`Could not add assessments: ${error}`, '', { timeout: 2000 });
                this.$refs.loadingDialog.close()
                this.retryAddAssessmentAll(selectedAssessment);
            });
        },

        retryAddAssessmentAll(selectedAssessment) {
            this.$refs.yesNoDialog.open({ title: 'Operation failed', message: 'An error ocurred when trying to add the assessments. Do you want to try again?' })
            .then(() => this.handleAddAssessmentAllRequest(selectedAssessment))
            .catch(error => {});
        },

        onRefreshClicked() {
            if (this.isScoringHistoryEmpty) {
                this.getScoringsRequest();
                return;
            }

            this.$refs.yesNoDialog.open({
                title: "Refresh",
                message: "There are unsaved changes. Are your sure you want to refresh?"
            })
            .then(() => this.getScoringsRequest())
            .catch(() => {});
        },

        onExportClicked() {
            let parsed = csv.methods.parseScoringAssessments(this.scorings);
            csv.methods.export(this.scorings);
        },

        onStaffSelected(selectedStaff) {
            this.isShowingLoadingMessage = false;
            
            this.columns = 5;
            this.rows = 8;
            this.scorings = [];
            this.isLoadingScorings = true;
            this.isShowingTable = false;
            this.isShowingSkeleton = true;
        },

        onSectionSelected(selectedSection) {
            this.searchParams.uniqueSectionCode = selectedSection.id;
            this.searchParams.storeInCache = true
            this.getScoringsRequest();
        },

        onCategorySelected(selectedCategory) {
            console.log('onCategorySelected()', selectedCategory);
            this.searchParams.category = selectedCategory;
        },

        onSaveClicked() {
            this.$refs.yesNoDialog.open({ title: 'Save Changes', message: 'Do you want to save these changes? You won\'t be able to undo them.' })
            .then(() => this.saveConfirm())
            .catch(error => {});
        },

        saveConfirm() {
            let body = {
                searchParams: this.searchParams,
                scorings: this.scorings,
            };

            this.$refs.loadingDialog.open({ title: 'Saving changes' });

            Promise.all([
                this.updateScorings(this.scorings),
                this.cacheScoringAssessmentsAll(body),
            ])
            .then(() => {
                this.$snotify.success('Changes have been saved!', '', { timeout: 2000 });
                this.resetScoringInputsStatusAndHistory();
                this.categoriesFromScorings = this.getCategoriesFromScorings();
            })
            .catch(error => {
                console.error('Could not save changes :(');
                this.$snotify.error(`Could not save changes: ${error}`, '', { timeout: 4000 });
            })
            .finally(() => this.$refs.loadingDialog.close());
        },

        onDataSetChanged(scorings) {
            if (this.isScoringHistoryEmpty) {
                let snapshot = this.getScoringsSnapshot();
                this.scoringsHistory.push(snapshot);
            }
            
            this.scoringsHistory.push(scorings);
        },

        onNamespaceClicked(value /* boolean */) {
            this.searchParams.onlyFromCurrentNamespace = value;
            this.getScoringsRequest();
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

        resetScoringInputsStatusAndHistory() {
            this.scorings = this.scorings.map(studentAssessments => {
                studentAssessments.associations = studentAssessments.associations.map(association => {
                    association.isInputDisabled = true;
                    association.isInputDirty = false;
                    return association;
                });
                return studentAssessments;
            });

            this.scoringsHistory = [];
        },
    }, // methods

    watch: {
        currentSchoolId() {
            console.log("currentSchoolId()", this.currentSchoolId);
            this.scorings = [];
            this.scoringsHistory = [];
            this.categoriesFromScorings = [ 'All' ];
            this.associationsToDelete = [];
            this.associationsToDeleteAll = [];
            this.isLoadingScorings = false;
            this.isShowingTable = false;
            this.isShowingSkeleton = false;
            this.isShowingLoadingMessage = true;
            this.isShowingErrorMessage = false;
            this.searchParams = {
                staffUniqueId: undefined, // string
                uniqueSectionCode: undefined, // string
                category: undefined, // any
                showInactiveStudents: undefined, // bool
                getFromCache: true,
                storeInCache: false,
                onlyFromCurrentNamespace: true,
            };
        },
    }, // watch

    components: {
        AddScoringAssessmentDialog,
        LoadingDialog,
        ScoringAssessmentsFilters,
        ScoringAssessmentsTable,
        ScoringAssessmentsToolbar,
        TableSkeletonLoader,
        YesNoDialog,
        AssessmentListDialog,
        AppToolbar,
    }, // components

    mixins: [ csv, dates ],
}
</script>
