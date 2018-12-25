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

namespace SimpleAuth.Logging
{
    using Microsoft.Extensions.Logging;

    public class UmaServerEventSource : BaseEventSource, IUmaServerEventSource
    {
        private static class Tasks
        {
            public const string Authorization = "Authorization";
            public const string Introspection = "Introspection";
            public const string Permission = "Permission";
            public const string AuthorizationPolicy = "AuthorizationPolicy";
            public const string ResourceSet = "ResourceSet";
            public const string Scope = "Scope";
            public const string Failure = "Failure";
        }

        public UmaServerEventSource(ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<UmaServerEventSource>())
        {
        }

        public void StartGettingAuthorization(string request)
        {
            var evt = new Event
            {
                Id = 700,
                Task = Tasks.Authorization,
                Message = $"Start getting RPT tokens : {request}"
            };

            LogInformation(evt);
        }

        public void CheckAuthorizationPolicy(string request)
        {
            var evt = new Event
            {
                Id = 701,
                Task = Tasks.Authorization,
                Message = $"Check authorization policy : {request}"
            };

            LogInformation(evt);
        }

        public void AuthorizationPoliciesFailed(string ticketId)
        {
            var evt = new Event
            {
                Id = 702,
                Task = Tasks.Authorization,
                Message = $"The authorization policies failed for the ticket {ticketId}"
            };

            LogInformation(evt);
        }

        public void RequestIsNotAuthorized(string request)
        {
            var evt = new Event
            {
                Id = 703,
                Task = Tasks.Authorization,
                Message = $"Request is not authorized : {request}",
                Operation = "not-authorized"
            };

            LogInformation(evt);
        }

        public void RequestIsAuthorized(string request)
        {
            var evt = new Event
            {
                Id = 704,
                Task = Tasks.Authorization,
                Message = $"Request is authorized : {request}",
                Operation = "authorized"
            };

            LogInformation(evt);
        }

        public void StartToIntrospect(string rpt)
        {
            var evt = new Event
            {
                Id = 710,
                Task = Tasks.Introspection,
                Message = $"Start to introspect the RPT {rpt}"
            };

            LogInformation(evt);
        }

        public void RptHasExpired(string rpt)
        {
            var evt = new Event
            {
                Id = 711,
                Task = Tasks.Introspection,
                Message = $"RPT {rpt} has expired"
            };

            LogInformation(evt);
        }

        public void EndIntrospection(string result)
        {
            var evt = new Event
            {
                Id = 712,
                Task = Tasks.Introspection,
                Message = $"End introspection {result}"
            };

            LogInformation(evt);
        }

        public void StartAddPermission(string request)
        {
            var evt = new Event
            {
                Id = 720,
                Task = Tasks.Permission,
                Message = $"Start to add permission : {request}"
            };

            LogInformation(evt);
        }

        public void FinishAddPermission(string request)
        {
            var evt = new Event
            {
                Id = 721,
                Task = Tasks.Permission,
                Message = $"Finish to add permission : {request}"
            };

            LogInformation(evt);
        }

        public void StartAddingAuthorizationPolicy(string request)
        {
            var evt = new Event
            {
                Id = 730,
                Task = Tasks.AuthorizationPolicy,
                Message = $"Start adding authorization policy : {request}"
            };

            LogInformation(evt);
        }

        public void FinishToAddAuthorizationPolicy(string result)
        {
            var evt = new Event
            {
                Id = 731,
                Task = Tasks.AuthorizationPolicy,
                Message = $"Finish to add authorization policy : {result}"
            };

            LogInformation(evt);
        }

        public void StartToRemoveAuthorizationPolicy(string policyId)
        {
            var evt = new Event
            {
                Id = 732,
                Task = Tasks.AuthorizationPolicy,
                Message = $"Start to remove authorization policy : {policyId}"
            };

            LogInformation(evt);
        }

        public void FinishToRemoveAuthorizationPolicy(string policyId)
        {
            var evt = new Event
            {
                Id = 733,
                Task = Tasks.AuthorizationPolicy,
                Message = $"Finish to remove authorization policy : {policyId}"
            };

            LogInformation(evt);
        }

