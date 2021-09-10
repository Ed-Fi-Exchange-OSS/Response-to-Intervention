<template>
  <div></div>
</template>

<script lang="ts">
import Vue from "vue"
import { mapActions } from "vuex"

export default {
  mounted () {
    this.$nextTick(() => {
      this.oidcSignInCallback()
        .then((redirectPath: any) => {
          //localStorage.setItem("userRole", "user")
          if (redirectPath) {
            this.$router.push(redirectPath)
          }
          else {
            this.$router.push("/")
          }
        })
        .catch((err: Error) => {
          console.error(err)
          this.$router.push("/")
        })
    })
  },

  methods: {
    ...mapActions("auth", [
      "oidcSignInCallback"
    ])
  }, // methods
}
</script>
