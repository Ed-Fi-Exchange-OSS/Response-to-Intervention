<template>
  <v-expansion-panels
    multiple
    focusable
    :value="[ 0 ]">
    <v-expansion-panel>
      <v-expansion-panel-header>Filters</v-expansion-panel-header>
      <v-expansion-panel-content>
        <!-- school -->
        <div class="d-flex align-center mb-3 mt-4">
          <p class="ma-0 mr-3 pa-0">
            Selected School:
          </p>
          <OrganizationDropdown></OrganizationDropdown>
        </div><!-- school -->

        <v-row>
          <v-col
            cols="12"
            sm="6"
            md="3">

            <v-select v-if="isRoleAdmin"
              :color="darkMode ? 'accent' : 'primary'"
              :item-color="darkMode ? 'accent' : 'primary'"
              v-model="selectedStaffId"
              label="Teacher"
              hide-details
              :disabled="isDisabledStaffs"
              :loading="isLoadingStaffs"
              :items="staffs"
              item-value="id"
              item-text="fullName"
              @change="onStaffSelected">
            </v-select>

            <v-text-field v-if="isRoleTeacher"
              v-model="currentStaff.fullName"
              label="Teacher"
              hide-details
              disabled
              :loading="isLoadingStaffs" />
          </v-col>

          <v-col
            cols="12"
            sm="6"
            md="3">
            <v-select
              :color="darkMode ? 'accent' : 'primary'"
              :item-color="darkMode ? 'accent' : 'primary'"
              v-model="selectedSectionId"
              label="Section"
              hide-details
              :disabled="isDisabledSections"
              :loading="isLoadingSections"
              :items="sections"
              item-value="id"
              @change="onSectionSelected">
              <template v-slot:selection="{ item }">
                {{ item.classPeriodReference.name }} | {{ item.courseOfferingReference.localCourseCode }} | {{ item.courseOfferingReference.schoolYear }} | {{ item.courseOfferingReference.termDescriptor }}
              </template>
              <template v-slot:item="{ item }">
                {{ item.classPeriodReference.name }} | {{ item.courseOfferingReference.localCourseCode }} | {{ item.courseOfferingReference.schoolYear }} | {{ item.courseOfferingReference.termDescriptor }}
              </template>
            </v-select>
          </v-col>

          <v-col cols="12" sm="6" md="3">
            <v-select
              :color="darkMode ? 'accent' : 'primary'"
              :item-color="darkMode ? 'accent' : 'primary'"
              v-model="selectedCategory"
              label="Category"
              hide-details
              :placeholder="isLoadingCategories ? 'Loading categories...' : 'Select a category'"
              :disabled="isCategoriesDisabled"
              :loading="isLoadingCategories"
              :items="categories"
              @change="onCategorySelected" />
          </v-col>

          <v-col
            cols="12"
            sm="6"
            md="3">
            <v-checkbox
              :color="darkMode ? 'accent' : 'primary'"
              :item-color="darkMode ? 'accent' : 'primary'"
              v-model="showInactiveStudents"
              label="Show inactive students"
              hide-details
              @change="onShowInactiveStudentsSwitched" />
          </v-col>
        </v-row>
      </v-expansion-panel-content>
    </v-expansion-panel>
  </v-expansion-panels>
</template>

<script>
/* eslint-disable */

import { mapActions, mapState, mapGetters } from 'vuex';
import OrganizationDropdown from '../../../components/widgets/OrganizationDropdown';
import storage from '@/storage';

