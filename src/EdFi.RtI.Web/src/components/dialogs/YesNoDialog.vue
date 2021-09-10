<template>
  <v-dialog
    v-model="isDialogOpened"
    max-width="400"
    persistent>
    <v-card>
      <v-toolbar
        dense
        elevation="0.50">
        <v-toolbar-title>
          <h5 class="font-weight-regular">
            {{ title }}
          </h5>
        </v-toolbar-title>
      </v-toolbar>
      <v-card-text class="mt-6">
        <p class="ma-0">
          {{ message }}
        </p>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn
          color="primary"
          class="text-capitalize ml-2"
          @click.native="onYesClicked">
          Yes
        </v-btn>
        <v-btn
          color="error"
          class="text-capitalize ml-2"
          @click.native="onNoClicked">
          No
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: "YesNoDialog",

  data () {
    return {
      isDialogOpened: false,
      title: "Title",
      message: "This is a message",
      resolve: null,
      reject: null
    }
  }, // data

  methods: {
    open ({ title, message }) {
      this.isDialogOpened = true

      if (title) {this.title = title}
      if (message) {this.message = message}

      return new Promise((resolve, reject) => {
        this.resolve = resolve
        this.reject = reject
      })
    },

    onNoClicked () {
      if (this.reject) {this.reject()}
      this.isDialogOpened = false
    },

    onYesClicked () {
      if (this.resolve) {this.resolve()}
      this.isDialogOpened = false
    }
  } // methods
}
</script>
