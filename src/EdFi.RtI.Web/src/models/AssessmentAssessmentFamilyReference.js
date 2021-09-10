import { AssessmentAssessmentFamilyReferenceLink } from "./AssessmentAssessmentFamilyReferenceLink"
import { BaseModel } from "./BaseModel"

export class AssessmentAssessmentFamilyReference extends BaseModel {
    title = "";

    link = new AssessmentAssessmentFamilyReferenceLink();

    safeParse () {
      if (this.link && this.link.safeParse) {this.link.safeParse()}
    }
}
