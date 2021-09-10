<template>
    <v-card>
        <v-card-text>

            <div v-if="!hasNavItems" class="my-12">
                <h3 class="font-weight-regular text-center ma-0">Looks like there aren't any places to navigate to!</h3>
            </div>
            
            <!-- nav items container -->
            <div v-if="hasNavItems" class="d-flex align-center flex-wrap my-6">

                <div v-for="(item, index) of navItems" :key="index"
                    class="app-cursor-pointer app-scale-hover app-transition-ease"
                    style="width: 200px;"
                    @click="$router.push(item.path)">
                    <v-layout justify-center>
                        <v-icon style="font-size: 70px">{{item.icon}}</v-icon>
                    </v-layout>
                    <h3 class="font-weight-regular text-center ma-0 mt-4">{{item.title}}</h3>
                </div>

            </div><!-- nav items container -->

        </v-card-text>
    </v-card>
</template>

<script>

export default {
    props: {
        navItems: {
            type: Array,
        },
    }, // props

    data: () => ({ }), // data

    computed: {
        hasNavItems() {
            return this.navItems != undefined && this.navItems != null && this.navItems.length != 0
        },
    }, // computed
}
</script>
