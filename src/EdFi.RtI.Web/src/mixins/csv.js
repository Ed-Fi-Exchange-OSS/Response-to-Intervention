/* eslint-disable */

export const csv = {

    methods: {
        export(scorings) {
            let scoringsAsRows = this.parseScoringAssessments(scorings);

            var curday = function(sp) {
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth()+1; //As January is 0.
                var yyyy = today.getFullYear();
                
                if(dd<10) dd='0'+dd;
                if(mm<10) mm='0'+mm;
                return (mm+sp+dd+sp+yyyy+sp);
            };

            var date = new Date();
            var time = date.getHours() + "." + date.getMinutes() + "." + date.getSeconds();

            let csvContent = "data:text/csv;charset=utf-8,";                    
            
            scoringsAsRows.forEach(rowArray => {
                let row = rowArray.join(",");
                csvContent += row + "\r\n";
            });

            var encodedUri = encodeURI(csvContent);
            var link = document.createElement("a");
            link.setAttribute("href", encodedUri);
            link.setAttribute("download", "my_data_" + curday('-') + time + ".csv");
            document.body.appendChild(link);

            link.click();
        },

        exportInterventions(scorings, interventions) {
            let grid = this.parseScoringInterventions(scorings, interventions);
            let csvContent = "data:text/csv;charset=utf-8,";                    
            
            grid.forEach(row => {
                let rowStr = row.join(',');
                csvContent += rowStr + '\r\n';
            });

            var encodedUri = encodeURI(csvContent);
            var link = document.createElement("a");

            link.setAttribute("href", encodedUri);
            link.setAttribute("download", this.getFileName());
            
            document.body.appendChild(link);
            link.click();
        },

        getFileName() {
            let curday = function(sp) {
                let today = new Date();
                let dd = today.getDate();
                let mm = today.getMonth() + 1; //As January is 0.
                let yyyy = today.getFullYear();
                
                if (dd < 10) dd = '0' + dd;
                if (mm < 10) mm = '0' + mm;

                return mm + sp + dd + sp + yyyy+ sp;
            };

            let date = new Date();
            let time = date.getHours() + "." + date.getMinutes() + "." + date.getSeconds();

            return `my_data_${curday('-')}${time}.csv`;
        },

        parse() {
            var curday = function(sp) {
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth()+1; //As January is 0.
                var yyyy = today.getFullYear();
                
                if(dd<10) dd='0'+dd;
                if(mm<10) mm='0'+mm;
                return (mm+sp+dd+sp+yyyy+sp);
            };

            var date = new Date();
            var time = date.getHours() + "." + date.getMinutes() + "." + date.getSeconds();

            const rows = [
                ["Lexia", "2020-08-25T00:00:00Z", "2021-05-23T00:00:00Z", "Curriculum", "Individual"],
                ["Waterford", "2020-08-25T00:00:00Z", "2021-05-23T00:00:00Z", "Curriculum", "Individual"]
            ];
            
            let csvContent = "data:text/csv;charset=utf-8,";                    
            
            rows.forEach(function(rowArray) {
                let row = rowArray.join(",");
                csvContent += row + "\r\n";
            });

            var encodedUri = encodeURI(csvContent);
            var link = document.createElement("a");
            link.setAttribute("href", encodedUri);
            link.setAttribute("download", "my_data_" + curday('-') + time + ".csv");
            document.body.appendChild(link); // Required for FF

            link.click(); // This will download the data file named "my_data.csv".*/
        },

        parseTest() {
            var json_data = [
                {
                    "_etag": "637390595534200000",
                    "appropriateGradeLevels": [],
                    "appropriateSexes": [],
                    "beginDate": "2020-08-25T00:00:00Z",
                    "classType": "Curriculum",
                    "deliveryMethodType": "Individual",
                    "diagnoses": [],
                    "educationContents": [],
                    "educationOrganizationReference": {
                        "educationOrganizationId": 255901001,
                        "link": {
                            "href": "/educationOrganizations?educationOrganizationId=255901001",
                            "rel": "EducationOrganization"
                        }
                    },
                    "endDate": "2021-05-23T00:00:00Z",
                    "id": "4da1bbe8f6ef42b2a575455ac54dd4ba",
                    "identificationCode": "Lexia",
                    "interventionPrescriptions": [],
                    "learningResourceMetadataURIs": [],
                    "meetingTimes": [],
                    "populationServeds": [],
                    "staffs": [],
                    "uris": []
                },
                {
                    "_etag": "637390595835970000",
                    "appropriateGradeLevels": [],
                    "appropriateSexes": [],
                    "beginDate": "2020-08-25T00:00:00Z",
                    "classType": "Curriculum",
                    "deliveryMethodType": "Individual",
                    "diagnoses": [],
                    "educationContents": [],
                    "educationOrganizationReference": {
                        "educationOrganizationId": 255901001,
                        "link": {
                            "href": "/educationOrganizations?educationOrganizationId=255901001",
                            "rel": "EducationOrganization"
                        }
                    },
                    "endDate": "2021-05-23T00:00:00Z",
                    "id": "93d6591f6c564c708760ee0c0652037a",
                    "identificationCode": "Waterford",
                    "interventionPrescriptions": [],
                    "learningResourceMetadataURIs": [],
                    "meetingTimes": [],
                    "populationServeds": [],
                    "staffs": [],
                    "uris": []
                }
            ];

            var result = [];
            var aux = [];
            var json_aux

            for (var i in json_data){
                json_aux = json_data [i]
                for(var j in json_aux){
                    aux.push([json_aux [j]]);
                } 
                result.push([aux]);
                aux.pop();
            }

            console.log("ResultRRRR: ", result);

            //result.splice(1,2);

            /*console.log("Array: ", result);

            const rows = [
                ["Lexia", "2020-08-25T00:00:00Z", "2021-05-23T00:00:00Z", "Curriculum", "Individual"],
                ["Waterford", "2020-08-25T00:00:00Z", "2021-05-23T00:00:00Z", "Curriculum", "Individual"]
            ];

            console.log("Array 222: ", rows);*/
        },

        parseScoringAssessments(scorings) {
            console.log('parseScoringAssessments', scorings);

            let rows = [];

            let headers = [];

            headers.push('Students');
    
            let scoring = scorings[0];
            scoring.associations.forEach(association => {
            headers.push(association.assessment.title);
            });

            rows.push(headers);

            scorings.forEach(scoring => {
                let scoringRow = [];

                let studentName = `${scoring.student.firstName} ${scoring.student.lastSurname}`;
                scoringRow.push(studentName);

                scoring.associations.forEach(association => {
                    let score = association.associationModel.scoreResults[0].result;
                    scoringRow.push(score);
                });

                rows.push(scoringRow);
            });

            return rows;
        },

        /**
         * Parses the scorings into an array of arrays representing the Scoring Interventions grid
         * Eg:
         * [ Student,      Intervention 1, Intervention 2, ..., Intervention N ],
         * [ Student Name, Boolean 1,      Boolean 2, ........, Boolean N, ],
         * [ Student Name, Boolean 1,      Boolean 2, ........, Boolean N, ],
         * [ Student Name, Boolean 1,      Boolean 2, ........, Boolean N, ],
         * ...
         * 
         * @param { StudentInterventionsDTO } scorings 
         * @param { Intervention[] } interventions 
         */
        parseScoringInterventions(scorings, interventions) {
            console.log('parseScoringInterventions');
            console.log('scorings:', scorings);
            console.log('interventions:', interventions);

            let grid = [];

            // grid headers -> [ Student, Intervention 1, Intervention 2, ..., Intervention N ]
            let headers = [];

            headers.push('Student');

            interventions.forEach(intervention => {
                let headerName = `${intervention.identificationCode} | ${intervention.beginDate}`;
                headers.push(headerName);
            });

            grid.push(headers);

            // grid rows -> [ Student Name, Boolean, Boolean, Boolean, ... ]
            scorings.forEach(scoring => {
                let row = [];

                let studentName = `${scoring.student.firstName} ${scoring.student.lastSurname}`;
                row.push(studentName);

                interventions.forEach(intervention => {
                    let value = isInterventionAssociated(scoring.associations, intervention.id) ? 'x' : '';
                    row.push(value);
                });

                grid.push(row);
            });

            return grid;
        },
    }, // methods
}

const isInterventionAssociated = (associations, interventionId) => {
    let studentInterventionAssociationIds = associations.map(association => association.intervention.id);
    let uniqueAssociationIds = new Set(studentInterventionAssociationIds);
    return uniqueAssociationIds.has(interventionId);
};
