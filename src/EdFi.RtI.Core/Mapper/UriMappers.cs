using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class UriMappers
    {
        public static InterventionUrisItem MapToInterventionUrisItem(this Uriv3 a)
        {
            return new InterventionUrisItem
            {
                Uri = a.Uri,
            };
        }

        public static Uriv3 MapToUriv3(this InterventionUrisItem a)
        {
            return new Uriv3
            {
                Uri = a.Uri,
            };
        }
    }
}
