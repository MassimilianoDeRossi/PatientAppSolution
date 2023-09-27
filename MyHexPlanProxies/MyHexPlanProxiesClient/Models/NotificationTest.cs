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

    public partial class NotificationTest
    {
        /// <summary>
        /// Initializes a new instance of the NotificationTest class.
        /// </summary>
        public NotificationTest() { }

        /// <summary>
        /// Initializes a new instance of the NotificationTest class.
        /// </summary>
        public NotificationTest(string message = default(string), int? type = default(int?), Guid? appId = default(Guid?), int? motivationalMessageCategory = default(int?))
        {
            Message = message;
            Type = type;
            AppId = appId;
            MotivationalMessageCategory = motivationalMessageCategory;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Message")]
        public string Message { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public int? Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AppId")]
        public Guid? AppId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MotivationalMessageCategory")]
        public int? MotivationalMessageCategory { get; set; }

    }
}