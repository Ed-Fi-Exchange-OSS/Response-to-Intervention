import { AssessmentContentStandardMandatingEducationOrganizationReference } from "./AssessmentContentStandardMandatingEducationOrganizationReference"
import { BaseModel } from "./BaseModel"

export class AssessmentContentStandard extends BaseModel {
    authors = [];

    beginDate = new Date().toISOString(); // optional

    endDate = new Date().toISOString(); // optional

    mandatingEducationOrganizationReference = new AssessmentContentStandardMandatingEducationOrganizationReference();

    publicationDate = new Date().toISOString(); // optional

    publicationStatusType = "";

    publicationYear = 0; // optional

    title = "State Essential Knowledge and Skills";

    uri = "";

    version = "";

    safeParse () {
      if (this.publicationYear) {this.publicationYear = Number(this.publicationYear)}
      if (this.mandatingEducationOrganizationReference && this.mandatingEducationOrganizationReference.safeParse) {this.mandatingEducationOrganizationReference.safeParse()}
    }
}
