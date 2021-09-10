using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class LearningResourceMetaDataUriMappers
    {
        public static InterventionLearningResourceMetadataURIsItem MapToInterventionLearningResourceMetadataURIsItem(this LearningResourceMetadataURIv3 a)
        {
            return new InterventionLearningResourceMetadataURIsItem
            {
                LearningResourceMetadataURI = a.LearningResourceMetadataURI,
            };
        }

        public static LearningResourceMetadataURIv3 MapToLearningResourceMetadataURIv3(this InterventionLearningResourceMetadataURIsItem a)
        {
            return new LearningResourceMetadataURIv3
            {
                LearningResourceMetadataURI = a.LearningResourceMetadataURI,
            };
        }
    }
}
