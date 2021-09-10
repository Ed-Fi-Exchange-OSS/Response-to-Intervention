<template>
  <div>
    <v-data-table
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

      <template v-slot:item.sections="{ item }">
        <v-chip color="primary" @click="onSectionsCountClicked(item)">
          {{getSectionsCount(item)}}
        </v-chip>
      </template>

    </v-data-table>

    <!-- side panel (sections) -->
    <v-navigation-drawer v-model="isShowingSections" fixed right temporary width="400">
      <v-list>
        <div v-for="(section, index) of selectedSections" :key="index">
          <v-list-item>
            <v-list-item-content>
                <v-list-item-title>{{section.classPeriodReference.name}} | {{section.courseOfferingReference.localCourseCode}} | {{section.courseOfferingReference.termDescriptor}}</v-list-item-title>
                <v-list-item-subtitle>Unique Section Code: <b>{{section.uniqueSectionCode}}</b></v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
          <v-divider/>
        </div>
      </v-list>
    </v-navigation-drawer><!-- side panel (sections) -->

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
    isShowingSections: false,
    selectedSections: [],
    headers: [
      {
        text: "Staff",
        value: "staffId",
        sortable: false,
      },
      {
        text: "Sections",
        value: "sections",
        sortable: false,
      },
    ],
    listViewOptions: {
      page: 1,
      itemsPerPage: 10
    },
  }), // data

  methods: {
    onSectionsCountClicked(item) {
      this.isShowingSections = true;
      this.selectedSections = item.sections;
    },

    getSectionsCount(item) {
      return item.sections.length;
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