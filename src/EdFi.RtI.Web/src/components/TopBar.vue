<template>
    <div>
        <v-app-bar class="px-2" color="#1D3557" dark>

            <v-toolbar-title>{{title}}</v-toolbar-title>

            <v-spacer/>

            <v-menu bottom rounded offset-y>
                <template v-slot:activator="{ on }">
                    <v-btn icon v-on="on">
                    <v-avatar color="accent">
                        <v-icon>mdi-account</v-icon>
                    </v-avatar>
                    </v-btn>
                </template>
                <v-card>
                    <v-list dense>
                    <v-list-item v-for="(item, index) of dropdownItems" :key="index"
                        dense @click="item.click">
                        <v-list-item-action>
                            <v-icon>{{item.icon}}</v-icon>
                        </v-list-item-action>
                        <v-list-item-title>
                            <h4 class="font-weight-regular">{{item.label}}</h4>
                        </v-list-item-title>
                    </v-list-item>
                    </v-list>
                </v-card>
            </v-menu>

        </v-app-bar>

        <LoadingDialog ref="loadingDialog" />
    </div>
</template>

<script>
import timers from '@/utils/timers'
import { mapActions } from 'vuex'

export default {

    props: {
        title: {
            type: String,
            default: '',
        },
    }, // props

    data: (vue) => ({
        dropdownItems: [
            {
                icon: 'mdi-logout',
                label: 'Sign Out',
                click: () => vue.onSignOutClicked(),
            },
        ],
    }), // data

    computed: {
        loadingDialog() {
            return this.$refs.loadingDialog
        },
    }, // computed

    methods: {
        ...mapActions({
            signOutOidc: 'auth/signOutOidc',
        }),

        async onSignOutClicked() {
            this.loadingDialog.open({ title: "Signing out" })
            await timers.wait(1000)
            await this.signOutOidc()
        },
    }, // methods

}
</script>
