let debug = true

/**
 * @param {import('node_modules/vue-router/types/index').Route} to
 * @param {import('node_modules/vue-router/types/index').Route} from
 * @param {import('node_modules/vue-router/types/index').NavigationGuardNext} next
 * @param {Function[]} guards
 */
export async function applyRouteGuardsAsync(to, from, next, guards) {
    if (debug) {
        console.log("applyRouteGuardsAsync");
        console.log("guards:", guards);
    }

    for (const guard of guards) {
        if (debug)
            console.log("Applying guard", guard);
        
        const canContinue = await guard(to, from, next)

        if (debug)
            console.log("canContinue:", canContinue);
        
        if (canContinue != true) {
            if (debug)
                console.log("Route guard failed :(");
            return
        }
    }

    if (debug)
        console.log("All guards succeeded!");
}
