<template>
    <div>
        <NavigationItemsCard :navItems="navItems"/>
    </div>
</template>

<script>
import { getSettingsNavItemsForCurrentStartupMode } from '@/components/NavigationItemsCard/NavigationItem'
export default {
    data: () => ({
        navItems: getSettingsNavItemsForCurrentStartupMode(),
    }), // data
}
</script>
