﻿// Copyright 2015 Habart Thierry
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

using SimpleIdentityServer.Uma.Core.Errors;
using SimpleIdentityServer.Uma.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Uma.Core.Helpers
{
    internal class RepositoryExceptionHelper : IRepositoryExceptionHelper
    {
        public Task<T> HandleException<T>(string message, Func<Task<T>> callback)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            try
            {
                return callback();
            }
            catch (Exception ex)
            {
                throw new BaseUmaException(ErrorCodes.InternalError,
                    message,
                    ex);
            }
        }
    }
}
