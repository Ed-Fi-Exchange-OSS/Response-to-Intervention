import Vue from "vue"
import App from "./App.vue"
import router from "./router/router"
import Snotify, { SnotifyPosition } from "vue-snotify"
import "vue-snotify/styles/material.css"
import vuetify from "./plugins/vuetify"
import VuetifyConfirm from "vuetify-confirm"
import { store } from "./store/store"
import lodash from "lodash"
import loadAuth from "./store/modules/auth.store"
import '@/components'
import "@bit/edgraph.shared.styles-main" // global styles
import "typeface-open-sans"              // global font
import IdleVue from "idle-vue"

// compare the version when load in browser
require("./plugins/version")
Vue.config.productionTip = false

Vue.use(Snotify, {
  toast: {
    position: SnotifyPosition.rightTop
  }
})

const eventsHub = new Vue()

Vue.use(IdleVue, {
  eventEmitter: eventsHub,
  store,
  idleTime: 1000 * 60 * 15,
  startAtIdle: false
})

Vue.use(VuetifyConfirm, {
  vuetify,
  color: "primary",
  title: "primary"
})

const configFile = 'config.json'
const configFilePath = `${process.env.BASE_URL}${configFile}`
console.log("configFilePath:", configFilePath)

fetch(configFilePath)
  .then((data) => {
    return data.json()
  })
  .then((config) => {
    console.log("App Config:", config)

    Vue.prototype.$config = config
    require("./plugins/axios.plugin")

    loadAuth(store, config)

    new Vue({
      router,
      store,
      vuetify,

      beforeMount() {
        Vue.prototype.$lodash = lodash
        Vue.prototype.$vuetify = vuetify.framework // FIXME why do we need to do this, this is already there
        Vue.prototype.$t = (arg) => arg // TODO What is $t? (needed for SideNav shared component)
        // ANS: it is used by vue-i18n for translations. you can add that as well `vue add i18n`
      },

      render: (h) => h(App)

    }).$mount("#app")
  })
