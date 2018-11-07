﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Uma.Common.DTOs
{
    [DataContract]
    public class SearchAuthPoliciesResponse
    {
        [DataMember(Name = SearchResponseNames.Content)]
        public IEnumerable<PolicyResponse> Content { get; set; }

        [DataMember(Name = SearchResponseNames.TotalResults)]
        public int TotalResults { get; set; }

        [DataMember(Name = SearchResponseNames.StartIndex)]
        public int StartIndex { get; set; }
    }
}
