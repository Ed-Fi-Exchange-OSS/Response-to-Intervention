<template>
    <div>

        <SharedTopBar v-if="isStartupModeHosted" title="Select a Profile" :hideLauncher="true"/>
        <TopBar v-if="isStartupModeStandalone" title="Select a Profile"/>

        <div v-if="isStartupModeHosted" style="height: 65px"></div>

        <v-container class="py-8">
            <v-row>
                <v-col>
                    <UserList
                        :emptyMessage="'No admins found'"
                        :error="errorUserAdmins"
                        :errorMessage="errorMessageAdmins"
                        :icon="'mdi-account-hard-hat'"
                        :loading="isLoadingUserAdmins"
                        :loadingMessage="'Loading Admins'"
                        :title="'Administrators'"
                        :users="userAdmins"
                        @profile-selected="onProfileSelected"
                        @try-again="onTryAgainAdminsClicked"
                        />
                </v-col>

                <v-col>
                    <UserList
                        :emptyMessage="'No teachers found'"
                        :error="errorUserTeachers"
                        :errorMessage="errorMessageTeachers"
                        :icon="'mdi-account-tie'"
                        :loading="isLoadingUserTeachers"
                        :loadingMessage="'Loading Teachers'"
                        :title="'Teachers'"
                        :users="userTeachers"
                        @profile-selected="onProfileSelected"
                        @try-again="onTryAgainTeachersClicked"
                        />
                </v-col>
            </v-row>
        </v-container>

        <LoadingDialog ref="loadingDialog" />
    </div>
</template>

<script>
import { mapActions, mapGetters, mapMutations, mapState } from 'vuex';
import LoadingDialog from "@/components/dialogs/LoadingDialog";
import UserList from "@/components/widgets/UserList";
import navItemsGetter from "@/utils/navItemsGetter";
import storage from '@/storage';
import environment from '@/environment';

export default {
    data: () => ({
        errorMessageAdmins: "Could not get admins",
        errorMessageTeachers: "Could not get teachers",
        errorUserAdmins: false,
        errorUserTeachers: false,
        isLoadingUserAdmins: false,
        isLoadingUserTeachers: false,
        userAdmins: [],
        userTeachers: [],
    }),

    computed: {
        ...mapGetters({
            oidcUser: 'auth/oidcUser',
            oidcIdToken: 'auth/oidcIdToken',
            oidcAccessToken: 'auth/oidcAccessToken',
            currentProfile: "session/getProfile",
        }),

        ...mapState([
            "selectedTenantId",
        ]),

        isDemo() {
            return this.$config.app.isDemo;
        },

        isStartupModeHosted() {
            return environment.app.isStartupModeHosted()
        },

        isStartupModeStandalone() {
            return environment.app.isStartupModeStandalone()
        },
    }, // computed

    mounted() {
        if (this.isDemo) {
            this.getUserAdminsRequest();
            this.getUserTeachersRequest();
        } else {
            this.getUserByEmailRequest();
        }
    }, // mounted

    methods: {
        ...mapMutations({
            setSelectedTenantId: 'setSelectedTenantId',
            setNavItems: "setNavItems",
        }),

        ...mapActions({
            getUserSessionProfileByEmail: 'session/getUserSessionProfileByEmail',
            getAdminUsers: 'session/getAdminUsers',
            getTeacherUsers: 'session/getTeacherUsers',
            signIn: 'session/signIn',
            getFeaturesSettings: "features/getFeaturesSettings",
        }),

        getUserAdminsRequest() {
            this.isLoadingUserAdmins = true;
            this.errorUserAdmins = false;

            this.getAdminUsers()
            .then(result => {
                if (!Array.isArray(result.data)) { // Did not get users. Handle error
                    this.errorUserAdmins = true;
                    this.errorMessageAdmins = result.data;
                    return;
                }

                this.userAdmins = result.data;
            })
            .catch(error => {
                this.errorUserAdmins = true;
            })
            .finally(() => this.isLoadingUserAdmins = false);
        },

        getUserTeachersRequest() {
            this.isLoadingUserTeachers = true;
            this.errorUserTeachers = false;

            this.getTeacherUsers()
            .then(result => {
                if (!Array.isArray(result.data)) { // Did not get users. Handle error
                    this.errorUserTeachers = true;
                    this.errorMessageTeachers = result.data;
                    return;
                }

                this.userTeachers = result.data;
            })
            .catch(error => {
                this.errorUserTeachers = true;
            })
            .finally(() => this.isLoadingUserTeachers = false);
        },

        getUserByEmailRequest() {
            this.$refs.loadingDialog.open({ title: "Loading User Profile" });

            let email = this.oidcUser.email;

            if (this.isDemo)
                email = 'barrytanner@edfi.org';
            
            this.getUserSessionProfileByEmail(email)
            .then(result => this.onProfileSelected(result.data))
            .catch(() => this.$router.push('/not-authorized'))
            .finally(() => this.$refs.loadingDialog.close());
        },

        onProfileSelected(selectedProfile) {
            this.$refs.loadingDialog.open({ title: "Starting App" });
            storage.local.setUserProfile(selectedProfile)
            this.signIn(selectedProfile).then(() => this.setNavItemsRequest());
        },

        onNavItemsSet() {
            
        },

        onTryAgainAdminsClicked() {
            this.getUserAdminsRequest();
        },

        onTryAgainTeachersClicked() {
            this.getUserTeachersRequest();
        },

        async setNavItemsRequest() {
            const navItems = await navItemsGetter();
            this.setNavItems(navItems);
            this.$refs.loadingDialog.close();
            this.$router.push('/home');
        },
    },

    components: {
        LoadingDialog,
        UserList,
	}, // components
}
</script>
