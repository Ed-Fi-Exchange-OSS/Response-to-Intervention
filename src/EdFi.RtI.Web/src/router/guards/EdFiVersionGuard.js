import api from '@/api/index';
import environment from '@/environment';
import storage from '@/storage/index';
import { notNullNorWhitespace } from '@/utils/validators';
import { Routes } from '../Routes';

/**
 * @param {import('node_modules/vue-router/types/index').Route} to
 * @param {import('node_modules/vue-router/types/index').Route} from
 * @param {import('node_modules/vue-router/types/index').NavigationGuardNext} next
 * @type {Promise<Boolean>}
 * @returns {Promise<Boolean>} true if the guard can continue, false otherwise
 */
export async function edFiVersionGuard(to, from, next) {
    if (environment.app.isStartupModeStandalone()) {
        next()
        return true
    }

    const edFiVersion = await api.settings.getDefaultEdFiVersion()
    console.log("edFiVersion:", edFiVersion);
    const hasEdFiVersion = notNullNorWhitespace(edFiVersion)
    
    if (hasEdFiVersion) {
        console.log("Has edfi version!");
        storage.local.setEdFiVersion(edFiVersion)
        next()
        return true
    }
    
    console.log("Does not have edfi version!");
    storage.local.removeEdFiVersion()
    next(Routes.static.edFiVersionSelection)
    return false
}
