<template>
  <div>
    <v-data-table
      class="elevation-3"
      :headers="headers"
      :items="items"
      :loading="isLoading"
      :hide-default-footer="isLoading"
    >
      <template v-slot:no-data>
        <div></div>
      </template>
    </v-data-table>
  </div>
</template>

<script>
import { dates } from "@/mixins/mixins";
export default {
  props: ["items", "isLoading"],

  data: () => ({
    headers: [
      {
        text: "Section Name",
        value: "classPeriodReference.name",
        sortable: false,
      },
      {
        text: "Term Descriptor",
        value: "courseOfferingReference.termDescriptor",
        sortable: false,
      },
      {
        text: "Unique Section Code",
        value: "uniqueSectionCode",
        sortable: false,
      },
    ],
    listViewOptions: {
      page: 1,
      itemsPerPage: 10,
    },
  }),

  methods: {
    getBirthDateStr(student) {
      return dates.methods.toLocalShort(student.birthDate);
    },
  },

  components: {}, // components

  mixins: [dates],
};
</script>