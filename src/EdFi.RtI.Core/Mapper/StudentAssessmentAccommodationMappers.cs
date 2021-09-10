using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StudentAssessmentAccommodationMappers
    {

        public static StudentAssessmentAccommodationsItem MapToStudentAssessmentAccommodationsItem(this StudentAssessmentAccommodationv3 a)
        {
            return new StudentAssessmentAccommodationsItem
            {
                AccommodationDescriptor = a.AccommodationDescriptor?.MapToAccommodationDescriptorv2(),
            };
        }

        public static StudentAssessmentAccommodationv3 MapToStudentAssessmentAccommodationv3(this StudentAssessmentAccommodationsItem a)
        {
            return new StudentAssessmentAccommodationv3
            {
                AccommodationDescriptor = a.AccommodationDescriptor?.MapToAccommodationDescriptorv3(),
            };
        }
    }
}
