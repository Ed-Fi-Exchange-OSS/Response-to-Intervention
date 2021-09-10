export default {
    /**
     * @param {Number} miliseconds 
     * @returns {Promise<void>}
     */
    wait(miliseconds) {
        return new Promise(resolve => {
            setTimeout(() => resolve(), miliseconds)
        })
    }
}
