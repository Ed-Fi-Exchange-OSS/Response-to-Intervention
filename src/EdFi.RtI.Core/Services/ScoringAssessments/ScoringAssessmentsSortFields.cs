using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.ScoringAssessments
{
    public class ScoringAssessmentsSortFields
    {
        public static readonly string Date = "date";

        public static readonly string Default = Date;

        public static readonly HashSet<string> SortFields = new HashSet<string>
        {
            Date,
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
