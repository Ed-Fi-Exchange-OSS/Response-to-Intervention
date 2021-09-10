using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Interventions
{
    public class InterventionSortFields
    {
        public static readonly string BeginDate = "beginDate";
        public static readonly string EndDate = "endDate";
        public static readonly string IdentificationCode = "identificationCode";

        public static readonly string Default = IdentificationCode;

        public static readonly HashSet<string> SortFields = new HashSet<string>
        {
            IdentificationCode,
            BeginDate,
            EndDate,
        };

        public static bool IsSortFieldValid(string field)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return false;
            }

            return SortFields.Contains(field);
        }
    }
}
