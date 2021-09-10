using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services
{
    public class SearchParams
    {
        public bool GetFromCache { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public bool StoreInCache { get; set; }
        public string SearchValue { get; set; }
        public bool SortDesc { get; set; }
        public string SortField { get; set; }

        public SearchParams Default()
        {
            return new SearchParams
            {
                GetFromCache = true,
                PageIndex = 1,
                PageSize = 10,
                StoreInCache = true,
            };
        }
    }
}
