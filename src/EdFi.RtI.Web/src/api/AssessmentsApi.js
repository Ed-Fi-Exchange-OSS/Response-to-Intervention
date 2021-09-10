import axios from 'axios'

export default {

    /**
     * @returns {Promise<any[]>}
     */
    async getAssessmentFamilies() {
        const response = await axios.get('assessment/assessmentFamily')
        return response.data
    },
}
