<template>
  <div style="background: rgb(255, 255, 255); background: linear-gradient(0deg, rgba(255, 255, 255, 1) 0%, rgba(226, 237, 255, 1) 100%);">
    <v-container
      class="d-flex justify-center align-center"
      style="height: 100vh">
      <v-card
        class="mx-auto"
        min-width="550"
        max-width="550"
        elevation="2">
        <v-card-text>
          <v-row>
            <!-- logo -->
            <v-col cols="6">
              <div class="pa-7">
                <v-img
                  contain
                  class="my-3"
                  height="130"
                  :src="require('../../assets/RTILogoFINAL-06.png')" />
              </div>
            </v-col><!-- logo -->

            <!-- sign in buttons -->
            <v-col cols="6">
              <div
                class="d-flex justify-center align-center"
                style="height: 100%">
                <div
                  v-if="isSigningIn"
                  style="width: 100%;">
                  <p class="text-center">
                    Signing In...
                  </p>
                  <v-progress-linear indeterminate />
                </div>

                <div
                  v-if="isAccesingApp"
                  style="width: 100%;">
                  <p class="text-center">
                    Accesing App...
                  </p>
                  <v-progress-linear indeterminate />
                </div>

                <div
                  v-if="isShowingSignInError"
                  style="width: 100%;">
                  <h4 class="text-center mb-3">
                    Could not Sign In :(
                  </h4>
                  <v-btn
                    block
                    color="error"
                    class="text-capitalize"
                    @click="retrySignIn">
                    Retry
                  </v-btn>
                </div>
              </div>
            </v-col><!-- sign in buttons -->
          </v-row>
        </v-card-text>
      </v-card>
    </v-container>
  </div>
</template>

<script>
/* eslint-disable */

import { mapActions, mapGetters } from 'vuex'

export default {

    name: 'OidcCallback',

    data: () => ({
        isSigningIn: false,
        isAccesingApp: false,
        isShowingSignInError: true,
    }), // data

    mounted() {
        console.log('OidcCallback.mounted()');
        this.attemptSignIn();
    }, // mounted

    computed: {
        ...mapGetters({
            oidcUser: 'auth/oidcUser',
        }),
    }, // computed

    methods: {
        ...mapActions({
            oidcSignInCallback: 'auth/oidcSignInCallback',
            getProfile: 'session/getUserSessionProfileByEmail',
        }),

        attemptSignIn() {
            this.isSigningIn = true;
            this.isAccesingApp = false;
            this.isShowingSignInError = false;

            this.oidcSignInCallback()
            .then(() => {
                this.isAccesingApp = true;
                const email = this.oidcUser.email;

                this.getProfile(email)
                .then(result => {
                    this.$router.push('/profileSelection');
                })
                .catch(error => {
                    console.error('Could not get user profile of', email);
                });
            })
            .catch(error => {
                console.log('Could not sign in:', error);
                this.isShowingSignInError = true;
            })
            .finally(this.isSigningIn = false);
        },

        retrySignIn() {
            this.$router.push('/login');
        },
    }, // methods
}
</script>
