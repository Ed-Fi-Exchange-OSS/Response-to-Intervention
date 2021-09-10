import api from "axios";

export default {
    async getFeaturesSettings() {
        const url = 'features';
        const response = await api.get(url);
        return response.data;
    },
}
