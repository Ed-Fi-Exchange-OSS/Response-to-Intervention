import { EdFiOdsApiSettings } from '@/models/EdFiOdsApiSettings'
import axios from 'axios'

export default {

    /**
     * @returns {Promise<String>}
     */
    async getDefaultEdFiVersion() {
        const response = await axios.get('settings/edFiVersion')
        return response.data
    },

    /**
     * @param {String} version 
     * @returns {Promise<EdFiOdsApiSettings>}
     */
    async getOdsApiSettings(version) {
        const response = await axios.get(`settings/odsApiSettings/${version}`)
        const data = response.data
        return data
    },

    /**
     * @returns {Promise<String[]>}
     */
    async getUserRoleAdminMappings() {
        const response = await axios.get('settings/userRoleAdminMappings')
        return response.data
    },
    
    /**
     * @returns {Promise<String[]>}
     */
    async getUserRoleTeacherMappings() {
        const response = await axios.get('settings/userRoleTeacherMappings')
        return response.data
    },
    
    /**
     * @param {EdFiOdsApiSettings} odsApiSettings 
     */
    async testOdsApiSettings(odsApiSettings) {
        await axios.post('settings/odsApiSettings/test', odsApiSettings)
    },

    /**
     * @param {EdFiOdsApiSettings} odsApiSettings 
     */
    async saveOdsApiSettings(odsApiSettings) {
        await axios.post('settings/odsApiSettings', odsApiSettings)
    },

    /**
     * @param {any[]} mappings 
     */
    async setUserEmailMappings(mappings) {
        await axios.post('settings/userEmailMappingsBulk', mappings)
    },

    /**
     * @param {String[]} staffClassificationDescriptors 
     */
    async saveUserRoleAdminMappings(staffClassificationDescriptors) {
        await axios.post('settings/userRoleAdminMappings', staffClassificationDescriptors)
    },
    
    /**
     * @param {String[]} staffClassificationDescriptors 
     */
    async saveUserRoleTeacherMappings(staffClassificationDescriptors) {
        await axios.post('settings/userRoleTeacherMappings', staffClassificationDescriptors)
    },

    /**
     * @param {String} edFiVersion
     */
    async setDefaultEdFiVersion(edFiVersion) {
        await axios.post('settings/edFiVersion', { edFiVersion })
    },
}