export default {

    props: {
        categories: Array,
        isCategoriesDisabled: Boolean,
    },

    data: () => ({
        staffs: [],
        selectedStaffId: 0,
        isDisabledStaffs: true,
        isLoadingStaffs: false,

        sections: [],
        selectedSectionId: 0,
        isDisabledSections: true,
        isLoadingSections: false,

        selectedCategory: 'All',
        isLoadingCategories: false,

        showInactiveStudents: false,
    }), // data

    computed: {
        ...mapState(["darkMode"]),
        ...mapGetters({
            currentSchoolId: 'catalog/getCurrentSchoolId',
            staffsCache: 'cache/getStaffs',
            sectionsCache: 'cache/getSections'
        }),
        
        currentStaff() {
          return storage.local.getStaff();
        },

        isRoleAdmin() {
          return storage.local.isUserRoleAdmin()
        },

        isRoleTeacher() {
          return storage.local.isUserRoleTeacher()
        },

        isDarkMode() {
          return this.darkMode == true;
        },
    }, // computed

    mounted() {
        if (this.currentSchoolId && !this.isLoadingStaffs) {
            this.getStaffsRequest();
        }
    }, // mounted

    methods: {
        ...mapActions({
            getStaffsBySchool: 'catalog/getStaffsBySchool',
            getSectionsByStaff: 'catalog/getSectionsByStaff',
        }),

        getStaffsRequest() {
            this.isDisabledStaffs = true;
            this.isLoadingStaffs = true;

            if (storage.local.isUserRoleTeacher()) {
                this.isDisabledStaffs = false;
                this.isLoadingStaffs = false;
                this.selectedStaffId = this.currentStaff.id;
                this.onStaffSelected();
                return;
            }

            this.getStaffsBySchool(this.currentSchoolId).then(result => {
                this.staffs = result.data;
                this.isDisabledStaffs = false;

                if (this.staffs && this.staffs.length > 0) {
                    this.selectedStaffId = null;
                    this.staffs = [
                        {
                            id: null,
                            fullName: 'Select a teacher',
                        },
                        ...this.staffs,
                    ];
                }
            })
            .catch(() => {})
            .finally(() => this.isLoadingStaffs = false);
        },

        getSectionsByStaffRequest() {
            this.isDisabledSections = true;
            this.isLoadingSections = true;

            return this.getSectionsByStaff(this.selectedStaffId)
            .then(result => {
                this.isDisabledSections = false;
                this.sections = result.data;

                if (this.sections && this.sections.length > 0) {
                    this.selectedSectionId = this.sections[0].id;
                    this.onSectionSelected();
                }
            })
            .finally(() => this.isLoadingSections = false);
        },

        onStaffSelected() {
            if (this.isRoleTeacher) {
              console.log("onStaffSelected Teacher", this.currentStaff);
                this.$emit('staff-selected', this.currentStaff);
            }
            else if (this.isRoleAdmin) {
              let selectedStaff = this.staffs.find(staff => staff.id == this.selectedStaffId);
              console.log("onStaffSelected Admin", selectedStaff);
                this.$emit('staff-selected', selectedStaff);
            }

            this.getSectionsByStaffRequest();
        },

        onSectionSelected() {
            let selectedSection = this.sections.find(section => section.id == this.selectedSectionId);
            this.$emit('section-selected', selectedSection);
        },

        onCategorySelected() {
            let selectedCategory = this.selectedCategory == 'All' ? null : this.selectedCategory;
            this.$emit('category-selected', selectedCategory);
        },

        onShowInactiveStudentsSwitched() {
            this.$emit('show-inactive-students-switched', this.showInactiveStudents);
        },
    }, // methods

    watch: {
        currentSchoolId() {
          this.staffs = [];
          this.selectedStaffId = 0;
          this.isDisabledStaffs = true;
          this.isLoadingStaffs = false;
          this.sections = [];
          this.selectedSectionId = 0;
          this.isDisabledSections = true;
          this.isLoadingSections = false;
          this.selectedCategory = 'All';
          this.isLoadingCategories = false;
          this.showInactiveStudents = false;

          if (!this.isLoadingStaffs) {
              this.getStaffsRequest();
          }
        },
    }, // watch

    components: {
        OrganizationDropdown,
    },
}
</script>
