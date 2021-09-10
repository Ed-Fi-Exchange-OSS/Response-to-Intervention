<template>
    <div>

        <SharedTopBar
            title="User Email Mappings"
            :hideLauncher="true"/>

        <div style="height: 65px"></div>
        
        <v-container class="py-8">

            <h2 class="mb-8">Specify the emails that should be mapped to specific Staff Emails</h2>

            <v-alert icon="mdi-alert" color="info" dark class="mb-4">
                This option should only be available for the <b>Development</b> environment. If you are testing this application with a third party email, we recommend that you <b>DO NOT SKIP THIS STEP</b>.
            </v-alert>

            <v-layout justify-end class="mb-4">
                <v-btn class="ml-4" color="primary" @click="onAddEmailMappingsClicked">Add Email Mappings</v-btn>
                <v-btn class="ml-4" color="accent" :disabled="isSaveDisabled" @click="onSaveClicked">Save</v-btn>
                <v-btn class="ml-4" color="primary" @click="onContinueClicked">Continue</v-btn>
            </v-layout>

            <div v-for="(item, index) of userEmailMappings" :key="index" class="mb-6">
                <UserEmailMappingsItem :item="item" @remove="onRemoveEmailMappingClicked(index)"/>
            </div>

        </v-container>

        <LoadingDialog ref="loadingDialog"/>

    </div>
</template>

<script>
import api from '@/api'
export default {

    data: () => ({
        userEmailMappings: [ {} ],
    }),

    computed: {
        isSaveDisabled() {
            return this.userEmailMappings.length == 0
        },

        loadingDialog() {
            return this.$refs['loadingDialog']
        },
    }, // computed

    methods: {
        onAddEmailMappingsClicked() {
            this.userEmailMappings.push({})
        },

        onRemoveEmailMappingClicked(index) {
            this.userEmailMappings.splice(index, 1)
        },

        async onSaveClicked() {
            console.log("this.userEmailMappings:", this.userEmailMappings)
            this.loadingDialog.open({ title: 'Saving Email Mappings' })

            try {
                await api.settings.setUserEmailMappings(this.userEmailMappings)
                this.$snotify.success('Mappings saved', '', { timeout: 1500 })
            } finally {
                this.loadingDialog.close()
            }
        },

        onContinueClicked() {

        },
    }, // methods
}
</script>

<style>

</style>
