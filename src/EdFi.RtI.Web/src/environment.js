import vue from 'vue'

export default {
    app: {
        getStartupMode: () => vue.prototype.$config.app.startupMode,

        isStartupModeHosted() {
            return vue.prototype.$config.app.startupMode == 'Hosted'
        },

        isStartupModeStandalone() {
            return vue.prototype.$config.app.startupMode == 'Standalone'
        },
    },

    mode: {
        get: () => process.env.NODE_ENV,
        isLocal: () => process.env.NODE_ENV == 'local',
    },
}
