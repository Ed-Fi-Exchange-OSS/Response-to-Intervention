<template>
  <div
    style="
        background: rgb(255, 255, 255);
        background: linear-gradient(0deg, rgba(255, 255, 255, 1) 0%, rgba(226, 237, 255, 1) 100%);">
    <!-- main container -->
    <v-container
      class="d-flex justify-center align-center"
      style="height: 100vh">
      <!-- login card -->
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
                  :src="require('../assets/RTILogoFINAL-06.png')" />
              </div>
            </v-col><!-- logo -->

            <!-- sign in buttons -->
            <v-col cols="6">
              <div
                class="d-flex justify-center align-center"
                style="height: 100%">
                <div style="width: 100%">
                  <h1 class="mb-3">
                    Sign In
                  </h1>
                  <h3 class="font-weight-regular mb-6">
                    Pick your provider
                  </h3>

                  <v-btn
                    block
                    color="primary"
                    class="mt-3"
                    :disabled="isLoggingIn"
                    @click.prevent="openProfileSelectionDialog">
                    Office 365
                  </v-btn>
                  <v-btn
                    block
                    color="error"
                    class="mt-3"
                    :disabled="isLoggingIn"
                    @click="signInWithAuth">
                    Google
                  </v-btn>

                  <div v-if="isLoggingIn">
                    <p class="text-center mt-6">
                      Signing In...
                    </p>
                    <v-progress-linear indeterminate />
                  </div>
                </div>
              </div>
            </v-col><!-- sign in buttons -->
          </v-row>
        </v-card-text>
      </v-card><!-- login card -->
    </v-container><!-- main container -->

    <!-- dialogs -->
    <div>
      <!-- select user dialog -->
      <v-dialog
        v-model="isShowingProfilesSelectionDialog"
        scrollable
        max-width="400">
        <v-card>
          <v-card-title>Select a Profile</v-card-title>
          <v-divider></v-divider>

          <v-card-text class="pa-0">
            <!-- loading profiles -->
            <div
              v-if="isLoadingProfiles"
              class="py-16">
              <div class="d-flex justify-center">
                <p>Loading Profiles...</p>
              </div>
              <div class="d-flex justify-center">
                <v-progress-circular indeterminate />
              </div>
            </div><!-- loading profiles -->

            <!-- profiles loaded -->
            <div v-if="!isLoadingProfiles">
              <div
                v-for="(profile, index) of profiles"
                :key="index">
                <v-list-item @click="onProfileSelected(profile)">
                  <v-list-item-content>
                    <v-list-item-title>{{ profile.staff.fullName }}</v-list-item-title>
                    <v-list-item-subtitle>{{ profile.staffEducationOrganizationAssignmentAssociation.staffClassificationDescriptor }}</v-list-item-subtitle>
                  </v-list-item-content>
                </v-list-item>
                <v-divider v-if="index < profiles.length - 1" />
              </div>
            </div><!-- profiles loaded -->
          </v-card-text>

          <v-divider></v-divider>
          <v-card-actions>
            <v-spacer />
            <v-btn
              text
              color="error"
              class="text-capitalize"
              @click="isShowingProfilesSelectionDialog = false">
              Close
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog><!-- select user dialog -->
    </div><!-- dialogs -->
  </div>
</template>

<script>
import { mapActions } from "vuex"

export default {

  data: () => ({
    isLoadingProfiles: false,
    isLoggingIn: false,
    isShowingProfilesSelectionDialog: false,
    profiles: []
  }), // mounted

  computed: {

  }, // data

  mounted () {
    this.getUserSessionProfileAllRequest()

    const auto = this.$route.query.auto

    if (auto && auto.toString() == "true"){
      this.isShowingProfilesSelectionDialog = true
    }
  }, // computed

  methods: {
    ...mapActions({
      getUserSessionProfileAll: "session/getUserSessionProfileAll",
      signIn: "session/signIn"
    }),

    auth (){
      this.$router.push("/callback")
    },

    getUserSessionProfileAllRequest () {
      this.isLoadingProfiles = true

      this.getUserSessionProfileAll()
        .then((result) => this.profiles = result.data)
        .finally(() => this.isLoadingProfiles = false)
    },

    onProfileSelected (selectedProfile) {
      this.isShowingProfilesSelectionDialog = false
      this.isLoggingIn = true

      this.signIn(selectedProfile)
        .then(() => this.$router.push("/home"))
    },

    openProfileSelectionDialog () {
      this.isShowingProfilesSelectionDialog = true
    },

    signInCallback () {

    },

    signInWithAuth () {
      this.$router.push("/callback")
    }

  } // methods
}
</script>
