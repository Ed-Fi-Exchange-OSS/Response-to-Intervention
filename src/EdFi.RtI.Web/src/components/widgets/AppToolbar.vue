<template>
    <div>
        <v-app-bar>
            <div class="d-flex flex-row align-center mx-2" v-if="!isSearching">

                <v-icon class="mr-4">{{icon}}</v-icon>

                <v-toolbar-title>{{title}}</v-toolbar-title>
                
                <v-btn  class="ml-2 mr-2" small icon @click="isSearching = !isSearching" v-if="showSearch">
                    <v-icon>mdi-magnify</v-icon>
                </v-btn>
                <v-chip
                    v-if="isSearchInputVisible"
                    close
                    @click:close="clearSearchInput"
                    >
                    {{searchInput}}
                </v-chip>
            </div>

            <div v-if="isSearching" style="width: 100%">
                <v-text-field
                    v-model="searchInput"
                    solo
                    hide-details
                    class="px-2 pt-3 pb-3"
                    style="width: 100%"
                    placeholder="Search assessments..."
                    prepend-icon="mdi-chevron-left"
                    append-icon="mdi-close"
                    @click:prepend="isSearching = false"
                    @click:append="clearSearchInput"
                    @change="sentInput"/>
            </div><!-- search bar -->

            <v-spacer></v-spacer>

            <!-- buttons -->
            <div v-for="button in buttons" :key="button.text">

                <div class="d-flex flex-column align-center mx-2" 
                    :class="{ 'cursor-pointer': !disabledButtons && !isButtonDisabled(button) }">
                    <v-btn small icon @click="button.click" :disabled="disabledButtons == true || isButtonDisabled(button)">
                        <v-icon>{{button.icon}}</v-icon>
                    </v-btn>
                    <p class="ma-0" style="font-size: 12px">{{button.text}}</p>
                </div>
            </div><!-- buttons -->

        </v-app-bar><!-- toolbar -->
    </div>
</template>

<script>
export default {
    props: { 
        title: String, 
        icon: String, 
        buttons: Array,
        showSearch: Boolean,
        disabledButtons: Boolean,
    },

    data: () => ({
        isSearching: false,
        searchInput: '',
    }), // data

    computed: {
        isSearchInputVisible(){
            return this.searchInput && this.searchInput.trim().length > 0;
        },
    },

    methods: {
        clearSearchInput(){
            this.searchInput = null;
            this.sentInput();
        },

        isButtonDisabled(button) {
            return button.disabled && button.disabled() == true;
        },

        sentInput(){
            this.$emit('input', this.searchInput);
        },
    },
}
</script>