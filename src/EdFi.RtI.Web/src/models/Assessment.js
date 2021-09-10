import { AssessmentAssessmentFamilyReference } from "./AssessmentAssessmentFamilyReference"
import { AssessmentContentStandard } from "./AssessmentContentStandard"
import { AssessmentEducationOrganizationReference } from "./AssessmentEducationOrganizationReference"
import { BaseModel } from "./BaseModel"

export class Assessment extends BaseModel {
    sections= [];

    scores= [
      {
        assessmentReportingMethodType: "",
        resultDatatypeType: "",
        minimumScore: "",
        maximumScore: ""
      }
    ];

    revisionDate= new Date().toISOString();

    programs= [];

    periodDescriptor= "";

    performanceLevels= [];

    nomenclature= "";

    namespaceProperty= "http://edgraph.com";

    maxRawScore= 0;

    languages= [];

    identifier= "";

    identificationCodes= [
      {
        assessmentIdentificationSystemDescriptor: "Test Contractor",
        assigningOrganizationIdentificationCode: null,
        identificationCode: "01774fa3-06f1-47fe-8801-c8b1e65057f2"
      }
    ];

    id= "";

    form= "";

    educationOrganizationReference = new AssessmentEducationOrganizationReference();

    contentStandard = new AssessmentContentStandard();

    categoryDescriptor= "";

    assessmentFamilyReference = new AssessmentAssessmentFamilyReference();

    assessedGradeLevels= [
      {
        gradeLevelDescriptor: "Third grade"
      }
    ];

    adaptiveAssessment= false;

    academicSubjects= [
      {
        academicSubjectDescriptor: "English Language Arts"
      }
    ];

    title= "";

    version= 0;

    _etag = "636712545113000000";

    safeParse () {
      if (this.maxRawScore) {this.maxRawScore = Number(this.maxRawScore)}
      if (this.version) {this.version = Number(this.version)}
      if (this.educationOrganizationReference && this.educationOrganizationReference.safeParse) {this.educationOrganizationReference.safeParse()}
      if (this.contentStandard && this.contentStandard.safeParse) {this.contentStandard.safeParse()}
      if (this.assessmentFamilyReference && this.assessmentFamilyReference.safeParse) {this.assessmentFamilyReference.safeParse()}
    }
}
