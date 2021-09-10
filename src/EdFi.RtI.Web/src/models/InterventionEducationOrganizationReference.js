import { InterventionEducationOrganizationReferenceLink } from "./InterventionEducationOrganizationReferenceLink"
import { BaseModel } from "./BaseModel"

export class InterventionEducationOrganizationReference extends BaseModel {
    educationOrganizationId = 255901001;

    interventionEducationOrganizationReferenceLink = new InterventionEducationOrganizationReferenceLink();

    safeParse () {
      if (this.educationOrganizationId) {this.educationOrganizationId = Number(this.educationOrganizationId)}
      if (this.interventionEducationOrganizationReferenceLink && this.interventionEducationOrganizationReferenceLink) {this.interventionEducationOrganizationReferenceLink.safeParse()}
    }
}
