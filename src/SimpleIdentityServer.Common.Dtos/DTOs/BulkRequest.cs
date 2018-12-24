﻿// Copyright © 2015 Habart Thierry, © 2018 Jacob Reimers
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SimpleIdentityServer.Shared.DTOs
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class BulkRequest
    {
        [DataMember(Name = ScimConstants.ScimResourceNames.Schemas)]
        public IEnumerable<string> Schemas { get; set; }
        /// <summary>
        /// Number of errors that the service provider will accept before the operation is terminated
        /// and an error response is returned.
        /// </summary>
        [DataMember(Name = ScimConstants.BulkRequestNames.FailOnErrors)]
        public int? FailOnErrors { get; set; }
        /// <summary>
        /// Operations within a bulk job.
        /// </summary>
        [DataMember(Name = ScimConstants.BulkRequestNames.Operations)]
        public IEnumerable<BulkOperationRequest> Operations { get; set; }
    }
}
