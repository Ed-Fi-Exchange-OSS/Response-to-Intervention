<template>
  <v-dialog
    v-model="isDialogOpened"
    max-width="400"
    persistent>
    <v-card>
      <v-card-title>{{ title }}</v-card-title>
      <v-divider />

      <v-card-text class="py-12">
        <p class="text-center">
          {{ message }}
        </p>
        <div class="d-flex justify-center">
          <v-progress-circular indeterminate />
        </div>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
export default {

  name: "LoadingDialog",

  data () {
    return {
      isDialogOpened: false,
      title: "Loading",
      message: "Please wait",
      resolve: null,
      reject: null
    }
  }, // data

  methods: {
    open ({ title, message }) {
      if (title) this.title = title
      else this.title = "Loading"

      if (message) this.message = message
      else this.message = "Please wait..."

      this.isDialogOpened = true
    },

    close () {
      this.isDialogOpened = false
    }
  } // methods
}
</script>
