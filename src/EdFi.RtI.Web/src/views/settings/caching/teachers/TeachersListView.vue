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

      <template v-slot:item.fullName="{ item }">
        {{item.lastSurname}}, {{item.firstName}}
      </template>

      <template v-slot:item.birthDate="{ item }">
        {{ getBirthDateStr(item) }}
      </template>

    </v-data-table>
  </div>
</template>

<script>
import { dates } from "@/mixins/mixins";
import { mapActions } from "vuex";
export default {
  props: ["items", "isLoading"],

  data: () => ({
    headers: [
      {
        text: "Staff Name",
        value: "fullName",
        sortable: false,
      },
      {
        text: "Birth Date",
        value: "birthDate",
        sortable: false,
      },
    ],
    listViewOptions: {
      page: 1,
      itemsPerPage: 10,
    },
  }),

  methods: {
    ...mapActions({}),

    getBirthDateStr(staff) {
      return dates.methods.toLocalShort(staff.birthDate);
    },
  },

  components: {}, // components

  mixins: [dates],
};
</script>