import axios from "axios"
import get from "lodash/get"
import Vue from "vue"

const showResponseErrorLogs = true   // Wether the app should display the error logs caught by the response interceptor
const errorToastTimeout = 2000

axios.defaults.baseURL = `${Vue.prototype.$config.api.baseUri}`

axios.interceptors.request.use((request) => {
  setAuthorizationHeader(request)
  setContentType(request)
  return request
})

axios.interceptors.response.use(
  (success) => {
    return Promise.resolve(success)
  },
  (error) =>  {
    console.log(error?.config?.url ?? '')

    if (get(error, "response.status") == 401) {
      // TODO What to do in this case?
      // if (!router.currentRoute.meta.isPublic) {
      sessionStorage.clear()
      //window.location.href = "/not-authorized" // TODO Why is this here? It was causing not-authorized looping errors
      // }
    }
    else {
      const requestUrl = (error && error.config) ? `${error.config.baseURL}${error?.config?.url ?? ""}` : null
      const method = (error && error.config) ? error.config.method.toUpperCase() : null
      const showResponseErrorToasts = Vue.prototype.$config.app.toastErrors == "yes"

      if (showResponseErrorLogs) {
        if (requestUrl && method) {
          console.log(`%c${method} | ${requestUrl}`, "color: yellow; font-weight:bold;")
          console.log(`%c${error.name} - ${error.message}`, "color: yellow;")
        }

        const errorJson = JSON.parse(JSON.stringify(error))
        console.log("Error caught in interceptor:", errorJson)
      }

      if (showResponseErrorToasts) {
        if (requestUrl && method) {
          Vue.prototype.$snotify.error(`${method} ${error?.config?.url ?? ''} ${error.name} ${error.message}`, {
            bodyMaxLength: 1000,
            icon: false,
            timeout: errorToastTimeout,
            titleMaxLength: 1000
          })
        }
      }
      
      return Promise.reject(error)
    }
  }
)

/**
 * @param {import("node_modules/axios/index").AxiosRequestConfig} request 
 */
function setAuthorizationHeader(request) {
  const accessToken = getAccessToken()

  if (accessToken == undefined || accessToken == null || accessToken.trim().length == 0)
    return
  
  request.headers['Authorization'] = `Bearer ${accessToken}`
}

/**
 * @param {import("node_modules/axios/index").AxiosRequestConfig} request 
 */
function setContentType(request) {
  request.headers['Content-Type'] = 'application/json; charset=utf-8'
}

/**
 * @returns {String}
 */
function getAccessToken() {
  const oidcUserStr = sessionStorage.getItem("edgraph.oidcUser")

  if (oidcUserStr == undefined || oidcUserStr == null || oidcUserStr.trim().length == 0)
    return null
  
  const oidcUser = JSON.parse(oidcUserStr)

  return oidcUser.access_token
}
