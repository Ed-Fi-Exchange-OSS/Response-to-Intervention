<template>
    <v-footer fixed padless style="background-color: #E0E0E0;">
        <v-layout justify-end class="px-3 py-1">

            <span class="ml-8" style="color: #4F4F4F; font-size: 14px;">
                Startup Mode <b>{{startupMode}}</b>
            </span>

            <span v-if="hasEdFiVersion" class="ml-8" style="color: #4F4F4F; font-size: 14px;">
                Ed-Fi Version <b>{{edFiVersionDescription}}</b>
            </span>

        </v-layout>
    </v-footer>
</template>

<script>
import api from '@/api'
import environment from '@/environment'
import { edFiVersionSuite2, edFiVersionSuite3 } from '@/models/EdFiVersion'
import { mapGetters } from 'vuex'

export default {

    data: () => ({
        edFiVersion: '',
    }), // data

    mounted() {
        if (this.oidcIsAuthenticated)
            this.getEdFiVersion()
    }, // mounted

    computed: {
        ...mapGetters({
            oidcIsAuthenticated: 'auth/oidcIsAuthenticated',
        }),

        edFiVersionDescription() {
            if (this.edFiVersion == edFiVersionSuite2.value)
                return edFiVersionSuite2.name

            if (this.edFiVersion == edFiVersionSuite3.value)
                return edFiVersionSuite3.name
        
            return '';
        },

        hasEdFiVersion() {
            return this.edFiVersion != undefined && this.edFiVersion != null && this.edFiVersion.trim().length > 0
        },

        startupMode() {
            return environment.app.getStartupMode()
        }
    }, // computed

    methods: {
        async getEdFiVersion() {
            this.edFiVersion = await api.settings.getDefaultEdFiVersion()
        },
    }, // methods

    watch: {
        oidcIsAuthenticated() {
            if (this.oidcIsAuthenticated)
                this.getEdFiVersion()
        },
    }, // watch
}
</script>
