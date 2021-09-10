/* eslint-disable */

import Vue from "vue"
import VueRouter from 'vue-router'
import { vuexOidcCreateRouterMiddleware } from "vuex-oidc"
import { store } from '@/store/store'
import OidcSilentRenew from '@/views/oidc/OidcSilentRenew'
import { applyRouteGuardsAsync } from "./guards/RouteGuardInterceptor"
import { edFiVersionGuard } from "./guards/EdFiVersionGuard"
import { userRoleMappingsGuard } from "./guards/UserRoleMappingsGuard"

Vue.use(VueRouter);

const featuresAssessmentsGuard = (to, from, next) => {
    console.log("featuresAssessmentsGuard");

    const featuresJsonStr = localStorage.getItem("edfi.features"); // TODO Use @/utils/session

    if (featuresJsonStr == null) {
        next();
    } else {
        const features = JSON.parse(featuresJsonStr);

        if (features.showAssessments == false)
            next("/not-authorized")
        else
            next();
    }
};

const featuresInterventionsGuard = (to, from, next) => {
    console.log("featuresInterventionsGuard");

    const featuresJsonStr = localStorage.getItem("edfi.features"); // TODO Use @/utils/session

    if (featuresJsonStr == null) {
        next();
    } else {
        const features = JSON.parse(featuresJsonStr);

        if (features.showInterventions == false)
            next("/not-authorized")
        else
            next();
    }
};

