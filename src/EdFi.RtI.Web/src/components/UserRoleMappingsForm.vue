<template>
    <div>

        <div>
            <v-combobox
                clearable multiple solo chips hide-details
                class="mb-4"
                placeholder="Select the Staff Classification Descriptors you would like to map"
                :items="staffClassificationDescriptorsCodes"
                :loading="loading"
                v-model="mappedStaffClassificationDescriptors"
                @change="onMappedStaffClassificationDescriptorsChange">
                <template v-slot:selection="{ attrs, item, select, selected }">
                    <v-chip
                        v-bind="attrs"
                        :input-value="selected"
                        close
                        @click="select"
                        @click:close="onRemoveMappedRole(item)">
                    <strong>{{ item }}</strong>&nbsp;
                </v-chip>
                </template>
            </v-combobox>
            <v-layout justify-end>
                <v-btn color="primary" :disabled="loading || !isMappedRolesValid" @click="onSaveClicked">Save</v-btn>
            </v-layout>
        </div>

        <LoadingDialog ref="loadingDialog"/>
    </div>
</template>

<script>
import api from '@/api'

export default {

    data: () => ({
        loading: false,
        staffClassificationDescriptors: [],
        mappedStaffClassificationDescriptors: [],
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

        isMappedRolesValid() {
            return this.mappedStaffClassificationDescriptors.length > 0
        },
    }, // computed

    methods: {
        async init() {
            this.loading = true

            try {
                this.staffClassificationDescriptors = await api.descriptors.getStaffClassificationDescriptors()
            } finally {
                this.loading = false
            }
        },

        onMappedStaffClassificationDescriptorsChange() {
            const staffClassificationDescriptors = new Set(this.staffClassificationDescriptorsCodes)
            
            for (let i = 0; i < this.mappedStaffClassificationDescriptors.length; i++) {
                const descriptor = this.mappedStaffClassificationDescriptors[i]

                if (!staffClassificationDescriptors.has(descriptor))
                    this.mappedStaffClassificationDescriptors.splice(i, 1)
            }
        },

        onRemoveMappedRole(item) {
            const index = this.mappedStaffClassificationDescriptors.indexOf(item)
            this.mappedStaffClassificationDescriptors.splice(index, 1)
        },

        async onSaveClicked() {
            this.$emit('save', this.mappedStaffClassificationDescriptors)
        },
    }, // methods
}
</script>
