<template>
  <v-app-bar>
    <v-toolbar-title>Scoring Assessments</v-toolbar-title>

    <v-spacer></v-spacer>

    <!-- add -->
    <div
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

    <div
      class="d-flex flex-column align-center mx-2"
      :class="{ 'cursor-pointer': !disabled }">
      <v-btn
        small
        icon
        :disabled="disabled"
        @click="onDeleteClicked">
        <v-icon>mdi-delete</v-icon>
      </v-btn>
      <p
        class="ma-0"
        style="font-size: 12px">
        Delete
      </p>
    </div><!-- delete -->

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
      :class="{ 'cursor-pointer': isUndoEnabled }">
      <v-btn
        small
        icon
        :disabled="disabled || !isUndoEnabled"
        @click="onUndoClicked">
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
    disabled: Boolean,
    isUndoEnabled: Boolean
  }, // props

  methods: {
    onAddClicked () {
      this.$emit("add")
    },

    onDeleteClicked (){
      this.$emit("delete")
    },

    onRefreshClicked () {
      this.$emit("refresh")
    },

    onExportClicked () {
      this.$emit("export")
    },

    onSaveClicked () {
      this.$emit("save")
    },

    onUndoClicked () {
      this.$emit("undo")
    }
  } // methods
}
</script>
