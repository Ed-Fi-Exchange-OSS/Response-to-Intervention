using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.UserRoleMappings
{
    public class UserRoleMappingsAppSettings
    {
        public IList<string> Admins { get; set; }
        public IList<string> Teachers { get; set; }
    }
}
