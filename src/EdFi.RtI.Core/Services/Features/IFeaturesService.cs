using EdFi.RtI.Core.DTOs.Features;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Features
{
    public interface IFeaturesService
    {
        Task<FeaturesSettings> GetFeaturesSettings();
        Task UpdateFeaturesSettings(FeaturesSettings featureSettings);
    }
}
