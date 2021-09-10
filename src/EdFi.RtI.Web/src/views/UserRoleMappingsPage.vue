<template>
    <div>

        <SharedTopBar v-if="isStartupModeHosted" title="User Role Mappings" :hideLauncher="true"/>
        <TopBar v-if="isStartupModeStandalone" title="User Role Mappings"/>

        <div style="height: 65px"></div>

        <v-container class="py-8">

            <p class="mb-6">Please select the Staff Classification Descriptors that should be mapped to the Admin and Teacher roles.</p>

            <div class="mb-4">
                <h3 class="mb-4">Admin Staff Classification Descriptors</h3>
                <p class="ma-0 mb-2" style="color: darkgray">Click on the input below to expand the Staff Classification Descriptors list</p>
                <UserRoleMappingsForm @save="onAdminMappingsSaveClicked"/>
            </div>

            <div class="mb-14">
                <h3 class="mb-4">Teacher Staff Classification Descriptors</h3>
                <p class="ma-0 mb-2" style="color: darkgray">Click on the input below to expand the Staff Classification Descriptors list</p>
                <UserRoleMappingsForm @save="onTeacherMappingsSaveClicked"/>
            </div>

            <v-layout justify-end>
                <v-btn color="accent" :disabled="isContinueDisabled" @click="onContinueClicked">
                    Continue
                    <v-icon right>mdi-chevron-right</v-icon>
                </v-btn>
            </v-layout>

        </v-container>

        <LoadingDialog ref="loadingDialog"/>
        
    </div>
</template>

<script>
import api from '@/api'
import timers from '@/utils/timers'
import storage from '@/storage'
import environment from '@/environment'
import { Routes } from '@/router/Routes'

export default {

    data: () => ({
        adminMappingsSaved: false,
        teacherMappingsSaved: false,
    }), // data

    computed: {
        loadingDialog() {
            return this.$refs['loadingDialog']
        },

        isContinueDisabled() {
            return !this.adminMappingsSaved || !this.teacherMappingsSaved
        },

        isStartupModeHosted() {
            return environment.app.isStartupModeHosted()
        },

        isStartupModeStandalone() {
            return environment.app.isStartupModeStandalone()
        },
    }, // computed

    methods: {
        async onAdminMappingsSaveClicked(mappedStaffClassificationDescriptors) {
            this.loadingDialog.open({ title: 'Saving Admin Role Mappings' })

            try {
                await api.settings.saveUserRoleAdminMappings(mappedStaffClassificationDescriptors)
                await timers.wait(1000)
                this.adminMappingsSaved = true
                this.$snotify.success('Mappings saved', '', { timeout: 1500 })
            } finally {
                this.loadingDialog.close()
            }
        },

        async onTeacherMappingsSaveClicked(mappedStaffClassificationDescriptors) {
            this.loadingDialog.open({ title: 'Saving Teacher Role Mappings' })

            try {
                await api.settings.saveUserRoleTeacherMappings(mappedStaffClassificationDescriptors)
                await timers.wait(1000)
                this.teacherMappingsSaved = true
                this.$snotify.success('Mappings saved', '', { timeout: 1500 })
            } finally {
                this.loadingDialog.close()
            }
        },

        onContinueClicked() {
            this.$router.push(Routes.static.home)
        },
    }, // methods

}
</script>
