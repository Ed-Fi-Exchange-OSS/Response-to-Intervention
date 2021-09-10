import { NavigationItem } from "@/components/NavigationItemsCard/NavigationItem";

export const ScoringHeadingNavItem = {
    title: 'Scoring',
    type: 'heading',
};

export const ScoringAssessmentsNavItem = {
    icon: 'mdi-view-dashboard',
    title: 'Assessments',
    path: '/scoring/assessments',
};

export const ScoringInterventionsNavItem = {
    icon: 'mdi-text-box-check',
    title: 'Interventions',
    path: '/scoring/interventions',
};

export const AdministrationHeadingNavItem = {
    title: 'Administration',
    type: 'heading',
};

export const ManageAssessmentsNavItem = {
    icon: 'mdi-view-dashboard',
    title: 'Manage Assessments',
    path: '/assesments',
};

export const ManageInterventionsNavItem = {
    icon: 'mdi-text-box-check',
    title: 'Manage Interventions',
    path: '/interventions',
};

export const SettingsNavItem = {
    icon: 'mdi-cogs',
    title: 'Settings',
    path: '/settings',
};

const SettingsOdsApiConnectionNavItem = new NavigationItem()
SettingsOdsApiConnectionNavItem.icon = 'mdi-connection'
SettingsOdsApiConnectionNavItem.title = 'ODS API Connection'



