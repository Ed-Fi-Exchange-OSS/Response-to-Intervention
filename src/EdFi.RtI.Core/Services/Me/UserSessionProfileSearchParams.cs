using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Me
{
    public class UserSessionProfileSearchParams : SearchParams
    {
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string StaffId { get; set; }

        public new UserSessionProfileSearchParams Default()
        {
            return new UserSessionProfileSearchParams
            {
                GetFromCache = false,
                StoreInCache = true,
            };
        }
    }
}
