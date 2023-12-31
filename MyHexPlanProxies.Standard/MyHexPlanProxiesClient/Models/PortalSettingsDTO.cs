﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace MyHexPlanProxies.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class PortalSettingsDTO
    {
        /// <summary>
        /// Initializes a new instance of the PortalSettingsDTO class.
        /// </summary>
        public PortalSettingsDTO() { }

        /// <summary>
        /// Initializes a new instance of the PortalSettingsDTO class.
        /// </summary>
        public PortalSettingsDTO(PinSiteCare pinSiteCareSettings = default(PinSiteCare), IList<FinalTreatmentCaseDateDTO> finalTreatmentDateList = default(IList<FinalTreatmentCaseDateDTO>), SurgeonAddress surgeonAddressInfo = default(SurgeonAddress))
        {
            PinSiteCareSettings = pinSiteCareSettings;
            FinalTreatmentDateList = finalTreatmentDateList;
            SurgeonAddressInfo = surgeonAddressInfo;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PinSiteCareSettings")]
        public PinSiteCare PinSiteCareSettings { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "FinalTreatmentDateList")]
        public IList<FinalTreatmentCaseDateDTO> FinalTreatmentDateList { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SurgeonAddressInfo")]
        public SurgeonAddress SurgeonAddressInfo { get; set; }

    }
}
