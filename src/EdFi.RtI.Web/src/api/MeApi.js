import axios from 'axios'

export default {
    async getUserProfile() {
        const response = await axios.get('me/profile')
        return response.data
    },
}
