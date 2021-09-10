<template>
<!--eslint-disable-->
    <div>

        <v-card>
            <v-card-text>

                <div v-if="loadingFeatures">
                    <v-skeleton-loader type="list-item-three-line"/>
                    <v-skeleton-loader type="list-item-three-line"/>
                    <v-skeleton-loader type="list-item-three-line"/>
                </div>

                <div v-if="!loadingFeatures">
                    <v-alert v-if="!hasFeaturesSettings" type="warning">
                        Features Settings for this Tenant have not been stored. Apply some changes and click the <b>Save</b> button to save the configuration.
                    </v-alert>

                    <v-alert type="info" text class="mb-6">
                        The app will be reloaded once the <b>Save</b> button is pressed.
                    </v-alert>

                    <!-- show assessments -->
                    <div class="mb-8">
                        <v-switch v-model="features.showAssessments" class="mb-2" color="accent" label="Show Assessments" hide-details/>
                        <p>If toggled on, shows the <b>Assessments</b> tab in both the <b>Scoring</b> and <b>Administration</b> sections </p>
                    </div><!-- show assessments -->

                    <!-- show assessments -->
                    <div class="mb-8">
                        <v-switch v-model="features.showInterventions" class="mb-2" color="accent" label="Show Interventions" hide-details/>
                        <p>If toggled on, shows the <b>Interventions</b> tab in both the <b>Scoring</b> and <b>Administration</b> sections </p>
                    </div><!-- show assessments -->
                </div>

            </v-card-text>
            
            <v-card-actions v-if="!loadingFeatures">
                <v-spacer/>
                <v-btn class="font-weight-regular text-capitalize" color="primary" @click="onSaveClicked">
                    <v-icon left>mdi-content-save</v-icon>
                    Save
                </v-btn>
            </v-card-actions>
        </v-card>

        <LoadingDialog ref="loadingDialog"/>

    </div>
</template>

<script>
/* eslint-disable */
import AppToolbar from "@/components/widgets/AppToolbar";
import LoadingDialog from "@/components/dialogs/LoadingDialog";

import { mapActions } from "vuex";
import timers from '@/utils/timers'

export default {
    
    data: () => ({
        loadingFeatures: true,
        hasFeaturesSettings: false,
        features: {
            showAssessments: false,
            showInterventions: false,
        },
    }), // data
    
    mounted() {
        this.getFeaturesSettingsRequest();
    }, // mounted
    
    methods: {
        ...mapActions({
            getFeaturesSettings: "features/getFeaturesSettings",
            updateFeatures: "features/updateFeatures",
        }),

        async onSaveClicked() {
            this.$refs.loadingDialog.open({ title: "Updating Features Settings" });

            try {
                await this.updateFeatures(this.features);
                await timers.wait(1000)
                window.location.reload();
            } finally {
                this.$refs.loadingDialog.close();
            }
        },

        async getFeaturesSettingsRequest() {
            this.loadingFeatures = true;

            try {
                this.features = await this.getFeaturesSettings();
                this.hasFeaturesSettings = true;
            } catch {
                this.hasFeaturesSettings = false;
            } finally {
                this.loadingFeatures = false;
            }
        },
    }, // methods
    
    components: {
        AppToolbar,
        LoadingDialog,
    }, // components
}
</script>
