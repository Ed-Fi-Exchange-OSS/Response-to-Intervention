using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Assessments
{
    public class AssessmentSortFields
    {
        public static readonly string Default = LastModifiedDate;

        public static readonly string CategoryDescriptor = "categoryDescriptor";
        public static readonly string LastModifiedDate = "revisionDate";
        public static readonly string MaxRawScore = "maxRawScore";
        public static readonly string Title = "title";

        public static readonly HashSet<string> SortFields = new HashSet<string>
        {
            CategoryDescriptor,
            LastModifiedDate,
            MaxRawScore,
            Title,
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
