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

    public partial class PatientDiaryEvent
    {
        /// <summary>
        /// Initializes a new instance of the PatientDiaryEvent class.
        /// </summary>
        public PatientDiaryEvent() { }

        /// <summary>
        /// Initializes a new instance of the PatientDiaryEvent class.
        /// </summary>
        public PatientDiaryEvent(DateTime? eventDate = default(DateTime?), string type = default(string), string description = default(string), Guid? caseUid = default(Guid?), int? entityId = default(int?), DateTime? entityExpectedDate = default(DateTime?))
        {
            EventDate = eventDate;
            Type = type;
            Description = description;
            CaseUid = caseUid;
            EntityId = entityId;
            EntityExpectedDate = entityExpectedDate;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "EventDate")]
        public DateTime? EventDate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CaseUid")]
        public Guid? CaseUid { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "EntityId")]
        public int? EntityId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "EntityExpectedDate")]
        public DateTime? EntityExpectedDate { get; set; }

        /// <summary>
        /// Local Guid used to avoid problem with duplicated logs on myhexplan portal
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }
    }
}
