import { AssessmentContentStandardMandatingEducationOrganizationReferenceLink } from "./AssessmentContentStandardMandatingEducationOrganizationReferenceLink"
import { BaseModel } from "./BaseModel"

export class AssessmentContentStandardMandatingEducationOrganizationReference extends BaseModel {
    educationOrganizationId = 0;

    link = new AssessmentContentStandardMandatingEducationOrganizationReferenceLink();

    safeParse () {
      if (this.educationOrganizationId) {this.educationOrganizationId = Number(this.educationOrganizationId)}
      if (this.link && this.link.safeParse) {this.link.safeParse()}
    }
}
