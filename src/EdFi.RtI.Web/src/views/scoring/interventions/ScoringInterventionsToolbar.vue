<template>
  <v-app-bar>
    <v-toolbar-title>Scoring Interventions</v-toolbar-title>

    <v-spacer></v-spacer>

    <!-- add -->
    <div
      v-if="false"
      class="d-flex flex-column align-center mx-2"
      :class="{ 'cursor-pointer': !disabled }">
      <v-btn
        small
        icon
        :disabled="disabled"
        @click="onAddClicked">
        <v-icon>mdi-plus</v-icon>
      </v-btn>
      <p
        class="ma-0"
        style="font-size: 12px">
        Add
      </p>
    </div><!-- add -->

    <!-- export -->
    <div
      class="d-flex flex-column align-center mx-2"
      :class="{ 'cursor-pointer': !disabled }">
      <v-btn
        small
        icon
        :disabled="disabled"
        @click="onRefreshClicked">
        <v-icon>mdi-rotate-right</v-icon>
      </v-btn>
      <p
        class="ma-0"
        style="font-size: 12px">
        Refresh
      </p>
    </div><!-- export -->

    <!-- export -->
    <div
      class="d-flex flex-column align-center mx-2"
      :class="{ 'cursor-pointer': !disabled }">
      <v-btn
        small
        icon
        :disabled="disabled"
        @click="onExportClicked">
        <v-icon>mdi-file-export-outline</v-icon>
      </v-btn>
      <p
        class="ma-0"
        style="font-size: 12px">
        Export
      </p>
    </div><!-- export -->

    <!-- undo -->
    <div
      class="d-flex flex-column align-center mx-2"
      :class="{ 'cursor-pointer': !disabled }">
      <v-btn
        small
        icon
        :disabled="disabled">
        <v-icon>mdi-undo</v-icon>
      </v-btn>
      <p
        class="ma-0"
        style="font-size: 12px">
        Undo
      </p>
    </div><!-- undo -->

    <!-- save -->
    <div
      class="d-flex flex-column align-center mx-2"
      :class="{ 'cursor-pointer': !disabled }">
      <v-btn
        small
        icon
        :disabled="disabled"
        @click="onSaveClicked">
        <v-icon>mdi-content-save</v-icon>
      </v-btn>
      <p
        class="ma-0"
        style="font-size: 12px">
        Save
      </p>
    </div><!-- save -->
  </v-app-bar>
</template>

<script>
export default {

  props: {
    disabled: Boolean
  }, // props

  methods: {
    onAddClicked () {
      this.$emit("add")
    },

    onRefreshClicked () {
      this.$emit("refresh")
    },

    onExportClicked () {
      this.$emit("export")
    },

    onSaveClicked () {
      this.$emit("save")
    }
  } // methods
}
</script>
