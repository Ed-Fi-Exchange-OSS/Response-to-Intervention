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
  props: ["items", "isLoading", "searchParams"],

  data: () => ({
    headers: [
      {
        text: "School Name",
        value: "nameOfInstitution",
        sortable: false,
      },
      {
        text: "Administrative Funding Control Descriptor",
        value: "administrativeFundingControlDescriptor",
        sortable: false,
      },
    ],
    listViewOptions: {
      page: 1,
      itemsPerPage: 10,
    },
  }),

  methods: {
    getBirthDateStr(staff) {
      return dates.methods.toLocalShort(staff.birthDate);
    },
  },

  components: {}, // components

  mixins: [dates],
};
</script>