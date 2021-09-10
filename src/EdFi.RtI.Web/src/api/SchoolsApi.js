import axios from 'axios'

export default {

    /**
     * 
     * @returns {Promise<any[]>}
     */
    async getSchools() {
        const response = await axios.get('catalog/schools')
        return response.data
    },

    /**
     * 
     * @param {String} staffUniqueId
     * @returns {Promise<any[]>}
     */
    async getSchoolsByStaff(staffUniqueId) {
        const response = await axios.get(`catalog/schools/staff/${staffUniqueId}`)
        return response.data
    },
}
