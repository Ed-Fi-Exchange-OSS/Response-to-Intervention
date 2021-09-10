<template>
    <v-card>
        <v-card-title class="d-flex justify-center">
            <v-icon class="mr-2" x-large>{{icon}}</v-icon>
            <h2 class="font-weight-regular">{{title}}</h2>
        </v-card-title>

        <v-divider></v-divider>

        <v-card-text class="pa-0">
            
            <!-- error -->
            <div v-if="error" class="py-16">

                <p class="text-center ma-0">
                    <v-icon size="64">mdi-alert-outline</v-icon>
                </p>

                <h3 class="text-center mb-4">An error has occurred!</h3>

                <p class="text-center">{{errorMessage}}</p>

                <div class="d-flex justify-center">
                    <v-btn small color="primary" class="text-capitalize" @click="onTryAgainClicked">Try Again</v-btn>
                </div>

            </div><!-- error -->

            <!-- no error -->
            <div v-else>

                <!-- loading profiles -->
                <div v-if="loading" class="py-16">
                    <div class="d-flex justify-center">
                        <p>{{loadingMessage}}</p>
                    </div>
                    <div class="d-flex justify-center">
                        <v-progress-circular indeterminate/>
                    </div>
                </div><!-- loading profiles -->

                <!-- profiles loaded -->
                <div v-else>

                    <!-- no results -->
                    <div v-if="isEmpty" class="py-16">
                        <p class="text-center ma-0">{{emptyMessage}}</p>
                    </div><!-- no results -->

                    <!-- show results -->
                    <div v-else>
                        <div v-for="(profile, index) of users" :key="index">
                            <v-list-item @click="onProfileSelected(profile)">
                                <v-list-item-content :class="{ 'py-5': !hasSchool(profile) }"> <!-- add some padding if user does not have school (to make height look alike to the rest of the items) -->
                                    <v-list-item-title>{{getUserFullName(profile)}} | {{getUserRole(profile)}}</v-list-item-title>
                                    <v-list-item-subtitle v-if="hasSchool(profile)">{{getUserSchool(profile)}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                            <v-divider v-if="index < users.length - 1"/>
                        </div>
                    </div><!-- show results -->

                </div><!-- profiles loaded -->

            </div><!-- no error -->

        </v-card-text>
    </v-card>
</template>

<script>
export default {
    
    props: {
        emptyMessage: {
            type: String,
            default: "No users found",
        },
        error: {
            type: Boolean,
            default: false,
        },
        errorMessage: {
            type: String,
            default: "An error ocurred",
        },
        icon: {
            type: String,
            default: "mdi-person",
        },
        loading: {
            type: Boolean,
            default: false,
        },
        loadingMessage: {
            type: String,
            default: "Loading",
        },
        title: {
            type: String,
            default: "Users",
        },
        users: {
            type: Array,
            required: true,
        },
    }, // props

    computed: {
        isEmpty() {
            return this.users.length == 0;
        },
    }, // computed

    methods: {

        hasSchool(profile) {
            const school = profile.school;

            if (!school)
                return false;
            
            const schoolName = school.nameOfInstitution;

            if (!schoolName)
                return false;

            if (schoolName == 'Unkown')
                return false;
            
            return schoolName;
        },

        onProfileSelected(selectedProfile) {
            this.$emit('profile-selected', selectedProfile);
        },

        onTryAgainClicked() {
            this.$emit('try-again');
        },

        getUserFullName(profile) {
            return profile.staff.fullName;
        },

        getUserRole(profile) {
            return profile.staffEducationOrganizationAssignmentAssociation.staffClassificationDescriptor;
        },

        getUserSchool(profile) {
            return profile.school.nameOfInstitution;
        },

    }, // methods
}
</script>
