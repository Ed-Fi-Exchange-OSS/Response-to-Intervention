<template>
  <div>
    <v-app-bar>
      <v-btn icon>
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-toolbar-title>Intervention Detail</v-toolbar-title>

      <v-spacer></v-spacer>
    </v-app-bar>
    <v-container>
      <v-row>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Lexio"
            label="Name*"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Class Type*"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Delivery Method Type*"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="5/20/2020"
            label="Begin Date*"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="6/30/2020"
            label="End Date"
            disabled></v-text-field>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>
<script>
export default {
}
</script>
