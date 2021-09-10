<template>
  <div>
    <v-data-table
      class="elevation-3"
      :headers="headers"
      :items="items"
      :loading="isLoading"
      :options.sync="listViewOptions"
      :server-items-length="items.length"
      :hide-default-footer="isLoading"
      :footer-props="{ 'items-per-page-options': [] }">
      
      <template v-slot:no-data>
        <div></div>
      </template>

      <template v-slot:item.staffs="{ item }">
        <v-chip color="primary" @click="onStaffCountClicked(item)">
          {{getStaffsCount(item)}}
        </v-chip>
      </template>

    </v-data-table>

    <!-- side panel (staffs) -->
    <v-navigation-drawer v-model="isShowingStaffs" fixed right temporary>
      <v-list>
        <v-list-item v-for="(staff, index) of selectedStaffs" :key="index">
          <v-list-item-content>
              <v-list-item-title>{{getFullName(staff)}}</v-list-item-title>
              <v-list-item-subtitle>Staff Unique Id: <b>{{staff.staffUniqueId}}</b></v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer><!-- side panel (staffs) -->

  </div>
</template>

<script>
export default {

  props: {
    items: {
      type: Array,
      default: () => [],
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
    searchParams: {
      type: Object,
      default: () => ({}),
    }
  }, // props

  data: () => ({
    isShowingStaffs: false,
    selectedStaffs: [],
    headers: [
      {
        text: "School",
        value: "school.nameOfInstitution",
        sortable: false,
      },
      {
        text: "School Short Name",
        value: "school.shortNameOfInstitution",
        sortable: false,
      },
      {
        text: "Staffs",
        value: "staffs",
        sortable: false,
      },
    ],
    listViewOptions: {
      page: 1,
      itemsPerPage: 10
    },
  }), // data

  methods: {
    getFullName(staff) {
      return `${staff.firstName} ${staff.lastSurname}`;
    },

    getStaffsCount(item) {
      return item.staffs.length;
    },

    onStaffCountClicked(item) {
      console.log("item", item);
      this.selectedStaffs = item.staffs;
      this.isShowingStaffs = true;
    },
  }, // methods

  watch: {
    listViewOptions: {
      handler () {

        console.log("Sort by:", this.listViewOptions.sortBy[0])
        console.log("Desc:", this.listViewOptions.sortDesc[0])

        this.searchParams.pageNumber = this.listViewOptions.page
        this.searchParams.pageSize = this.listViewOptions.itemsPerPage
        this.searchParams.sort = this.listViewOptions.sortBy[0]
        this.searchParams.desc = this.listViewOptions.sortDesc[0]

        if (this.listViewOptions.sortBy[0] == undefined || this.listViewOptions.sortDesc[0] == undefined){
          this.searchParams.sort = "fullName"
          this.searchParams.desc = false
        }

        console.log("Info: ", this.searchParams)

        this.$emit("search-params-changed", this.searchParams)
      }
    }
  }, // watch
}
</script>