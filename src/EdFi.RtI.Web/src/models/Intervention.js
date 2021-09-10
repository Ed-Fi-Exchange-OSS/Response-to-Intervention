import { InterventionEducationOrganizationReference } from "./InterventionEducationOrganizationReference"
import { BaseModel } from "./BaseModel"

export class Intervention extends BaseModel {
    _Etagc = "637390595534200000";

    appropriateGradeLevels = [];

    appropriateSexes = [];

    beginDate = new Date().toISOString();

    classType = "";

    deliveryMethodType = "";

    diagnoses = [];

    educationContents = [];

    educationOrganizationReference = new InterventionEducationOrganizationReference();

    endDate = new Date().toISOString();

    id = "";

    identificationCode = "";

    interventionPrescriptions = [];

    learningResourceMetadataURIs = [];

    meetingTimes = [];

    populationServeds = [];

    staffs = [];

    uris = [];

    safeParse (){
      if (this.educationOrganizationReference && this.educationOrganizationReference.safeParse) {this.educationOrganizationReference.safeParse()}
    }
}
