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

namespace SimpleAuth.Api.Scopes.Actions
{
    using System;
    using System.Threading.Tasks;
    using Errors;
    using Exceptions;
    using Shared.Models;
    using Shared.Repositories;

    internal class GetScopeOperation : IGetScopeOperation
    {
        private readonly IScopeRepository _scopeRepository;

        public GetScopeOperation(IScopeRepository scopeRepository)
        {
            _scopeRepository = scopeRepository;
        }
        
        public async Task<Scope> Execute(string scopeName)
        {
            if (string.IsNullOrWhiteSpace(scopeName))
            {
                throw new ArgumentNullException(nameof(scopeName));
            }

            var result = await _scopeRepository.Get(scopeName).ConfigureAwait(false);
            if (result == null)
            {
                throw new IdentityServerManagerException(ErrorCodes.InvalidRequestCode,
                    string.Format(ErrorDescriptions.TheScopeDoesntExist, scopeName));
            }

            return result;
        }
    }
}
