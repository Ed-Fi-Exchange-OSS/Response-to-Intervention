using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class CachedModel
    {
        public DateTime DateCached { get; set; }
        public DateTime DateLastRefreshed { get; set; }
    }
}
