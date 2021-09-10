<template>
  <v-list dense>
    <!-- logo and title -->
    <v-list-item class="mb-2">
      <v-list-item-avatar
        height="60px"
        width="60px">
        <v-img :src="require('../../assets/edwire.png')"></v-img>
      </v-list-item-avatar>

      <v-list-item-content>
        <v-list-item-title><h2>RTI Module</h2></v-list-item-title>
      </v-list-item-content>
    </v-list-item><!-- logo and title -->

    <v-divider></v-divider>

    <!-- scoring -->
    <section class="mt-3 mb-2">
      <!-- divider -->
      <div>
        <p
          class="ma-0 ml-4"
          style="font-size: 13px; color: darkgray">
          Scoring
        </p>
        <v-divider></v-divider>
      </div><!-- divider -->

      <!-- scoring -->
      <v-list-item to="/scoring/assessments">
        <v-list-item-action>
          <v-icon>mdi-view-dashboard</v-icon>
        </v-list-item-action>
        <v-list-item-content>
          <v-list-item-title>
            <h3 class="font-weight-regular">
              Assessments
            </h3>
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item><!-- scoring -->

      <!-- scoring assessments -->
      <v-list-item to="/scoring/interventions">
        <v-list-item-action>
          <v-icon>mdi-text-box-check</v-icon>
        </v-list-item-action>
        <v-list-item-content>
          <v-list-item-title>
            <h3 class="font-weight-regular">
              Interventions
            </h3>
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item><!-- scoring assessments -->
    </section><!-- scoring -->

    <!-- administration -->
    <section
      v-if="isUserRoleAdmin"
      class="mt-3 mb-2">
      <!-- divider -->
      <div>
        <p
          class="ma-0 ml-4"
          style="font-size: 13px; color: darkgray">
          Administration
        </p>
        <v-divider></v-divider>
      </div><!-- divider -->

      <!-- items -->
      <div>
        <!-- assesments -->
        <v-list-item :to="'/assesments'">
          <v-list-item-action>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>
              <h3 class="font-weight-regular">
                Assesments
              </h3>
            </v-list-item-title>
          </v-list-item-content>
        </v-list-item><!-- assesments -->

        <!-- interventions -->
        <v-list-item :to="'/interventions'">
          <v-list-item-action>
            <v-icon>mdi-text-box-check</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>
              <h3 class="font-weight-regular">
                Interventions
              </h3>
            </v-list-item-title>
          </v-list-item-content>
        </v-list-item><!-- interventions -->
      </div><!-- items -->
    </section><!-- administration -->
  </v-list>
</template>

<script>
import storage from '@/storage'

export default {

  computed: {
    isUserRoleAdmin() {
      return storage.local.isUserRoleAdmin()
    }
  } // computed

}
</script>
