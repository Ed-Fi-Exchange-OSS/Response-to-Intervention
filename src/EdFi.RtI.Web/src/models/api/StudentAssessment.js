import { StudentAssessmentAssessmentReference } from "./StudentAssessmentAssessmentReference"
import { StudentAssessmentScoreResultsItem } from "./StudentAssessmentScoreResultsItem"
import { StudentAssessmentStudentReference } from "./StudentAssessmentStudentReference"

export class StudentAssessment {
    studentObjectiveAssessments = [];

    serialNumber = "";

    scoreResults = [
      new StudentAssessmentScoreResultsItem()
    ];

    retestIndicatorType = "";

    reasonNotTestedType = "";

    performanceLevels = [];

    items = [];

    identifier = "";

    id = "";

    eventDescription = "";

    eventCircumstanceType = "";

    assessmentReference = new StudentAssessmentAssessmentReference();

    administrationLanguageDescriptor = "";

    administrationEnvironmentType = "";

    administrationEndDate = new Date();

    administrationDate = new Date();

    accommodations = [];

    _etag = "";

    studentReference = new StudentAssessmentStudentReference();

    whenAssessedGradeLevelDescriptor = "";
}
