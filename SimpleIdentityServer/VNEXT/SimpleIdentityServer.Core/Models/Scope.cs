﻿#region copyright
// Copyright 2015 Habart Thierry
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
#endregion
using System.Collections.Generic;

namespace SimpleIdentityServer.Core.Models
{
    public enum ScopeType
    {
        ProtectedApi,
        ResourceOwner
    }

    public class Scope
    {
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a boolean whether the scope is displayed in the consent.
        /// </summary>
        public bool IsDisplayedInConsent { get; set; }

        /// <summary>
        /// Gets or sets a boolean whether the scope is internal : openid, profile ...
        /// </summary>
        public bool IsOpenIdScope { get; set; }

        /// <summary>
        /// Gets or sets a boolean whether the scope is exposed in the well-known configuration endpoint.
        /// </summary>
        public bool IsExposed { get; set; }

        public ScopeType Type { get; set; }

        public List<string> Claims { get; set; }
    }
}
