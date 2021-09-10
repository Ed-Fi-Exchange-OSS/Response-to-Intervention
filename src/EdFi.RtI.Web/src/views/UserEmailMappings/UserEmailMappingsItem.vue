<template>
    <v-card>
        <v-card-text>
            
            <v-text-field filled label="Email To Map" v-model="item.email"/>

            <v-combobox
                clearable multiple solo chips hide-details
                label="Mapped Emails"
                v-model="item.mappedEmails">
                <template v-slot:selection="{ attrs, item, select, selected }">
                <v-chip
                    v-bind="attrs"
                    :input-value="selected"
                    close
                    @click="select"
                    @click:close="remove(item)">
                    <strong>{{ item }}</strong>&nbsp;
                    <span>(interest)</span>
                </v-chip>
                </template>
            </v-combobox>

        </v-card-text>

        <v-card-actions>
            <v-spacer/>
            <v-btn color="error" @click="onRemoveClicked">
                Remove
            </v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
export default {
    props: {
        item: {
            type: Object,
            required: true,
        },
    }, // props

    methods: {
        onRemoveClicked() {
            this.$emit('remove')
        }
    }, // methods
}
</script>
