import api from "@/api/index";
import environment from "@/environment";
import storage from "@/storage/index"
import { isNullOrEmpty } from "@/utils/validators";
import { Routes } from "../Routes";

/**
 * @param {import('node_modules/vue-router/types/index').Route} to
 * @param {import('node_modules/vue-router/types/index').Route} from
 * @param {import('node_modules/vue-router/types/index').NavigationGuardNext} next
 * @returns {Promise<Boolean>} true if the guard can continue, false otherwise
 */
export async function userRoleMappingsGuard(to, from, next) {
    if (environment.app.isStartupModeStandalone()) {
        next()
        return true
    }

    const adminMappings = await api.settings.getUserRoleAdminMappings()
    storage.local.setUserRoleAdminMappings(adminMappings)
    if (isNullOrEmpty(adminMappings)) {
        next(Routes.static.userRoleMappings)
        return false
    }
    
    const teacherMappings = await api.settings.getUserRoleTeacherMappings()
    storage.local.setUserRoleTeacherMappings(teacherMappings)
    if (isNullOrEmpty(teacherMappings)) {
        next(Routes.static.userRoleMappings)
        return false
    }

    next()
    return true
}
