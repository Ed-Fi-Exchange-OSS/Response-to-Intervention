<template>
  <div>
    <v-container>

      <Toolbar class="mb-5" icon="mdi-cogs" title="Settings" :showSearch="false"/>

      <v-tabs class="mb-5 elevation-1">
        <v-tab class="text-capitalize" to="/settings/home">Home</v-tab>
        <v-tab v-for="(item, index) of navItems" :key="index" class="text-capitalize" :to="item.path">{{item.title}}</v-tab>
      </v-tabs>

      <router-view></router-view>

    </v-container>
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
