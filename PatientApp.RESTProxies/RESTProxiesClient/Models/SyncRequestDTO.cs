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

    public partial class SyncRequestDTO
    {
        /// <summary>
        /// Initializes a new instance of the SyncRequestDTO class.
        /// </summary>
        public SyncRequestDTO() { }

        /// <summary>
        /// Initializes a new instance of the SyncRequestDTO class.
        /// </summary>
        public SyncRequestDTO(DateTime? lastDateSync = default(DateTime?))
        {
            LastDateSync = lastDateSync;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "LastDateSync")]
        public DateTime? LastDateSync { get; set; }

    }
}
