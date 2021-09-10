using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Assessments
{
    public class AssessmentSearchParams: SearchParams
    {
        /**
         * null = All Assessments
         * true = Assessments with current namespace
         * false = Assessments from third parties (with namespace different from current)
         */
        public bool? CurrentNamespace { get; set; }

        public new AssessmentSearchParams Default()
        {
            return new AssessmentSearchParams
            {
                GetFromCache = true,
                StoreInCache = true,
                PageIndex = 1,
                PageSize = 10,
            };
        }
    }
}
