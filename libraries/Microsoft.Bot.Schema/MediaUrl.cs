﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Schema
{
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Media URL.
    /// </summary>
    public partial class MediaUrl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaUrl"/> class.
        /// </summary>
        public MediaUrl()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaUrl"/> class.
        /// </summary>
        /// <param name="url">Url for the media.</param>
        /// <param name="profile">Optional profile hint to the client to
        /// differentiate multiple MediaUrl objects from each other.</param>
        public MediaUrl(string url = default(string), string profile = default(string))
        {
            Url = url;
            Profile = profile;
            CustomInit();
        }

        /// <summary>
        /// Gets or sets url for the media.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets optional profile hint to the client to differentiate
        /// multiple MediaUrl objects from each other.
        /// </summary>
        [JsonProperty(PropertyName = "profile")]
        public string Profile { get; set; }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults.
        /// </summary>
        partial void CustomInit();
    }
}
