import axios from 'axios'

export default {

    /**
     * @returns {Promise<any[]>}
     */
    async getAssessmentCategories() {
        const response = await axios.get('descriptor/assessmentCategory')
        return response.data
    },

    /**
     * @returns {Promise<any[]>}
     */
    async getAssessmentItemResults() {
        const response = await axios.get('types/assessmentResult')
        return response.data
    },

    /**
     * @returns {Promise<any[]>}
     */
    async getAssessmentPeriods() {
        const response = await axios.get('descriptor/assessmentPeriod')
        return response.data
    },
    
    /**
     * @returns {Promise<any[]>}
     */
    async getAssessmentReportingMethods() {
        const response = await axios.get('types/assessmentReportingMethod')
        return response.data
    },
    
    /**
     * @returns {Promise<any[]>}
     */
    async getDeliveryMethods() {
        const response = await axios.get('types/deliveryMethod')
        return response.data
    },
    
    /**
     * @returns {Promise<any[]>}
     */
    async getInterventionClasses() {
        const response = await axios.get('types/interventionClass')
        return response.data
    },

    /**
     * @returns {Promise<any[]>}
     */
    async getStaffClassificationDescriptors() {
        const response = await axios.get('descriptor/staffClassificationDescriptors')
        return response.data
    },
}