        public void StartAddResourceToAuthorizationPolicy(string policy, string resourceId)
        {
            var evt = new Event
            {
                Id = 734,
                Task = Tasks.AuthorizationPolicy,
                Message = $"Start to add resource {resourceId} to authorization policy {policy}"
            };

            LogInformation(evt);
        }

        public void FinishAddResourceToAuthorizationPolicy(string policy, string resourceId)
        {
            var evt = new Event
            {
                Id = 735,
                Task = Tasks.AuthorizationPolicy,
                Message = $"Finish to add resource {resourceId} to authorization policy {policy}"
            };

            LogInformation(evt);
        }

        public void StartRemoveResourceFromAuthorizationPolicy(string policy, string resourceId)
        {
            var evt = new Event
            {
                Id = 736,
                Task = Tasks.AuthorizationPolicy,
                Message = $"Start to remove resource {resourceId} from authorization policy {policy}"
            };

            LogInformation(evt);
        }

        public void FinishRemoveResourceFromAuthorizationPolicy(string policy, string resourceId)
        {
            var evt = new Event
            {
                Id = 737,
                Task = Tasks.AuthorizationPolicy,
                Message = $"Finish to remove resource {resourceId} from authorization policy {policy}"
            };

            LogInformation(evt);
        }

        public void StartUpdateAuthorizationPolicy(string request)
        {
            var evt = new Event
            {
                Id = 738,
                Task = Tasks.AuthorizationPolicy,
                Message = $"Start to update authorization policy {request}"
            };

            LogInformation(evt);
        }

        public void FinishUpdateAuhthorizationPolicy(string request)
        {
            var evt = new Event
            {
                Id = 739,
                Task = Tasks.AuthorizationPolicy,
                Message = $"Start to update authorization policy {request}"
            };

            LogInformation(evt);
        }

        public void StartToAddResourceSet(string request)
        {
            var evt = new Event
            {
                Id = 740,
                Task = Tasks.ResourceSet,
                Message = $"Start to add resource set : {request}"
            };

            LogInformation(evt);
        }

        public void FinishToAddResourceSet(string result)
        {
            var evt = new Event
            {
                Id = 741,
                Task = Tasks.ResourceSet,
                Message = $"Finish to add resource set : {result}"
            };

            LogInformation(evt);
        }

        public void StartToRemoveResourceSet(string resourceSetId)
        {
            var evt = new Event
            {
                Id = 742,
                Task = Tasks.ResourceSet,
                Message = $"Start to remove resource set : {resourceSetId}"
            };

            LogInformation(evt);
        }

        public void FinishToRemoveResourceSet(string resourceSetId)
        {
            var evt = new Event
            {
                Id = 743,
                Task = Tasks.ResourceSet,
                Message = $"Finish to remove resource set : {resourceSetId}"
            };

            LogInformation(evt);
        }

        public void StartToUpdateResourceSet(string request)
        {
            var evt = new Event
            {
                Id = 744,
                Task = Tasks.ResourceSet,
                Message = $"Start to update the resource set : {request}"
            };

            LogInformation(evt);
        }

        public void FinishToUpdateResourceSet(string request)
        {
            var evt = new Event
            {
                Id = 745,
                Task = Tasks.ResourceSet,
                Message = $"Start to update the resource set : {request}"
            };

            LogInformation(evt);
        }

        public void StartToAddScope(string request)
        {
            var evt = new Event
            {
                Id = 750,
                Task = Tasks.Scope,
                Message = $"Start to add scope: {request}"
            };

            LogInformation(evt);
        }

        public void FinishToAddScope(string result)
        {
            var evt = new Event
            {
                Id = 751,
                Task = Tasks.Scope,
                Message = $"Finish to add scope: {result}"
            };

            LogInformation(evt);
        }

        public void StartToRemoveScope(string scope)
        {
            var evt = new Event
            {
                Id = 752,
                Task = Tasks.Scope,
                Message = $"Start to remove scope: {scope}"
            };

            LogInformation(evt);
        }

        public void FinishToRemoveScope(string scope)
        {
            var evt = new Event
            {
                Id = 753,
                Task = Tasks.Scope,
                Message = $"Finish to remove scope: {scope}"
            };

            LogInformation(evt);
        }
    }
}
