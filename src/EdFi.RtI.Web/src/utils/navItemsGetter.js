import requests from "@/utils/requests";
import session from "@/utils/session";
import { ScoringHeadingNavItem, ScoringAssessmentsNavItem, ScoringInterventionsNavItem, AdministrationHeadingNavItem, ManageAssessmentsNavItem, ManageInterventionsNavItem, SettingsNavItem  } from "@/router/NavItems";
import storage from "@/storage/index";

export default async () => {
    const navItems = [];
    
    try {
        const features = await requests.getFeaturesSettings();
        console.log("features:", features);

        session.setFeaturesSettings(features);
        
        if (features.showAssessments == true || features.showInterventions == true)
            navItems.push(ScoringHeadingNavItem);

        if (features.showAssessments == true)
            navItems.push(ScoringAssessmentsNavItem);

        if (features.showInterventions == true)
            navItems.push(ScoringInterventionsNavItem);

        if (storage.local.isUserRoleAdmin()) {
            if (features.showAssessments == true || features.showInterventions == true)
                navItems.push(AdministrationHeadingNavItem);

            if (features.showAssessments == true)
                navItems.push(ManageAssessmentsNavItem);

            if (features.showInterventions == true)
                navItems.push(ManageInterventionsNavItem);
        }
    } catch {
        session.removeFeaturesSettings();

        navItems.push(ScoringHeadingNavItem);
        navItems.push(ScoringAssessmentsNavItem);
        navItems.push(ScoringInterventionsNavItem);

        if (storage.local.isUserRoleAdmin()) {
            navItems.push(AdministrationHeadingNavItem);
            navItems.push(ManageAssessmentsNavItem);
            navItems.push(ManageInterventionsNavItem);
        }
    }

    if (storage.local.isUserRoleAdmin())
        navItems.push(SettingsNavItem);

    return navItems;
};
