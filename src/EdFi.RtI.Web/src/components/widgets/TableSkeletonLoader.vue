<template>
  <div>
    <!-- headers -->
    <v-row
      v-if="columnCount && columnCount > 0"
      class="mb-6 mt-6">
      <v-col
        v-for="(item, index) of columns"
        :key="index">
        <v-skeleton-loader type="text" />
      </v-col>
    </v-row><!-- headers -->

    <div
      v-if="!columnCount || columnCount == 0"
      class="mb-4">
    </div>

    <v-skeleton-loader
      v-for="(item, index) of rows"
      :key="index"
      type="text"
      class="mb-10" />
  </div>
</template>

<script>
export default {

  props: {
    columnCount: Number,
    rowCount: Number
  }, // props

  computed: {
    columns () {
      if (!this.columnCount || this.columnCount == 0)
      {return []}

      return Array.from(Array(this.columnCount).keys())
    },

    rows () {
      if (!this.rowCount || this.rowCount == 0)
      {return []}

      return Array.from(Array(this.rowCount).keys())
    }
  }, // computed

  mounted () {

  } // mounted
}
</script>
