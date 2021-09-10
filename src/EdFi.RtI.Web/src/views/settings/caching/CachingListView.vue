<template>
  <div>
    <v-data-table
      class="elevation-1"
      :headers="headers"
      :items="items"
      :options.sync="listViewOptions"
      :server-items-length="100"
      :footer-props="{ 'items-per-page-options': [10, 20, 30, 40, 50] }"
    >
      <template v-slot:no-data>
        <div></div>
      </template>

      <!--<template v-slot:item.endDate="{ item }">
        {{ getEndDateStr(item) }}
      </template>-->

      <template v-slot:item.route="{ item }">
        <v-btn small icon @click="showCacheTable(item.route)">
          <v-icon>mdi-eye</v-icon>
        </v-btn>
      </template>
    </v-data-table>

    <YesNoDialog ref="yesNoDialog" />
    <LoadingDialog ref="loadingDialog" />
  </div>
</template>

<script>
import { dates } from "@/mixins/mixins";
import { mapActions } from "vuex";
import YesNoDialog from "@/components/dialogs/YesNoDialog.vue";
import LoadingDialog from "@/components/dialogs/LoadingDialog.vue";
export default {
  data: () => ({
    headers: [
      {
        text: "Cache Name",
        value: "name",
        sortable: true,
      },
      {
        text: "Last Updated",
        value: "lastUpdated",
        sortable: true,
      },
      {
        sortable: false,
        value: "route",
      },
      {
        sortable: false,
        value: "refreshAction",
      },
    ],

    items: [
      {
        id: "assessments",
        name: "Assessments",
        lastUpdated: "Loading...",
        route: "/assesments",
        click: () => {},
      },
      {
        id: "interventions",
        name: "Interventions",
        lastUpdated: "Loading...",
        route: "/interventions",
        click: () => {},
      },
      {
        id: "schools",
        name: "Schools",
        lastUpdated: "Loading...",
        route: "/settings/caching/schools",
        click: () => {},
      },
      {
        id: "sections",
        name: "Sections",
        lastUpdated: "Loading...",
        route: "/settings/caching/sections",
        click: () => {},
      },
      {
        id: "sectionsByStaff",
        name: "Sections by Staff",
        lastUpdated: "Loading...",
        route: "/settings/caching/sections-by-staff",
        click: () => {},
      },
      {
        id: "staffs",
        name: "Staffs",
        lastUpdated: "Loading...",
        route: "/settings/caching/teachers",
        click: () => {},
      },
      {
        id: "staffsBySchool",
        name: "Staffs by School",
        lastUpdated: "Loading...",
        route: "/settings/caching/staffs-by-school",
        click: () => {},
      },
      {
        id: "students",
        name: "Students",
        lastUpdated: "Loading...",
        route: "/settings/caching/students",
        click: () => {},
      },
      {
        id: "studentsBySection",
        name: "Students by Section",
        lastUpdated: "----------",
        route: "/settings/caching/students-by-section",
        click: () => {},
      },
      {
        id: "userProfiles",
        name: "User Profiles",
        lastUpdated: "Loading...",
        route: "/settings/caching/user-profiles",
        click: () => {},
      },
    ],
    listViewOptions: {
      page: 1,
      itemsPerPage: 10,
    },
  }),

  mounted() {
    this.getLastRefreshedDates();
  },

  methods: {
    ...mapActions({
      getSectionsByStaffLastRefreshedDate:
        "cacheRefresh/getSectionsByStaffLastRefreshedDate",
      getStaffsBySchoolLastRefreshedDate:
        "cacheRefresh/getStaffsBySchoolLastRefreshedDate",
      getUserProfilesStaffEmailIdPairsLastRefreshedDate:
        "cacheRefresh/getUserProfilesStaffEmailIdPairsLastRefreshedDate",
      getSectionsLastRefreshedDate: "cacheRefresh/getSectionsLastRefreshedDate",
      getSchoolsLastRefreshedDate: "cacheRefresh/getSchoolsLastRefreshedDate",
      getStaffsLastRefreshedDate: "cacheRefresh/getStaffsLastRefreshedDate",
      getStudentsLastRefreshedDate: "cacheRefresh/getStudentsLastRefreshedDate",
      getAssessmentsLastRefreshedDate:
        "cacheRefresh/getAssessmentsLastRefreshedDate",
      getInterventionsLastRefreshedDate:
        "cacheRefresh/getInterventionsLastRefreshedDate",
      refreshSchools: "cacheRefresh/refreshSchools",
      refreshStaffs: "cacheRefresh/refreshStaffs",
      refreshStudents: "cacheRefresh/refreshStudents",
      refreshAssessments: "cacheRefresh/refreshAssessments",
      refreshInterventions: "cacheRefresh/refreshInterventions",
    }),

    getEndDateStr(intervention) {
      return dates.methods.toLocalShort(intervention.endDate);
    },

    showCacheTable(route) {
      this.$router.push(route);
    },

    getLastRefreshedDates() {
      const defaultValue = "----------";

      this.getStaffsBySchoolLastRefreshedDate()
        .then((result) =>
          this.updateItemLastRefreshDate("staffsBySchool", result.data)
        )
        .catch(() =>
          this.updateItemLastRefreshDate("staffsBySchool", defaultValue)
        );

      this.getSectionsByStaffLastRefreshedDate()
        .then((result) =>
          this.updateItemLastRefreshDate("sectionsByStaff", result.data)
        )
        .catch(() =>
          this.updateItemLastRefreshDate("sectionsByStaff", defaultValue)
        );

      this.getUserProfilesStaffEmailIdPairsLastRefreshedDate()
        .then((result) =>
          this.updateItemLastRefreshDate("userProfiles", result.data)
        )
        .catch(() =>
          this.updateItemLastRefreshDate("userProfiles", defaultValue)
        );

      this.getSectionsLastRefreshedDate()
        .then((result) =>
          this.updateItemLastRefreshDate("sections", result.data)
        )
        .catch(() => this.updateItemLastRefreshDate("sections", defaultValue));

      this.getSchoolsLastRefreshedDate()
        .then((result) =>
          this.updateItemLastRefreshDate("schools", result.data)
        )
        .catch(() => this.updateItemLastRefreshDate("schools", defaultValue));

      this.getStaffsLastRefreshedDate()
        .then((result) => this.updateItemLastRefreshDate("staffs", result.data))
        .catch(() => this.updateItemLastRefreshDate("staffs", defaultValue));

      this.getStudentsLastRefreshedDate()
        .then((result) =>
          this.updateItemLastRefreshDate("students", result.data)
        )
        .catch(() => this.updateItemLastRefreshDate("students", defaultValue));

      this.getAssessmentsLastRefreshedDate()
        .then((result) =>
          this.updateItemLastRefreshDate("assessments", result.data)
        )
        .catch(() =>
          this.updateItemLastRefreshDate("assessments", defaultValue)
        );

      this.getInterventionsLastRefreshedDate()
        .then((result) =>
          this.updateItemLastRefreshDate("interventions", result.data)
        )
        .catch(() =>
          this.updateItemLastRefreshDate("interventions", defaultValue)
        );
    },

    updateItemLastRefreshDate(itemId, value) {
      const item = this.items.find((item) => item.id == itemId);

      try {
        item.lastUpdated = dates.methods.toLocalShort(result.data);
      } catch {
        item.lastUpdated = value;
      }
    },
  }, // methods

  components: {
    YesNoDialog,
    LoadingDialog,
  }, // components

  mixins: [dates],
};
</script>