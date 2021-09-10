<template>
  <v-dialog v-model="opened" max-width="400">
    <v-card>
      <v-card-title>
        <v-icon class="mr-4" :color="variant">{{ icon }}</v-icon>
        <span>{{ title }}</span>
      </v-card-title>
      <v-divider/>

      <v-card-text class="py-12">
        <p class="text-center">{{message}}</p>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
export default {

  data: () => ({
    opened: false,
    options: new AlertDialogOptions(),
  }), // data

  computed: {
    icon() {
      return this.options.icon
    },

    title() {
      return this.options.title
    },

    message() {
      return this.options.message
    },

    variant() {
      return this.options.variant;
    }
  },

  methods: {
    /**
     * @param {AlertDialogOptions} options
     */
    open (options) {
      this.options = DefaultAlertDialogOptions

      if (options) {
        if (options.icon)
          this.options.icon = options.icon

        if (options.title)
          this.options.title = options.title

        if (options.message)
          this.options.message = options.message

        if (options.variant)
          this.options.variant = options.variant
      }

      this.opened = true
    },
  }, // methods
}

export class AlertDialogOptions {
  icon = ''
  title = ''
  message = ''
  variant = ''
}

export const DefaultAlertDialogOptions = new AlertDialogOptions()
DefaultAlertDialogOptions.icon = 'mdi-alert-circle'
DefaultAlertDialogOptions.title = 'Alert'
DefaultAlertDialogOptions.message = 'This is an alert'
</script>
