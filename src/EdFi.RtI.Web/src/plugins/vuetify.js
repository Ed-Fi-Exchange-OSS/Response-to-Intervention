import Vue from "vue"
import Vuetify from "vuetify"
import "vuetify/dist/vuetify.min.css"
import en from "vuetify/es5/locale/en"
import fr from "vuetify/es5/locale/fr"

Vue.use(Vuetify)

export default new Vuetify({
  lang: {
    current: "en",
    locales: {
      en,
      fr
    }
  },
  theme: {
    options: {
      customProperties: true
    },
    dark: JSON.parse(localStorage.getItem("darkMode")),
    themes: {
      light: {
        primary: "#1D3557",
        secondary: "#464646",
        success: "#28C76F",
        accent: "#EF3D23",
        error: "#f00",
        info: "#2196F3",
        // warning: "#FFAB00",
        bg: "#f2f2f2"
      },
      dark: {
        primary: "#1D3557",
        secondary: "#464646",
        success: "#28C76F",
        accent: "#EF3D23",
        error: "#f00",
        info: "#2196F3"
        // warning: "#FFAB00"
      }
    }
  }
})
