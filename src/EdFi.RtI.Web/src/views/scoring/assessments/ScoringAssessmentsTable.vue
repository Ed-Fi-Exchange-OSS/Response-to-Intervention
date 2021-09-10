<template>
    <div>
        <v-card>
            <v-data-table
                fixed-header
                height="800"
                :headers="headers"
                :items="items"
                :options.sync="listViewOptions"
                :server-items-length="items.length"
                :hide-default-footer="true">
                
                <template v-slot:no-data>
                    <div></div>
                </template>

                <!-- add/delete scoring -->
                <template v-slot:item.addAssessment="{ item }">
                    <v-btn small icon :disabled="isLoading" @click="onAddAssessmentClicked(item)">
                        <v-icon>mdi-plus</v-icon>
                    </v-btn>
                    <v-btn small icon :disabled="isLoading" @click="onDeleteAssessmentClicked(item)">
                        <v-icon>mdi-delete</v-icon>
                    </v-btn>
                </template><!-- add/delete scoring -->

                <template v-slot:item.studentName="{ item }">
                    <v-chip small color="primary" @click="viewStudent(item)">
                        {{item.student.fullName}}
                    </v-chip>
                </template>

                <template v-slot:item.totalAssessments="{ item }">
                    <div style="width: 100%">
                        <p class="ma-0 text-center">
                            {{getAssessmentCount(item)}}
                        </p>
                    </div>
                </template>

                <!-- dynamic header slots (for assessment tile and date) -->
                <template v-for="slot in dynamicSlots" v-slot:[slot.idHeader]="{ }">
                    <div :key="slot.idHeader" style="min-width: 80px;">
                        <p class="text-center ma-0">{{slot.assessment.title}}</p>
                        <p class="text-center ma-0 font-weight-light" style="color: grey">{{getDateShort(slot.associationModel.administrationDate)}}</p>
                    </div>
                </template><!-- dynamic header slots (for assessment tile and date) -->

                <!-- dynamic item slots (for score results) -->
                <template v-for="slot in dynamicSlots" v-slot:[slot.idItem]="{ item }">
                    <div :key="slot.idItem" class="d-flex justify-center py-2">
                        <div v-if="isSlotAvailableForItem(slot, item)"
                            style="max-width: 65px; min-width: 60px;"> 
                            <v-text-field outlined dense hide-details
                                v-if="!readonly"
                                :class="{
                                    'input-disabled': isInputDisabled(slot, item),
                                    'input-dirty': isInputDirty(slot, item),
                                }"
                                :disabled="isLoading"
                                :readonly="isInputDisabled(slot, item) || readonly"
                                :ref="getItemSlotIndicesAsKey(slot, item)"
                                :value="getScoreResult(slot, item)"
                                @click="onClickedInput($event, slot, item)"
                                @change="scoreChanged($event, slot, item)"
                                @focus="$event.target.select()"
                                @keydown="onKeyDown($event, slot, item)"/>

                            <p class="ma-0 text-center" v-if="readonly">
                                {{getScoreResult(slot, item)}}
                            </p>
                        </div>
                    </div>
                </template><!-- dynamic item slots (for score results) -->

            </v-data-table>
        </v-card>

        <!-- view interventions side menu -->
        <v-navigation-drawer v-model="isShowingInterventionsSideMenu" fixed right temporary>
            <v-app-bar dense>
                <v-btn small icon @click="isShowingInterventionsSideMenu = false">
                    <v-icon>mdi-close</v-icon>
                </v-btn>

                <v-toolbar-title style="font-size: 16px">Intervention Details</v-toolbar-title>
            </v-app-bar>

            <v-list>
                <!-- name -->
                <v-list-item>
                    <v-list-item-avatar>
                        <v-icon>mdi-account</v-icon>
                    </v-list-item-avatar>
                    <v-list-item-content>
                        <v-list-item-title>Name</v-list-item-title>
                        <v-list-item-subtitle>John Doe</v-list-item-subtitle>
                    </v-list-item-content>
                </v-list-item><!-- name -->

                <v-divider></v-divider>

                <!-- programs -->
                <v-list-item>
                    <v-list-item-avatar>
                        <v-icon>mdi-puzzle</v-icon>
                    </v-list-item-avatar>
                    <v-list-item-content>
                        <v-list-item-title>Programs</v-list-item-title>
                        <v-list-item-subtitle>ENL, SPED, ES</v-list-item-subtitle>
                    </v-list-item-content>
                </v-list-item><!-- programs -->

                <v-divider></v-divider>

                <!-- interventions -->
                <div>
                    <v-list-item>
                        <v-list-item-avatar>
                            <v-chip small>2</v-chip>
                        </v-list-item-avatar>
                        <v-list-item-content>
                            <v-list-item-title>Interventions </v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>

                    <v-list-item>
                        <v-list-item-content>
                            <v-list-item-title>Lexia</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>

                    <v-list-item>
                        <v-list-item-content>
                            <v-list-item-title>Waterford</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>

                </div><!-- interventions -->

            </v-list>

        </v-navigation-drawer><!-- view interventions side menu -->

        <!-- dialogs -->
        <div>
            <AssessmentDetailsDialog ref="assessmentDetailsDialog"/>
            <StudentProfileDialog ref="studentProfileDialog"/>
        </div>
        <!-- dialogs -->
    </div>
