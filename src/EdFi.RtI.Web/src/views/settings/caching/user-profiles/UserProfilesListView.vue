<template>
  <div>
    <v-data-table
      :headers="headers"
      :items="items"
      :options.sync="listViewOptions"
      :server-items-length="100"
      :hide-default-footer="isLoading"
      :footer-props="{ 'items-per-page-options': [10, 20, 30, 40, 50] }">
      
      <template v-slot:no-data>
        <div></div>
      </template>

      <template v-slot:item.email="{ item }">
        {{getEmail(item)}}
      </template>

    </v-data-table>

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
    headers: [
      {
        text: "First Name",
        value: "staff.firstName",
        sortable: false,
      },
      {
        text: "Last Surname",
        value: "staff.lastSurname",
        sortable: false,
      },
      {
        text: "Email",
        value: "email",
        sortable: false,
      },
    ],
    listViewOptions: {
      page: 1,
      itemsPerPage: 10
    }
  }), // data

  methods: {
    getEmail(profile) {
      const staff = profile.staff;
      const electronicMails = staff.electronicMails;
      const emails = electronicMails.map(email => email.electronicMailAddress);
      return emails.join(", ");
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