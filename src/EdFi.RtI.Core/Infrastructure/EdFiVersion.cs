using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Infrastructure
{
    public static class EdFiVersion
    {
        /// <summary>
        /// Deprecated. Use v2 instead
        /// </summary>
        public static readonly string v26 = "v26";

        /// <summary>
        /// Deprecated. Use v3 instead
        /// </summary>
        public static readonly string v31 = "v31";

        public static readonly string v2 = "v2";
        public static readonly string v3 = "v3";

        private static readonly HashSet<string> Items = new HashSet<string>(new string[]
        {
            v2,
            v3,
        });

        public static bool IsValid(string value)
        {
            return Items.Contains(value);
        }
    }
}
