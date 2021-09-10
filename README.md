# Response to Intervention (RTI)

## Project Description
Center Grove Community School Corporation has expressed interest in partnering with EdWire and INsite as part of a potential MSDF grant initiative. Currently, teachers record student scores for Fountas and Pinnell reading level assessments by hand. These values are then manually entered into a large master Microsoft Excel spreadsheet.

In addition to assessment performance data, intervention data is also tracked in this spreadsheet. This manual transfer of data is a cumbersome and time-consuming process, which is prone to error.

To respond to these challenges, a "Response to Intervention (RTI)" web application was developped and allows district users (teachers, etc.) to input student's assessment scores as well as student interventions.

At a high level, this web application supports the following functionality:
1. Assessment metadata creation/management
1. Intervention metadata creation/management
1. Student assessment score entry
1. Student intervention recording
1. Analytics on assessment scores
1. Analytics on intervention effectiveness

## Installatin Instructions
### Prerequisites
- yarn
- Visual Studio 2019 or later
- .Net Core 3.1
- Redis (running on localhost:6379)
### Run the RTI Solution locally
- Run yarn install in a command prompt from the \Ed-Fi-X-RTI\src\EdFi.RtI.Web folder
```
yarn install
```
- Open \Ed-Fi-X-RTI\src\EdFi.RtI.sln in Visual Studio
- Configure the EdFi.Rti.Web project (Standalone mode)
  - Update app.config.json 
    - Confirm that the startupMode is set to Standalone
    ```
    "startupMode": "Standalone"
    ```
- Configure the EdFi.RtI.Api (Standalone mode)
  - Update appsettings.json 
    - Confirm that the StartupMode is set to Standalone
    ```
    "StartupMode": "Standalone"
    ```     
    - Modify UserRoleMappings to add the Staff Classifications for each Admin or Teacher role
    ```
    "UserRoleMappings": {
      "Admins": [
        "LEA Administrator",
        "LEA System Administrator",
        "School Administrator",
        "Operational Support"
      ],
      "Teachers": [
        "Teacher"
      ]
    },    
    ```
    - Modify OdsApiSettings to setup access to your Ed-Fi API
    ```
      "OdsApiSettings": {
        "Version": "v3", -- v2 or v3
        "TokenUrl": "[EdFiApiUrl]/oauth/token/",
        "ResourcesUrl": "[EdFiApiUrl]/data/v3/2011/ed-fi/",
        "CompositesUrl": "[EdFiApiUrl]/composites/v1/2011/ed-fi/",
        "ClientId": "[ClientId]",
        "ClientSecret": "[ClientSecret]",
        "AssessmentNamespace": "[SomeNamespace]"
      },
    ```
- Confirm both the EdFi.RtI.Api and EdFi.Rti.Web projects as selected in Startup Projects
- Make a copy of the \Ed-Fi-X-RTI\src\EdFi.RtI.Web\public\app.config.json file in the same folder and name it config.json
- Rebuild solution in Visual Studio
- Run the application in Visual Studio

## Technical Specifications
- We are currently supporting Ed-Fi v2.5 and v3.3
- There currently is a dependency on REDIS which will be removed in future version and replaced by Ed-Fi API Composites.
- A new Ed-Fi Claim Set is necessary in order to provide the RTI web application access to the required Ed-Fi API endpoint.
  - You can find the claimset script under /scripts/claimsets/EdFi_Security - RTI ClaimSet.sql
  - Note that you will also need to restart your Ed-Fi Api service for the new claim set to take effect.

## Legal Information

Copyright (c) 2021 Ed-Fi Alliance, LLC and contributors.

Licensed under the [Apache License, Version 2.0](LICENSE) (the "License").

Unless required by applicable law or agreed to in writing, software distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
CONDITIONS OF ANY KIND, either express or implied. See the License for the
specific language governing permissions and limitations under the License.

See [NOTICES](NOTICES.md) for additional copyright and license notifications.
