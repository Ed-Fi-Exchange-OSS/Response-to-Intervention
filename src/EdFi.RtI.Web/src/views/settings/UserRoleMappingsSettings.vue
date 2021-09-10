<template>
    <div>

        <!-- admin -->
        <div class="mb-8">
            <h3 class="mb-4">Admin Staff Classification Descriptors</h3>
            <v-card>
                <v-card-text>
                    <v-combobox
                        clearable multiple solo chips hide-details
                        placeholder="Select the Staff Classification Descriptors you would like to map to Admin users"
                        :items="staffClassificationDescriptorsCodes"
                        :loading="loading"
                        v-model="adminStaffClassificationDescriptors"
                        @change="onAdminStaffClassificationDescriptorsChange">
                        <template v-slot:selection="{ attrs, item, select, selected }">
                            <v-chip
                                v-bind="attrs"
                                :input-value="selected"
                                close
                                @click="select"
                                @click:close="onRemoveUserRoleAdmin(item)">
                                {{ item }}&nbsp;
                            </v-chip>
                        </template>
                    </v-combobox>
                </v-card-text>
                <v-card-actions>
                    <v-spacer/>
                    <v-btn color="primary" :disabled="loading || !isAdminRolesValid" @click="onSaveUserRolesAdminClicked">Save</v-btn>
                </v-card-actions>
            </v-card>
        </div><!-- admin -->

        <!-- teacher -->
        <div>
            <h3 class="mb-4">Teacher Staff Classification Descriptors</h3>
            <v-card>
                <v-card-text>
                    <v-combobox
                        clearable multiple solo chips hide-details
                        placeholder="Select the Teacher Classification Descriptors you would like to map to Admin users"
                        :items="staffClassificationDescriptorsCodes"
                        :loading="loading"
                        v-model="teacherStaffClassificationDescriptors"
                        @change="onTeacherStaffClassificationDescriptorsChange">
                        <template v-slot:selection="{ attrs, item, select, selected }">
                            <v-chip
                                v-bind="attrs"
                                :input-value="selected"
                                close
                                @click="select"
                                @click:close="onRemoveUserRoleTeacher(item)">
                                {{ item }}&nbsp;
                            </v-chip>
                        </template>
                    </v-combobox>
                </v-card-text>
                <v-card-actions>
                    <v-spacer/>
                    <v-btn color="primary" :disabled="loading || !isTeacherRolesValid" @click="onSaveUserRolesTeacherClicked">Save</v-btn>
                </v-card-actions>
            </v-card>
        </div><!-- teacher -->

        <LoadingDialog ref="loadingDialog"/>
    </div>
</template>

<script>
import api from '@/api'
import timers from '@/utils/timers'
import storage from '@/storage'

export default {

    data: () => ({
        loading: false,
        staffClassificationDescriptors: [],
        adminStaffClassificationDescriptors: [],
        teacherStaffClassificationDescriptors: [],
        savingTeacherUserRoles: false,
    }), // data

    mounted() {
        this.init()
    }, // mounted

    computed: {
        loadingDialog() {
            return this.$refs['loadingDialog']
        },

        staffClassificationDescriptorsCodes() {
            return this.staffClassificationDescriptors.map(x => x.codeValue);
        },

        isAdminRolesValid() {
            return this.adminStaffClassificationDescriptors.length > 0
        },

        isTeacherRolesValid() {
            return this.teacherStaffClassificationDescriptors.length > 0
        },
    }, // computed

    methods: {
        async init() {
            this.loading = true

            try {
                this.staffClassificationDescriptors = await api.descriptors.getStaffClassificationDescriptors()
                this.adminStaffClassificationDescriptors = await api.settings.getUserRoleAdminMappings()
                this.teacherStaffClassificationDescriptors = await api.settings.getUserRoleTeacherMappings()
            } finally {
                this.loading = false
            }
        },

        onAdminStaffClassificationDescriptorsChange() {
            const staffClassificationDescriptors = new Set(this.staffClassificationDescriptorsCodes)
            
            for (let i = 0; i < this.adminStaffClassificationDescriptors.length; i++) {
                const descriptor = this.adminStaffClassificationDescriptors[i]

                if (!staffClassificationDescriptors.has(descriptor))
                    this.adminStaffClassificationDescriptors.splice(i, 1)
            }
        },

        onTeacherStaffClassificationDescriptorsChange() {
            const staffClassificationDescriptors = new Set(this.staffClassificationDescriptorsCodes)
            
            for (let i = 0; i < this.teacherStaffClassificationDescriptors.length; i++) {
                const descriptor = this.teacherStaffClassificationDescriptors[i]

                if (!staffClassificationDescriptors.has(descriptor))
                    this.teacherStaffClassificationDescriptors.splice(i, 1)
            }
        },

        onRemoveUserRoleAdmin(item) {
            const index = this.adminStaffClassificationDescriptors.indexOf(item)
            this.adminStaffClassificationDescriptors.splice(index, 1)
        },

        onRemoveUserRoleTeacher(item) {
            const index = this.teacherStaffClassificationDescriptors.indexOf(item)
            this.teacherStaffClassificationDescriptors.splice(index, 1)
        },

        async onSaveUserRolesAdminClicked() {
            this.loadingDialog.open({ title: 'Saving Admin User Role Mappings' })

            try {
                await api.settings.saveUserRoleAdminMappings(this.adminStaffClassificationDescriptors)
                await timers.wait(1000)
                this.$snotify.success('Mappings saved', '', { timeout: 1500 })
            } finally {
                this.loadingDialog.close()
            }
        },

        async onSaveUserRolesTeacherClicked() {
            this.loadingDialog.open({ title: 'Saving Teacher User Role Mappings' })

            try {
                await api.settings.saveUserRoleTeacherMappings(this.teacherStaffClassificationDescriptors)
                await timers.wait(1000)
                this.$snotify.success('Mappings saved', '', { timeout: 1500 })
            } finally {
                this.loadingDialog.close()
            }
        },
    }, // methods
}
</script>
