﻿namespace SimpleAuth.Shared.Responses
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the search auth policies response.
    /// </summary>
    [DataContract]
    public record SearchAuthPoliciesResponse
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [DataMember(Name = "content")]
        public PolicyResponse[] Content { get; init; } = Array.Empty<PolicyResponse>();

        /// <summary>
        /// Gets or sets the total results.
        /// </summary>
        /// <value>
        /// The total results.
        /// </value>
        [DataMember(Name = "count")]
        public long TotalResults { get; init; }

        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        [DataMember(Name = "start_index")]
        public int StartIndex { get; init; }
    }
}
