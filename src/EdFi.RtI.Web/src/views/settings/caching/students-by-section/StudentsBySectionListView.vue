<template>
  <div>
    <v-data-table
      :headers="headers"
      :items="items"
      :loading="isLoading"
      :server-items-length="items.length"
      :hide-default-footer="isLoading"
    >
      <template v-slot:no-data>
        <div></div>
      </template>

      <template v-slot:item.students="{ item }">
        <v-chip color="primary" @click="onStudentsCountClicked(item)">
          {{ getStaffsCount(item) }}
        </v-chip>
      </template>
    </v-data-table>

    <!-- side panel (students) -->
    <v-navigation-drawer v-model="isShowingStudents" fixed right temporary>
      <v-list>
        <v-list-item v-for="(student, index) of selectedStudents" :key="index">
          <v-list-item-content>
            <v-list-item-title>{{ student.lastSurname }}, {{student.firstName}}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list> </v-navigation-drawer
    ><!-- side panel (students) -->
  </div>
</template>

<script>
import { dates } from "@/mixins/mixins";
export default {
  props: ["items", "isLoading"],

  data: () => ({
    selectedStudents: [],
    isShowingStudents: false,
    headers: [
      {
        text: "Students",
        value: "students",
        sortable: false,
      },
      {
        text: "Term Descriptor",
        value: "section.courseOfferingReference.termDescriptor",
        sortable: false,
      },
      {
        text: "Unique Section Code",
        value: "section.uniqueSectionCode",
        sortable: false,
      },
    ],
    listViewOptions: {
      page: 1,
      itemsPerPage: 10,
    },
  }),

  methods: {
    getFullName(staff) {
      return `${staff.firstName} ${staff.lastSurname}`;
    },

    getStaffsCount(item) {
      return item.students.length;
    },

    onStudentsCountClicked(item) {
      console.log("item", item);
      this.selectedStudents = item.students;
      this.isShowingStudents = true;
    },
  },

  components: {}, // components

  mixins: [dates],
};
</script>