﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace PatientApp.RESTProxies.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class PrescriptionClick
    {
        /// <summary>
        /// Initializes a new instance of the PrescriptionClick class.
        /// </summary>
        public PrescriptionClick() { }

        /// <summary>
        /// Initializes a new instance of the PrescriptionClick class.
        /// </summary>
        public PrescriptionClick(DateTime? dateOfAdjustment = default(DateTime?), int? sequence = default(int?), int? stageType = default(int?), IList<Strut> struts = default(IList<Strut>))
        {
            DateOfAdjustment = dateOfAdjustment;
            Sequence = sequence;
            StageType = stageType;
            Struts = struts;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DateOfAdjustment")]
        public DateTime? DateOfAdjustment { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Sequence")]
        public int? Sequence { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "StageType")]
        public int? StageType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Struts")]
        public IList<Strut> Struts { get; set; }

    }
}
