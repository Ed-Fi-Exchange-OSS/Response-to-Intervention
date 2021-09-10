using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StaffLanguagesItemv3
    {
        public string LanguageDescriptor { get; set; }
        public IList<StaffLanguagesItemUsesItemv3> Uses { get; set; }
    }
}
