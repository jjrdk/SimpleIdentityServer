﻿namespace SimpleAuth.Shared.Models
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the ticket line content.
    /// </summary>
    [DataContract]
    public record TicketLine
    {
        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        [DataMember(Name = "scopes")]
        public string[] Scopes { get; init; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets the resource set identifier.
        /// </summary>
        /// <value>
        /// The resource set identifier.
        /// </value>
        [DataMember(Name = "resource_id")]
        public string ResourceSetId { get; init; } = null!;
    }
}
