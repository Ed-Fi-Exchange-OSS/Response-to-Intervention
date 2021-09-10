// vuex-oidc : https://www.npmjs.com/package/vuex-oidc

import { WebStorageStateStore } from "oidc-client"
import { vuexOidcCreateStoreModule } from "vuex-oidc"

export default function (store, config) {
    const authConfig = {
        userStore: new WebStorageStateStore({
            store: window.sessionStorage
        }),
        ...config.auth
    }

    store.registerModule("auth", vuexOidcCreateStoreModule(
        authConfig,
        // Optional OIDC store settings
        {
            namespaced: true,
            routeBase: config.app.basePath,
            dispatchEventsOnWindow: true
        },
        // Optional OIDC event listeners
        {
            userLoaded: (user) => {
                sessionStorage.setItem('edgraph.oidcUser', JSON.stringify(user)); // TODO Maybe move this to session store
            },
            userUnloaded: () => {
                sessionStorage.removeItem('edgraph.oidcUser'); // TODO Maybe move this to session store
            },
            accessTokenExpiring: () => console.log('Access token will expire'),
            accessTokenExpired: () => console.log('Access token did expire'),
            silentRenewError: () => console.log('OIDC user is unloaded'),
            userSignedOut: () => {
                sessionStorage.removeItem('edgraph.oidcUser'); // TODO Maybe move this to session store
            },
            oidcError: (payload) => console.log('oidcError callback', payload),
            automaticSilentRenewError: (payload) => console.log('OIDC automaticSilentRenewError', payload)
        }
    ))
}
