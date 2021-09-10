<template>
<!--eslint-disable-->
    <v-container>
        <v-card>
            <v-card-text>

                <div v-if="!hasNavItems" class="my-12">
                    <h3 class="font-weight-regular text-center ma-0">Looks like there aren't any places to navigate to!</h3>
                </div>

                <div v-if="hasNavItems" class="d-flex flex-wrap">
                    <div v-for="item of navItems" :class="{ 'app-w-100': item.type == 'heading' }">

                        <div v-if="item.type == 'heading'" class="mb-4">
                            <h2 class="font-weight-regular mb-3">{{item.title}}</h2>
                            <hr style="border: 1px dashed lightgray">
                        </div>

                        <div v-else @click="$router.push(item.path)" style="width: 150px" class="mx-4 my-6">
                            <div class="app-cursor-pointer app-scale-hover app-transition-ease">
                                <div class="app-center-x">
                                    <v-icon style="font-size: 80px">{{item.icon}}</v-icon>
                                </div>
                                <h3 class="font-weight-regular text-center ma-0">{{item.title}}</h3>
                            </div>
                        </div>
                    </div>
                </div>                

            </v-card-text>
        </v-card>
    </v-container>
</template>

<script>
/* eslint-disable */

import { mapGetters, mapState } from 'vuex';

export default {
    
    data: () => ({
        
    }),
    
    mounted() {
        console.log("this.navItems:", this.navItems);
    },
    
    computed: {
        ...mapState({
            navItems: "sidenavItems",
        }),

        hasNavItems() {
            return this.navItems != undefined && this.navItems != null && this.navItems.length > 0;
        }
    },
    
    methods: {
        
    },
    
    components: {
        
    },
}
</script>
