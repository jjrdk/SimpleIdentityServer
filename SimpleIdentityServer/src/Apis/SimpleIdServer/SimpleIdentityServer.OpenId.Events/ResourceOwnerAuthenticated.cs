﻿#region copyright
// Copyright 2017 Habart Thierry
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

using SimpleBus.Core;

namespace SimpleIdentityServer.OpenId.Events
{
    public class ResourceOwnerAuthenticated : Event
    {
        public ResourceOwnerAuthenticated(string id, string aggregateId, string payload, int order)
        {
            Id = id;
            AggregateId = aggregateId;
            Payload = payload;
            Order = order;
        }

        public string Id { get; private set; }
        public string AggregateId { get; private set; }
        public string Payload { get; private set; }
        public int Order { get; set; }
    }
}