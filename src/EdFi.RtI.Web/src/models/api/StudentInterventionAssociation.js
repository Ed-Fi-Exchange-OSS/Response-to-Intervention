import { StudentInterventionAssociationCohortReference } from "./StudentInterventionAssociationCohortReference"
import { StudentInterventionAssociationInterventionReference } from "./StudentInterventionAssociationInterventionReference"
import { StudentInterventionAssociationStudentReference } from "./StudentInterventionAssociationStudentReference"

export class StudentInterventionAssociation {
    _etag = "";

    cohortReference = new StudentInterventionAssociationCohortReference();

    diagnosticStatement = "";

    id = "";

    interventionEffectivenesses = [];

    interventionReference = new StudentInterventionAssociationInterventionReference();

    studentReference = new StudentInterventionAssociationStudentReference();;
}
