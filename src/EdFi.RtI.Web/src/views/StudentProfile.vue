<template>
  <div>
    <v-app-bar>
      <v-btn icon>
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-toolbar-title>Student: John Doe (ID:12345)</v-toolbar-title>

      <v-spacer></v-spacer>
    </v-app-bar>
    <v-container>
      <v-row>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Email Address"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Characteristics"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Phone Number"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Disabilities"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Birth Date"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Indicators"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Sex"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Programs"
            disabled></v-text-field>
        </v-col>
        <v-col
          cols="12"
          sm="6">
          <v-text-field
            value="Text value"
            label="Race(s)"
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
