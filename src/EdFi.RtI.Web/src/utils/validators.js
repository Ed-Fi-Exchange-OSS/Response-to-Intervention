export function isNull(obj) {
    return obj == undefined || obj == null
}

/**
 * @param {String} str 
 */
export function isNullOrWhitespace(str) {
    return isNull(str) || str.trim().length == 0
}

/**
 * @param {any[]} array 
 * @returns 
 */
export function isNullOrEmpty(array) {
    return isNull(array) || array.length == 0
}

export function notNull(obj) {
    return !isNull(obj)
}

/**
 * @param {String} str 
 */
export function notNullNorWhitespace(str) {
    return notNull(str) && str.trim().length != 0
}

/**
 * @param {any[]} array 
 */
export function notNullNorEmpty(array) {
    return notNull(array) && array.length != 0;
}
