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

namespace SimpleAuth.Api.PolicyController
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using SimpleAuth.Shared;
    using SimpleAuth.Shared.Errors;
    using SimpleAuth.Shared.Models;
    using SimpleAuth.Shared.Repositories;

    internal class DeleteAuthorizationPolicyAction
    {
        private readonly IPolicyRepository _policyRepository;

        public DeleteAuthorizationPolicyAction(
            IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        public async Task<bool> Execute(string policyId, CancellationToken cancellationToken)
        {
            Policy policy;
            try
            {
                policy = await _policyRepository.Get(policyId, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SimpleAuthException(
                    ErrorCodes.InternalError,
                    string.Format(ErrorDescriptions.TheAuthorizationPolicyCannotBeRetrieved, policyId),
                    ex);
            }

            if (policy == null)
            {
                return false;
            }

            try
            {
                return await _policyRepository.Delete(policyId, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SimpleAuthException(
                    ErrorCodes.InternalError,
                    string.Format(ErrorDescriptions.TheAuthorizationPolicyCannotBeUpdated, policyId),
                    ex);
            }
        }
    }
}