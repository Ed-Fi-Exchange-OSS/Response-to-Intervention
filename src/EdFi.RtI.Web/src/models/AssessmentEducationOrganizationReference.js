import { AssessmentEducationOrganizationReferenceLink } from "./AssessmentEducationOrganizationReferenceLink"
import { BaseModel } from "./BaseModel"

export class AssessmentEducationOrganizationReference extends BaseModel {
    educationOrganizationId = 0;

    assessmentEducationOrganizationReferenceLink = new AssessmentEducationOrganizationReferenceLink();

    safeParse () {
      if (this.educationOrganizationId) {this.educationOrganizationId = Number(this.educationOrganizationId)}
      if (this.assessmentEducationOrganizationReferenceLink && this.assessmentEducationOrganizationReferenceLink) {this.assessmentEducationOrganizationReferenceLink.safeParse()}
    }
}
