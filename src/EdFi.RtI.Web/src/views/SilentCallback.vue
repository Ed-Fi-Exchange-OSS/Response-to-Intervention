<template>
  <div></div>
</template>

<script lang="ts">
import { vuexOidcProcessSilentSignInCallback } from "vuex-oidc"
import Vue from "vue"

export default Vue.extend({
  created () {
    vuexOidcProcessSilentSignInCallback()
    console.log("✅ Silently refreshed access token!")
  }
})
</script>
