using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Interventions
{
    public class InterventionSearchParams : SearchParams
    {
        public new InterventionSearchParams Default()
        {
            return new InterventionSearchParams
            {
                GetFromCache = true,
                StoreInCache = true,
                PageIndex = 1,
                PageSize = 10,
            };
        }
    }
}
