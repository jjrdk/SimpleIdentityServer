﻿namespace SimpleAuth.Shared.DTOs
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SearchResourceSetResponse
    {
        [DataMember(Name = SearchResponseNames.Content)]
        public IEnumerable<ResourceSetResponse> Content { get; set; }
        [DataMember(Name = SearchResponseNames.TotalResults)]
        public int TotalResults { get; set; }
        [DataMember(Name = SearchResponseNames.StartIndex)]
        public int StartIndex { get; set; }
    }
}
