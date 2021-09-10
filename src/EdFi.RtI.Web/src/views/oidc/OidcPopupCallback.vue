<template>
  <div>
  </div>
</template>

<script>
import { mapActions } from "vuex"

export default {
  name: "OidcPopupCallback",
  created () {
    this.oidcSignInPopupCallback()
      .catch((err) => {
        console.error(err) // Handle errors any way you want
      })
  },
  methods: {
    ...mapActions("oidcStore", [
      "oidcSignInPopupCallback"
    ])
  }
}
</script>
