<template>
  <v-dialog
    v-model="isActive"
    fullscreen
    persistent>
    <v-card
      style="
            width: 100%;
            height: 100%;
            background: rgb(255, 255, 255);
            background: linear-gradient(0deg, rgba(255, 255, 255, 1) 0%, rgba(226, 237, 255, 1) 100%);
            ">
      <div
        class="d-flex justify-center align-center"
        style="height: 100%">
        <div>
          <h3 class="text-center">
            Please wait. We're getting things ready
          </h3>
          <h5 class="text-center font-weight-regular mb-8">
            This might take a while
          </h5>

          <h4 class="text-center font-weight-regular mb-1">
            {{ loadingMessage }}
          </h4>
          <div class="app-center-x">
            <v-progress-linear indeterminate />
          </div>
        </div>
      </div>
    </v-card>
  </v-dialog>
</template>

<script>
/* eslint-disable */

import { mapActions, mapGetters, mapMutations } from 'vuex';

export default {
    
    name: 'CacheLoader',

    data: () => ({
        isActive: false,
        isLoadingCache: false,
        loadingMessage: 'Loading...',
        maxRecordCount: 150,
        resolve: null,
        reject: null,
    }), // data

    methods: {
        ...mapActions({
            getCache: 'cache/getCache',
            getSchools: 'cache/getSchools',
            getStaffs: 'cache/getStaffs',
            getSections: 'cache/getSections',
            getStudents: 'cache/getStudents',
            getCategories: 'cache/getCategories',
        }),

        ...mapMutations({
            setSchools: 'cache/setSchools',
            setStaffs: 'cache/setStaffs',
            setSections: 'cache/setSections',
            setCategories: 'cache/setCategories',
            setStudents: 'cache/setStudents',
            setIsCacheInitialized: 'cache/setIsCacheInitialized',
        }),

        async start() {
            this.isActive = true;

            await this.loadCache();

            return new Promise((resolve, reject) => {
                this.resolve = resolve;
                this.reject = reject;
            });
        },

        async loadCache() {
            this.isLoadingCache = true;
            let debug = false;

            try {
                if (debug) console.log('Getting schools...');
                this.loadingMessage = 'Getting schools...';
                let schools = (await this.getSchools()).data;
                if (debug) console.log('Got schools!', schools);
                this.setSchools(schools);
                if (debug) console.log('');

                let staffsAll = {};
                for (let school of schools) {
                    if (debug) console.log('Getting staffs...', school.id);
                    this.loadingMessage = 'Getting staffs...';
                    let staffs = (await this.getStaffs(school.id)).data;
                    staffsAll[school.id] = staffs;
                    if (debug) console.log('Got staffs!', staffs);
                }
                this.setStaffs(staffsAll);
                if (debug) console.log('');

                let sectionsAll = {};
                let sectionsCount = 0;
                sectionsLoop1: for (let schoolId of Object.keys(staffsAll)) {
                    for (let staff of staffsAll[schoolId]) {
                        if (debug) console.log('Getting sections...', staff.id);
                        this.loadingMessage = 'Getting sections...';
                        let sections = (await this.getSections(staff.id)).data;
                        sectionsAll[staff.id] = sections;
                        if (debug) console.log('Got sections!', sections);

                        sectionsCount += sections.length;
                        if (sectionsCount > this.maxRecordCount)
                            break sectionsLoop1;
                    }
                }
                this.setSections(sectionsAll);
                if (debug) console.log('');

                let studentsAll = {};
                let studentsCount = 0;
                studentsLoop1: for (let staffId of Object.keys(sectionsAll)) {
                    for (let section of sectionsAll[staffId]) {
                        if (debug) console.log('Getting students...', section.id);
                        this.loadingMessage = 'Getting students...';
                        let students = (await this.getStudents(section.id)).data;
                        studentsAll[section.id] = students;
                        if (debug) console.log('Got students!', students);

                        studentsCount += students.length;
                        if (studentsCount > this.maxRecordCount)
                            break studentsLoop1;
                    }
                }
                this.setStudents(studentsAll);
                if (debug) console.log('');

                if (debug) console.log('Getting categories...');
                this.loadingMessage = 'Getting categories...';
                let categories = (await this.getCategories()).data;
                if (debug) console.log('Got categories!', categories);
                this.setCategories(categories);
                if (debug) console.log('');

                if (debug) console.log('Cache loaded!');
                this.setIsCacheInitialized(true);
                this.isActive = false;
            }
            catch (error) {
                console.error('Could not load cache:', error);
            }
            finally {
                this.isLoadingCache = false;
            }
        },
    }, // methods
}
</script>
