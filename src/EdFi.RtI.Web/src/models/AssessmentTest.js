import { BaseModel } from "./BaseModel"

export class AssessmentTest extends BaseModel {
    namespace = "http://edgraph.com";

    _etag = null;

    academicSubjects = [
      {
        academicSubjectDescriptor: "English Language Arts"
      }
    ];

    adaptiveAssessment= false;

    assessedGradeLevels= [
      {
        gradeLevelDescriptor: "Third grade"
      }
    ];

    assessmentFamilyReference = null;

    categoryDescriptor= "";

    contentStandard= {
      authors: [],
      beginDate: null,
      endDate: null,
      mandatingEducationOrganizationReference: null,
      publicationDate: null,
      publicationStatusType: null,
      publicationYear: null,
      title: "State Essential Knowledge and Skills",
      uri: null,
      version: null
    };

    educationOrganizationReference = null;

    form = "";

    id= "";

    identificationCodes= [
      {
        assessmentIdentificationSystemDescriptor: "Test Contractor",
        assigningOrganizationIdentificationCode: null,
        identificationCode: "01774fa3-06f1-47fe-8801-c8b1e65057f2"
      }
    ];

    identifier= "01774fa3-06f1-47fe-8801-c8b1e65057f2";

    languages= [];

    maxRawScore= 0;

    namespaceProperty= "http=//ed-fi.org/Assessment/Assessment.xml";

    nomenclature= "";

    performanceLevels= [];

    periodDescriptor= "";

    programs= [];

    revisionDate=  new Date().toISOString();

    scores= [];

    sections= [];

    title= "";

    version= 0;

    safeParse () {
      if (this.maxRawScore) {this.maxRawScore = Number(this.maxRawScore)}
      if (this.version) {this.version = Number(this.version)}
    }
}