</template>

<style scoped>

.input-disabled {
    background-color: rgb(255, 255, 255);
}

.input-dirty {
    background-color: rgba(255, 192, 0, 0.4);
}

.v-input--is-focused {
    box-shadow: 2px 2px 5px 0px rgba(50, 50, 50, 0.75);
}

/*.v-text-field--outlined >>> fieldset {
    border-color: rgb(200, 200, 200);
}*/

</style>

<script>
/* eslint-disable */

import AssessmentDetailsDialog from '@/components/dialogs/AssessmentDetailsDialog';
import StudentProfileDialog from '@/components/dialogs/StudentProfileDialog';
import { KeyCodes } from '../../../models/constants/KeyCodes';
import { dates } from '../../../mixins/mixins';
import storage from '@/storage';

export default {

    props: {
        scorings: Array,
        isLoading: Boolean,
        searchParams: Object,

        readonly: {
            type: Boolean,
            default: false,
        },
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
            const headers = [];

            if (this.readonly == false) {
                headers.push({
                    align: 'center',
                    sortable: false,
                    text: 'Add/Remove Assessment',
                    value: 'addAssessment',
                });
            }

            headers.push({
                sortable: false,
                text: 'Student Name',
                value: 'studentName',
            });

            this.dynamicSlots.forEach(slot => {
                
                /**
                 * The header "value" fields needs to be the unique slot id for the items
                 * in otder for the headers and the items to match.
                 * 
                 * In the html template, the slot declaration would be "header.<slotId>"
                 * and this declaration needs the header value to match the <slotId>,
                 * otherwise, the headers won't show
                 */
                let uniqueSlotId = slot.id;

                headers.push({
                    align: 'center',
                    sortable: false,
                    value: uniqueSlotId,
                });
            });

            return headers;
        },

        items() {
            return this.scorings;
        },

        dynamicSlots() {
            let associationSlots = [];

            this.scorings.forEach(currentScoring => {
                currentScoring.associations.forEach(association => {
                    let uniqueSlotId = association.assessment.id + '.' + association.associationModel.administrationDate; // The unique id that will be used in both header and item slots

                    let associationSlot = {
                        id: uniqueSlotId, // slot name needs to be header.fieldName in order for dynamic header slot to work
                        idHeader: `header.${uniqueSlotId}`, // slot name needs to be header.fieldName in order for dynamic header slot to work
                        idItem: `item.${uniqueSlotId}`, // slot name needs to be item.fieldName in order for dynamic item slot to work
                        ...association,
                    };
                    associationSlots.push(associationSlot);
                });
            });

            let associationSlotIds = associationSlots.map(slot => slot.id);
            let uniqueAssociationSlotIds = new Set(associationSlotIds);
            let includedSlots = new Set();

            let uniqueAssociationSlots = associationSlots.filter(slot => {
                let shouldIncludeSlot = uniqueAssociationSlotIds.has(slot.id) && !includedSlots.has(slot.id);

                if (!includedSlots.has(slot.id))
                    includedSlots.add(slot.id);
                
                return shouldIncludeSlot;
            });

            return uniqueAssociationSlots;
        },
    }, // computed

    mounted() {

    }, // mounted

    methods: {
        getDateShort(dateStr) {
            return dates.methods.toLocalShort(dateStr);
        },

        emitCurrentDataSetChange() {
            let dataSetCopy = JSON.parse(JSON.stringify(this.scorings));
            this.$emit('data-set-changed', dataSetCopy);
        },

        getAssessmentCount(item) {
            return item.associations.length;
        },

        /**
         * Each dynamic slot in the v-data-table must correspond to one of the associations
         * contained in a scoring item. If none of the associations of the provided item
         * has an assessment with an id matching the slot id, or slot assessment, then there
         * is no relation between the student and the assessment, therefore, this slot for
         * this item (or student) should be displayed as empty in the table.
         */
        getAssociationFromSlot(slot, item) {
            let uniqueSlotId = slot.id

            let association = item.associations.find(association => {
                let uniqueSlotItemId = association.assessment.id + '.' + association.associationModel.administrationDate;
                return uniqueSlotId == uniqueSlotItemId;
            });

            return association;
        },

        /**
         * Gets the StudentAssessment association in the (x, y) position in the grid,
         * being "y" the student index and being "x" the slot index
         */
        getAssociationFromIndices(x, y) {
            console.log("getAssociationFromIndices", x, y);
            let studentAssessmentAssociations = this.scorings[y];

            console.log("studentAssessmentAssociations:", studentAssessmentAssociations);

            if (!studentAssessmentAssociations) {
                console.log("return null");
                return null;
            }

            let association = studentAssessmentAssociations.associations[x];

            console.log("studentAssessmentAssociations.associations:", studentAssessmentAssociations.associations);
            console.log("association found!:", association);
            console.log("");

            return association;
        },

        /**
         * Gets the (x, y) positions in the grid of the student's assessment association,
         * "x" being the slot index and "y" being the student index
         */
        getItemSlotIndices(slot, item) {
            let slotIndex = this.dynamicSlots.findIndex(currentSlot => currentSlot.id == slot.id);
            let studentIndex = this.scorings.findIndex(currentItem => currentItem.student.id == item.student.id);
            return { x: slotIndex, y: studentIndex };
        },

        /**
         * Each individual input in the grid needs a unique id to be able to set
         * focus. The key to these inputs is their (x, y) coordindates, since they cannot be repeated
         */
        getItemSlotIndicesAsKey(slot, item) {
            let indices = this.getItemSlotIndices(slot, item);
            return `${indices.x}-${indices.y}`;
        },

        /**
         * Gets the score result from the current item to display into the slot
         */
        getScoreResult(slot, item) {
            let association = this.getAssociationFromSlot(slot, item);
            return association.associationModel.scoreResults[0].result;
        },

        /**
         * Each individual texbox corresponds to an item's association model.
         * This method returns the isInputDisabled flag for that specific association
         */
        isInputDisabled(slot, item) {
            let association = this.getAssociationFromSlot(slot, item);

            if (association == undefined || association == null)
                console.log("isInputDisabled:", association);
            
            return association.isInputDisabled == true;
        },

        /**
         * Each individual texbox corresponds to an item's association model.
         * This method returns the isInputDirty flag for that specific association
         */
        isInputDirty(slot, item) {
            let association = this.getAssociationFromSlot(slot, item);
            return association.isInputDirty == true;
        },

        /**
         * Not all students are related to an assessment contained inside a slot.
         * If this is the case, then the slot should be displayed as empty.
         */
        isSlotAvailableForItem(slot, item) {
            let association = this.getAssociationFromSlot(slot, item);
            return association != null;
        },

        onAddAssessmentClicked(item) {
            let studentUniqueId = item.student.studentUniqueId;
            let assessmentIds = item.associations.map(association => association.assessment.id);

            this.$emit('add-assessment', {
                item,
                studentUniqueId,
                assessmentIds: [...new Set(assessmentIds)],
            });
        },

        onDeleteAssessmentClicked(item) {
            let studentUniqueId = item.student.studentUniqueId;
            let assessmentIds = item.associations.map(association => association.assessment);

            this.$emit('delete-assessment', {
                item,
                studentUniqueId,
                assessmentIds: [...new Set(assessmentIds)],
            });
        },

        onClickedInput($event, slot, item) {
            this.scorings.forEach(scoring => {
                scoring.associations.forEach(association => {
                    if (association == null || association == undefined)
                        console.log("onClickedInput1", association)
                    
                    association.isInputDisabled = true;
                });
            })

            let association = this.getAssociationFromSlot(slot, item);

            if (association == null || association == undefined)
                console.log("onClickedInput2", association);
            
            association.isInputDisabled = false;
        },

        onKeyDown($event, slot, item) {
            let key = $event.key;
            
            if (key == KeyCodes.enter) {
                this.onKeyDownEnter($event, slot, item)
                return
            }

            if (key == KeyCodes.arrowDown || key == KeyCodes.arrowLeft || key == KeyCodes.arrowRight || key == KeyCodes.arrowUp) {
                this.onKeyDownArrow($event, slot, item)
                return
            }
        },

        onKeyDownEnter($event, slot, item) {
            let association = this.getAssociationFromSlot(slot, item);

            if (association == undefined || association == null)
                console.log("onKeyDownEnter", association);
            
            association.isInputDisabled = !association.isInputDisabled;

            if (association.isInputDisabled == true) {
                this.moveFocusToInput(slot, item, 0, 1);
            }
        },

        onKeyDownArrow($event, slot, item) {
            let key = $event.key;
            let x = 0;
            let y = 0;

            if (key == KeyCodes.arrowDown) {
                y = 1;
            }
            else if (key == KeyCodes.arrowLeft) {
                x = -1;
            }
            else if (key == KeyCodes.arrowRight) {
                x = 1;
            }
            else if (key == KeyCodes.arrowUp) {
                y = -1;
            }

            this.moveFocusToInput(slot, item, x, y);
        },

        moveFocusToInput(slot, item, x, y) {
            let currentInputIndices = this.getItemSlotIndices(slot, item);

            let newInputIndices = {
                x: currentInputIndices.x + x,
                y: currentInputIndices.y + y,
            };

            // clamp indices to prevent out of bounds
            if (newInputIndices.x < 0) newInputIndices.x = 0;
            if (newInputIndices.y < 0) newInputIndices.y = 0;
            if (newInputIndices.x >= this.dynamicSlots.length - 1) newInputIndices.x = this.dynamicSlots.length - 1;
            if (newInputIndices.y >= this.scorings.length - 1) newInputIndices.y = this.scorings.length - 1;
            
            this.$nextTick(() => {
                let focusOnInputKey = `${newInputIndices.x}-${newInputIndices.y}`;
                let refs = this.$refs[focusOnInputKey];

                if (refs) {
                    refs[0].focus();

                    let associationRemoveFocus = this.getAssociationFromIndices(currentInputIndices.x, currentInputIndices.y);

                    if (associationRemoveFocus == undefined || associationRemoveFocus == null)
                        console.log("associationRemoveFocus:", associationRemoveFocus);
                    
                    associationRemoveFocus.isInputDisabled = true;

                    let associationSetFocus = this.getAssociationFromIndices(newInputIndices.x, newInputIndices.y);

                    if (associationSetFocus == undefined || associationSetFocus == null)
                        console.log("associationSetFocus:", associationSetFocus);
                    
                    associationSetFocus.isInputDisabled = false;
                }
            });
        },

        scoreChanged(newValue, slot, item) {
            console.log("scoreChanged:", newValue, slot, item);
            let association = this.getAssociationFromSlot(slot, item);
            console.log("association:", association);
            
            let currentScoreResultValue = association.associationModel.scoreResults[0].result;

            if (currentScoreResultValue == newValue) {
                console.log("No need to change score.");
                console.log("");
                return;
            }

            association.associationModel.scoreResults[0].result = newValue;
            console.log("Updated score!");
            console.log("");
            association.isInputDirty = true;
            this.emitCurrentDataSetChange();
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

/**
 * @param {String} str
 * @returns {Boolean}
 */
function isNumeric(str) {
  if (typeof str != "string")
    return false

  return !isNaN(str) && !isNaN(parseFloat(str))
}
</script>
