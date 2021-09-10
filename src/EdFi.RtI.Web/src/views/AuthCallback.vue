<template>
  <h1>Lorem ipsum dolor, sit amet consectetur adipisicing elit. Debitis doloremque distinctio dicta nobis ab id pariatur? Autem eum, quam, voluptas officia, inventore quo sit eveniet porro facilis dolorum cumque obcaecati!</h1>
</template>
<script>
import { mapActions, mapGetters } from "vuex"
export default {
  computed: {
    ...mapGetters("oidc", [
      "oidcIsAuthenticated",
      "oidcAuthenticationIsChecked",
      "oidcUser",
      "oidcIdToken",
      "oidcIdTokenExp"
    ])
  },

  mounted (){
    console.log("DDDERGTHHY")
    this.$nextTick(() => {
      console.log("Antes de")
      this.oidcSignInCallback().then((result) => {
        console.log("Entroo: ", result)
      }).catch((error) => {
        console.log("Fallo Aqui:", error)
      })
    })
  },

  methods: {
    ...mapActions({
      signIn: "session/signIn",
      authenticateOidcPopup: "oidc/authenticateOidcPopup",
      authenticateOidcSilent: "oidc/authenticateOidcSilent",
      removeOidcUser: "oidc/removeOidcUser",
      oidcSignInCallback: "oidc/oidcSignInCallback"
    })

  }

}
</script>
