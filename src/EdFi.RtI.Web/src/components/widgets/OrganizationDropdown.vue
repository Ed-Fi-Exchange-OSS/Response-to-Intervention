<template>
  <v-menu bottom rounded offset-y>
    <!-- activator -->
    <template v-slot:activator="{ on }">
      <v-btn color="primary" :disabled="isLoadingSchools" small v-on="on">
        <span class="font-weight-light text-capitalize">{{label}}</span>
        <v-icon small class="ml-1">mdi-chevron-down</v-icon>
      </v-btn>
    </template><!-- activator -->

    <!-- organizations -->
    <v-card>
      <v-list dense>
        <v-list-item
          v-for="(school, index) of schools"
          :key="index"
          dense
          @click="onSchoolSelected(school)">
          <v-list-item-title>
            <h4 class="font-weight-regular">
              {{ school.nameOfInstitution }}
            </h4>
          </v-list-item-title>
        </v-list-item>
      </v-list>
    </v-card><!-- organizations -->
  </v-menu>
</template>

<script>
/* eslint-disable */
import storage from '@/storage';
import {  mapGetters, mapMutations } from 'vuex';
import api from '@/api';
import { notNullNorEmpty } from '@/utils/validators';

export default {

  data: () => ({
    schools: [],
    isLoadingSchools: false,
  }), // data

  computed: {
    ...mapGetters({
      currentSchoolId: 'catalog/getCurrentSchoolId',
      currentSchool: 'catalog/getCurrentSchool',
    }),

    currentStaff() {
      return storage.local.getStaff()
    },

    label() {
      if (this.isLoadingSchools == true)
        return 'Loading Schools...'

      if (this.hasSchools) {
        if (this.isSchoolSelected)
          return this.currentSchool.nameOfInstitution

        return 'Select a School'
      }

      return this.noSchoolsMessage
    },

    hasSchools() {
      return notNullNorEmpty(this.schools)
    },

    isSchoolSelected() {
      return this.currentSchool != null;
    },

    noSchoolsMessage() {
      if (storage.local.isUserRoleAdmin())
        return "This user doesn't have any schools"

      if (storage.local.isUserRoleTeacher())
        return "You don't have any schools"

      throw `Computed property "noSchoolsMessage" has not been implemented for "${storage.local.getStaffClassificationDescriptor()}"`
    }
  }, // computed

  mounted() {
    this.init()
  }, // mounted

  methods: {
    ...mapMutations({
      setCurrentSchoolId: 'catalog/setCurrentSchoolId',
      setCurrentSchool: 'catalog/setCurrentSchool',
    }),

    async init() {
      this.isLoadingSchools = true

      if (storage.local.isUserRoleAdmin()) {
        console.log("Gettings schools for admin...");
        this.schools = await api.schools.getSchools()
      }
      else if (storage.local.isUserRoleTeacher()) {
        console.log("Gettings schools for teacher...");
        this.schools = await api.schools.getSchoolsByStaff(this.currentStaff.staffUniqueId)
      }
      else
        throw `GetSchools has not been implemented for \"${storage.local.getStaffClassificationDescriptor()}\"`

      console.log("this.schools:", this.schools)
      this.isLoadingSchools = false
      this.selectDefaultSchool()
    },

    onSchoolSelected(school) {
      this.setCurrentSchool(school);
      this.setCurrentSchoolId(school.id);
      this.$emit('on-school-selected', school);
    },

    selectDefaultSchool() {
      if (!this.schools)
        return
      
      if (this.schools.length == 0)
        return

      const defaultSchool = this.schools[0];
      this.onSchoolSelected(defaultSchool);
    },
  }, // methods
}
</script>
