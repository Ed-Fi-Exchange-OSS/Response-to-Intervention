import { isNull } from "@/utils/validators"

export const LocalStorageItems = {
    edFiVersion: 'edfi-rti.version',
    userProfile: 'edfi-rti.userProfile',
    userRoleAdminMappings: 'edfi-rti.userRoleAdminMappings',
    userRoleTeacherMappings: 'edfi-rti.edfi-rti.userRoleTeacherMappings',
}

export default {
    local: {
        getEdFiVersion() {
            return localStorage.getItem(LocalStorageItems.edFiVersion)
        },

        getStaff() {
            const userProfile = getUserProfile();

            if (isNull(userProfile))
                return null;

            return userProfile.staff;
        },

        /**
         * @returns {String}
         */
        getStaffClassificationDescriptor() {
            return getStaffClassificationDescriptor()
        },

        getUserProfile() {
            return getUserProfile()
        },

        /**
         * @returns {String[]}
         */
        getUserRoleAdminMappings() {
            return getUserRoleAdminMappings()
        },
        
        /**
         * @returns {String[]}
         */
        getUserRoleTeacherMappings() {
            return getUserRoleTeacherMappings()
        },

        isUserRoleAdmin() {
            const adminRoleMappings = getUserRoleAdminMappings()
            console.log("adminRoleMappings:", adminRoleMappings)
            const userStaffClassificationDescriptor = getStaffClassificationDescriptor()
            console.log("userStaffClassificationDescriptor:", userStaffClassificationDescriptor)
            return adminRoleMappings.includes(userStaffClassificationDescriptor)
        },

        isUserRoleTeacher() {
            const teacherRoleMappings = getUserRoleTeacherMappings()
            console.log("teacherRoleMappings:", teacherRoleMappings)
            const userStaffClassificationDescriptor = getStaffClassificationDescriptor()
            console.log("userStaffClassificationDescriptor:", userStaffClassificationDescriptor)
            return teacherRoleMappings.includes(userStaffClassificationDescriptor)
        },

        /**
         * @param {String} version 
         */
        setEdFiVersion(version) {
            localStorage.setItem(LocalStorageItems.edFiVersion, version)
        },

        setUserProfile(profile) {
            const jsonStr = JSON.stringify(profile)
            localStorage.setItem(LocalStorageItems.userProfile, jsonStr)
        },

        /**
         * @param {String[]} staffClassifictionDescriptors
         */
        setUserRoleAdminMappings(staffClassifictionDescriptors) {
            const jsonStr = JSON.stringify(staffClassifictionDescriptors)
            localStorage.setItem(LocalStorageItems.userRoleAdminMappings, jsonStr)
        },

        /**
         * @param {String[]} staffClassifictionDescriptors
         */
        setUserRoleTeacherMappings(staffClassifictionDescriptors) {
            const jsonStr = JSON.stringify(staffClassifictionDescriptors)
            localStorage.setItem(LocalStorageItems.userRoleTeacherMappings, jsonStr)
        },

        removeEdFiVersion() {
            localStorage.removeItem(LocalStorageItems.edFiVersion)
        },

        removeUserProfile() {
            localStorage.removeItem(LocalStorageItems.userProfile)
        },
    },
}

function getUserProfile() {
    const jsonStr = localStorage.getItem(LocalStorageItems.userProfile)
    return JSON.parse(jsonStr)
}

/**
 * @returns {String[]}
 */
function getUserRoleAdminMappings() {
    const jsonStr = localStorage.getItem(LocalStorageItems.userRoleAdminMappings)
    return JSON.parse(jsonStr)
}

/**
 * @returns {String[]}
 */
function getUserRoleTeacherMappings() {
    const jsonStr = localStorage.getItem(LocalStorageItems.userRoleTeacherMappings)
    return JSON.parse(jsonStr)
}

/**
 * @returns {String}
 */
function getStaffClassificationDescriptor() {
    const userProfile = getUserProfile()

    if (isNull(userProfile))
        return null

    if (isNull(userProfile.staffEducationOrganizationAssignmentAssociation))
        return null
    
    return userProfile.staffEducationOrganizationAssignmentAssociation.staffClassificationDescriptor
}