const router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,

    scrollBehavior() {
        return { x: 0, y: 0 };
    }, // scrollBehavior

    routes: [
        {
            path: "/",
            name: "page-loading",
            beforeEnter(to, from, next) {
                const isAuthenticated = store.getters["auth/oidcIsAuthenticated"]

                if (!isAuthenticated) {
                    next("/not-authorized")
                    return
                }

                next("/home")
            }
        },
        {
            path: "/login",
            name: "Login",
            component: () => import("@/views/Login.vue"),
        },
        {
            path: '/silent-renew',
            name: "OidcSilentRenew",
            component: OidcSilentRenew,
        },
        {
            path: '/edFiVersionSelection',
            component: () => import('@/views/EdFiVersionSelection.vue'),
        },
        {
            path: '/userEmailMappings',
            component: () => import('@/views/UserEmailMappings/UserEmailMappingsPage.vue'),
        },
        {
            path: '/userRoleMappings',
            component: () => import('@/views/UserRoleMappingsPage.vue'),
            beforeEnter: async (to, from, next) => await applyRouteGuardsAsync(to, from, next, [ edFiVersionGuard ]),
        },
        // TODO Is this route still being used?
        // {
        //     path: '/profileSelection',
        //     name: 'ProfileSelection',
        //     component: () => import("@/views/ProfileSelection.vue"),
        //     beforeEnter: async (to, from, next) => {
        //         await EdFiVersionGuard(to, from, next)
        //         await UserRoleMappingsGuard(to, from, next)
        //     },
        // },
        {
            path: "/select-tenant",
            name: "select-tenant",
            component: () => import("@bit/edgraph.shared.view-select-tenant"),
            meta: {
                layout: "full",
                isPublic: true
            }
        },
        {
            path: "/no-tenant",
            name: "no-tenant",
            component: () => import("@bit/edgraph.shared.view-no-tenant"),
            meta: {
                layout: "full",
                isPublic: true
            }
        },
        {
            path: "/not-authorized",
            name: "page-not-authorized",
            component: () => import("@bit/edgraph.shared.view-not-authorized"),
            meta: {
                layout: "full",
                isPublic: true
            }
        },
        {
            path: "/callback", // Needs to match redirect_uri in your oidcSettings
            name: "callback",
            component: () => import("@bit/edgraph.shared.view-callback"),
            meta: {
                isOidcCallback: true,
                isPublic: true,
                layout: "full"
            }
        },
        {
            path: "/silent-renew", // Needs to match redirect_uri in your oidcSettings
            name: "silent-renew",
            component: () => import("@bit/edgraph.shared.view-silent-renew"),
            meta: {
                isOidcCallback: true,
                isPublic: true,
                layout: "full"
            }
        },
        {
            path: "/404",
            name: "page-not-found",
            component: () => import("@bit/edgraph.shared.view-404"),
            meta: {
                layout: "full"
            }
        },
        {
            path: "*",
            name: "catch-all",
            redirect: "/404"
        },
        {
            path: "/",
            name: "MainContainer",
            component: () => import("@/views/MainContainer.vue"),
            beforeEnter: async (to, from, next) => await applyRouteGuardsAsync(to, from, next, [ edFiVersionGuard, userRoleMappingsGuard ]),
            children: [
                {
                    path: "/home",
                    name: "Home",
                    component: () => import("@/views/Home.vue"),
                },
                {
                    path: "/assesments",
                    name: "Assesments",
                    component: () => import("@/views/assesments/Assesments.vue"),
                    beforeEnter: featuresAssessmentsGuard,
                },
                {
                    path: "/assesments/add",
                    name: "AssesmentAdd",
                    component: () => import("@/views/assesments/AssesmentAdd.vue"),
                    beforeEnter: featuresAssessmentsGuard,
                },
                {
                    path: "/assesments/details/:id",
                    name: "AssesmentDetails",
                    component: () => import("@/views/assesments/AssesmentDetails.vue"),
                    beforeEnter: featuresAssessmentsGuard,
                },
                {
                    path: "/assesments/edit/:id",
                    name: "AssesmentEdit",
                    component: () => import("@/views/assesments/AssesmentDetails.vue"),
                    beforeEnter: featuresAssessmentsGuard,
                },
                {
                    path: "/interventions",
                    name: "Interventions",
                    component: () => import("@/views/interventions/Interventions.vue"),
                    beforeEnter: featuresInterventionsGuard,
                },
                {
                    path: "/interventions/details/:interventionId",
                    name: "InterventionsDetails",
                    component: () => import("@/views/interventions/InterventionsDetails.vue"),
                    beforeEnter: featuresInterventionsGuard,
                },
                {
                    path: "/interventions/edit/:interventionId",
                    name: "InterventionsEdit",
                    component: () => import("@/views/interventions/InterventionsDetails.vue"),
                    beforeEnter: featuresInterventionsGuard,
                },
                {
                    path: "/interventions/add",
                    name: "AddIntervention",
                    component: () => import("@/views/interventions/AddIntervention.vue"),
                    beforeEnter: featuresInterventionsGuard,
                },
                {
                    path: "/scoring/assessments",
                    name: "ScoringAssessments",
                    component: () => import("@/views/scoring/assessments/ScoringAssessments.vue"),
                    beforeEnter: featuresAssessmentsGuard,
                },
                {
                    path: "/scoring/interventions",
                    name: "ScoringInterventions",
                    component: () => import("@/views/scoring/interventions/ScoringInterventions.vue"),
                    beforeEnter: featuresInterventionsGuard,
                },
                {
                    path: "/studentProfile",
                    name: "StudentProfile",
                    component: () => import("@/views/StudentProfile.vue"),
                },
                {
                    path: "/teachersInterventionDetail",
                    name: "TeachersInterventionDetail",
                    component: () => import("@/views/TeachersInterventionDetail.vue"),
                },
                {
                    path: "/teachersAssessmentDetail",
                    name: "TeachersAssessmentDetail",
                    component: () => import("@/views/TeachersAssessmentDetail.vue"),
                },
                {
                    path: "/reporting",
                    name: "Reporting",
                    component: () => import("@/views/Reporting.vue"),
                },
                {
                    path: "/settings",
                    redirect: "/settings/home",
                    name: "Settings",
                    component: () => import("@/views/settings/Settings.vue"),
                    children: [
                        {
                            path: '/settings/home',
                            name: 'SettingsHome',
                            component: () => import('@/views/settings/SettingsHome.vue'),
                        },
                        {
                            path: '/settings/odsApiConnection',
                            name: 'OdsApiConnectionSettings',
                            component: () => import('@/views/settings/OdsApiConnectionSettings.vue'),
                        },
                        {
                            path: '/settings/userRoleMappings',
                            name: 'UserRoleMappingsSettings',
                            component: () => import('@/views/settings/UserRoleMappingsSettings.vue'),
                        },
                        {
                            path: '/settings/caching',
                            name: 'Caching',
                            component: () => import('@/views/settings/caching/Caching.vue'),
                        },
                        {
                            path: '/settings/features',
                            name: 'Features',
                            component: () => import('@/views/settings/Features/Features.vue'),
                        },
                    ],
                },
                {
                    path: "/settings/caching/teachers",
                    name: "Teachers",
                    component: () => import("@/views/settings/caching/teachers/Teachers.vue"),
                },
                {
                    path: "/settings/caching/students",
                    name: "Students",
                    component: () => import("@/views/settings/caching/students/Students.vue"),
                },
                {
                    path: "/settings/caching/schools",
                    name: "Schools",
                    component: () => import("@/views/settings/caching/schools/Schools.vue"),
                },
                {
                    path: "/settings/caching/user-profiles",
                    name: "UserProfiles",
                    component: () => import("@/views/settings/caching/user-profiles/UserProfiles.vue"),
                },
                {
                    path: "/settings/caching/sections-by-staff",
                    name: "Sections by Staff",
                    component: () => import("@/views/settings/caching/sections-by-staff/SectionsByStaff.vue"),
                },
                {
                    path: "/settings/caching/staffs-by-school",
                    name: "Staffs by School",
                    component: () => import("@/views/settings/caching/staffs-by-school/StaffsBySchool.vue"),
                },
                {
                    path: "/settings/caching/sections",
                    name: "Sections",
                    component: () => import("@/views/settings/caching/sections/Sections.vue"),
                },
                {
                    path: "/settings/caching/students-by-section",
                    name: "StudentsBySection",
                    component: () => import("@/views/settings/caching/students-by-section/StudentsBySection.vue"),
                },
            ],
        },
    ], // routes
});

router.beforeEach(vuexOidcCreateRouterMiddleware(store, 'auth'));

router.afterEach(() => {
    // Remove initial loading
    const appLoading = document.getElementById("loading-bg")
    if (appLoading) {
        appLoading.style.display = "none"
    }
});

export default router
