using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Types
{
    class DeliveryMethodDTO
    {
        public string _Etag { get; set; }
        public string CodeValue { get; set; }
        public int DeliveryMethodTypeId { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string ShortDescription { get; set; }
    }
}
