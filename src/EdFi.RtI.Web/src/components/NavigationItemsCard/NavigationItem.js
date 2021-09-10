import environment from "@/environment";

export class NavigationItem {
    icon = '';
    title = '';
    type = '';
    path = '';
}

const SettingsOdsApiConnectionNavItem = new NavigationItem()
SettingsOdsApiConnectionNavItem.icon = 'mdi-connection'
SettingsOdsApiConnectionNavItem.title = 'ODS API Connection'
SettingsOdsApiConnectionNavItem.path = '/settings/odsApiConnection'

const SettingsUserRoleMappingsItem = new NavigationItem()
SettingsUserRoleMappingsItem.icon = 'mdi-account-switch'
SettingsUserRoleMappingsItem.title = 'User Role Mappings'
SettingsUserRoleMappingsItem.path = '/settings/userRoleMappings'

const SettingsCachingNavItem = new NavigationItem()
SettingsCachingNavItem.icon = 'mdi-database'
SettingsCachingNavItem.title = 'Caching'
SettingsCachingNavItem.path = '/settings/caching'

const SettingsFeaturesNavItem = new NavigationItem()
SettingsFeaturesNavItem.icon = 'mdi-star'
SettingsFeaturesNavItem.title = 'Features'
SettingsFeaturesNavItem.path = '/settings/features'

export const SettingsNavItemsHostedMode = [
    SettingsOdsApiConnectionNavItem,
    SettingsUserRoleMappingsItem,
    SettingsCachingNavItem,
    SettingsFeaturesNavItem,
]

export const SettingsNavItemsStandaloneMode = [
    SettingsCachingNavItem,
    SettingsFeaturesNavItem,
]

export function getSettingsNavItemsForCurrentStartupMode() {
    if (environment.app.isStartupModeHosted())
        return SettingsNavItemsHostedMode

    if (environment.app.isStartupModeStandalone())
        return SettingsNavItemsStandaloneMode

    throw `getSettingsNavItemsForCurrentStartupMode has not been implemented for StartupMode \"${environment.app.getStartupMode()}\"`;
}
