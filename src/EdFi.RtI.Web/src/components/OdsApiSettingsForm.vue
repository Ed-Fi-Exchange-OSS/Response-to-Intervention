<template>
    <div>

        <v-select filled label="Version"
            :items="edFiVersionsArray"
            item-text="name"
            item-value="value"
            v-model="selectedEdFiVersion"
            @change="onSelectedVersionChanged"/>

        <div v-if="!isAnySuiteSelected" class="mb-6">
            <p class="text-center" style="color: gray;">You must selected an Ed-Fi Version to view the rest of the settings</p>
        </div>

        <div v-if="isAnySuiteSelected">
            <v-text-field v-if="isSuite2Selected" filled label="Auth URL" placeholder="https://api.edgraph.dev/edfi/v2.6/00000000-0000-0000-0000-000000000000/oauth/authorize/" v-model="odsApiSettings.authUrl"/>
            <v-text-field filled :disabled="loading" :loading="loading" label="Token URL" :prefix="odsApiSettings.baseUrl" :placeholder="tokenPlaceholder" v-model="odsApiSettings.tokenUrl"/>
            <v-text-field filled :disabled="loading" :loading="loading" label="Resources URL" :prefix="odsApiSettings.baseUrl" :placeholder="resourcesUrlPlaceholder" v-model="odsApiSettings.resourcesUrl"/>
            <v-text-field v-if="isSuite3Selected" filled label="Composites URL" placeholder="https://api.edgraph.dev/edfi/v5.2/saas/composites/v1/00000000-0000-0000-0000-000000000000/2011/ed-fi/" v-model="odsApiSettings.compositesUrl"/>
            <v-text-field filled :disabled="loading" :loading="loading" label="Client ID" v-model="odsApiSettings.clientId"/>
            <v-text-field filled :disabled="loading" :loading="loading" label="Client Secret" v-model="odsApiSettings.clientSecret"/>
            <v-text-field filled :disabled="loading" :loading="loading" label="Assessment Namespace" v-model="odsApiSettings.assessmentNamespace"/>
        </div>

        <v-layout justify-end>
            <v-btn color="primary" class="ml-2" :disabled="!isAnySuiteSelected || loading" text @click="onTestConnectionClicked">Test Connection</v-btn>
            <v-btn color="primary" class="ml-2" :disabled="!connectionValidated" @click="onSaveOdsApiSettingsClicked">Save</v-btn>
        </v-layout>

        <AlertDialog ref="alertDialog"/>
        <LoadingDialog ref="loadingDialog"/>

    </div>
</template>

<script>
import { EdFiVersionsArray, edFiVersionSuite2, edFiVersionSuite3 } from '@/models/EdFiVersion'
import { EdFiOdsApiSettings } from '@/models/EdFiOdsApiSettings';
import api from '@/api'
import storage from '@/storage';

// TODO Use OdsApiSettingsForm component

export default {

    props: {
        edFiVersion: {
            type: String,
        },
        odsApiSettings: {
            type: Object,
            default: () => new EdFiOdsApiSettings(),
        },
        loading: {
            type: Boolean,
            default: false,
        },
    }, // props

    data: () => ({
        edFiVersionsArray: EdFiVersionsArray,
        selectedEdFiVersion: '',
        connectionValidated: false,
    }), // data

    computed: {
        alertDialog() {
            return this.$refs.alertDialog
        },

        loadingDialog() {
            return this.$refs.loadingDialog
        },

        isAnySuiteSelected() {
            return this.selectedEdFiVersion != undefined && this.selectedEdFiVersion != null && this.selectedEdFiVersion.trim().length > 0;
        },

        isSuite2Selected() {
            return this.selectedEdFiVersion == edFiVersionSuite2.value
        },

        isSuite3Selected() {
            return this.selectedEdFiVersion == edFiVersionSuite3.value
        },

        resourcesUrlPlaceholder() {
            if (this.isSuite2Selected)
                return 'https://api.edgraph.dev/edfi/v2.6/00000000-0000-0000-0000-000000000000/api/v2.0/2011/'
      
            if (this.isSuite3Selected)
                return 'https://api.edgraph.dev/edfi/v5.2/saas/data/v3/00000000-0000-0000-0000-000000000000/2011/ed-fi/'

            return ''
        },

        tokenPlaceholder() {
            if (this.isSuite2Selected)
                return 'https://api.edgraph.dev/edfi/v2.6/00000000-0000-0000-0000-000000000000/oauth/token/'

            if (this.isSuite3Selected)
                return 'https://api.edgraph.dev/edfi/v5.2/saas/00000000-0000-0000-0000-000000000000/oauth/token/'

            return ''
        }
    }, // computed

    methods: {
        async onTestConnectionClicked() {
            this.loadingDialog.open({ title: 'Testing connection' })

            try {
                this.odsApiSettings.version = this.selectedEdFiVersion;
                await api.settings.testOdsApiSettings(this.odsApiSettings)
                this.alertDialog.open({ icon: 'mdi-check', title: 'Connection Succeeded', message: 'The Settings are correct! You can now click the save button.', variant: 'success' })
                this.connectionValidated = true
            } catch {
                this.alertDialog.open({ icon: 'mdi-alert-circle-outline', title: 'Connection Failed', message: 'The Settings are incorrect!', variant: 'error' })
                this.connectionValidated = false
            } finally {
                this.loadingDialog.close()
            }
        },

        async onSaveOdsApiSettingsClicked() {
            this.loadingDialog.open({ title: 'Saving Ed-Fi ODS API Settings'});

            try {
                await api.settings.saveOdsApiSettings(this.odsApiSettings)
                this.$snotify.success('Settings saved!', '', { timeout: 2000 })
                this.$emit('saved')
            } finally {
                this.loadingDialog.close()
            }
        },

        onSelectedVersionChanged() {
            this.$emit('edFiVersionChanged', this.selectedEdFiVersion)
        },
    }, // methods

    watch: {
        edFiVersion() {
            this.selectedEdFiVersion = this.edFiVersion
        }
    }, // watch
}
</script>
