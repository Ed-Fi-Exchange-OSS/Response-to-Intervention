<template>
    <div>

        <SharedTopBar v-if="isStartupModeHosted" title="EdFi Version Selection" :hideLauncher="true"/>
        <TopBar v-if="isStartupModeStandalone" title="EdFi Version Selection"/>

        <div v-if="isStartupModeHosted" style="height: 65px"></div>

        <v-container class="py-10">

            <h2 class="mb-3">Looks like you do not have any Ed-Fi ODS API Settings</h2>

            <p class="mb-6">Please select an Ed-Fi version and fill in the form.</p>

            <v-card>
                <v-card-text>
                    <OdsApiSettingsForm :odsApiSettings="odsApiSettings" @saved="onOdsApiSettingsSaved"/>
                </v-card-text>
            </v-card>

        </v-container>
    </div>
</template>

<script>
import api from '@/api';
import { EdFiOdsApiSettings } from '@/models/EdFiOdsApiSettings';
import storage from '@/storage';
import environment from '@/environment';
import { Routes } from '@/router/Routes';

export default {

    data: () => ({
        odsApiSettings: new EdFiOdsApiSettings(),
    }), // data

    mounted() {
        console.log("mounted");
    }, // mounted

    computed: {
        isStartupModeHosted() {
            return environment.app.isStartupModeHosted()
        },

        isStartupModeStandalone() {
            return environment.app.isStartupModeStandalone()
        },
    }, // computed

    methods: {
        async onOdsApiSettingsSaved() {
            console.log("onOdsApiSettingsSaved", this.odsApiSettings);
            const edFiVersion = this.odsApiSettings.version

            try {
                await api.settings.setDefaultEdFiVersion(edFiVersion)
                storage.local.setEdFiVersion(edFiVersion)
                this.$router.push(Routes.static.home)
            } catch {
                // TODO Handle error
            }
        },
    }, // methods
}
</script>
