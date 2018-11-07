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

using SimpleIdentityServer.Core.Authenticate;
using SimpleIdentityServer.Core.Errors;
using SimpleIdentityServer.Core.Exceptions;
using SimpleIdentityServer.Core.Parameters;
using System;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Core.Api.Token.Actions
{
    using Jwt;
    using Shared.Models;

    internal class RevokeTokenAction : IRevokeTokenAction
    {
        private readonly IAuthenticateClient _authenticateClient;
        private readonly ITokenStore _tokenStore;

        public RevokeTokenAction(
            IAuthenticateClient authenticateClient,
            ITokenStore tokenStore)
        {
            _authenticateClient = authenticateClient;
            _tokenStore = tokenStore;
        }

        public async Task<bool> Execute(RevokeTokenParameter revokeTokenParameter, AuthenticationHeaderValue authenticationHeaderValue, X509Certificate2 certificate, string issuerName)
        {
            if (revokeTokenParameter == null)
            {
                throw new ArgumentNullException(nameof(revokeTokenParameter));
            }

            if (string.IsNullOrWhiteSpace(revokeTokenParameter.Token))
            {
                throw new ArgumentNullException(nameof(revokeTokenParameter.Token));
            }
            
            // 1. Check the client credentials
            var instruction = authenticationHeaderValue.GetAuthenticateInstruction(revokeTokenParameter, certificate);
            var authResult = await _authenticateClient.AuthenticateAsync(instruction, issuerName).ConfigureAwait(false);
            var client = authResult.Client;
            if (client == null)
            {
                throw new IdentityServerException(ErrorCodes.InvalidClient, authResult.ErrorMessage);
            }

            // 2. Retrieve the granted token & check if it exists
            GrantedToken grantedToken = await _tokenStore.GetAccessToken(revokeTokenParameter.Token).ConfigureAwait(false);
            bool isAccessToken = true;
            if (grantedToken == null)
            {
                grantedToken = await _tokenStore.GetRefreshToken(revokeTokenParameter.Token).ConfigureAwait(false);
                isAccessToken = false;
            }

            if (grantedToken == null)
            {
                throw new IdentityServerException(ErrorCodes.InvalidToken, ErrorDescriptions.TheTokenDoesntExist);
            }

            // 3. Verifies whether the token was issued to the client making the revocation request
            if (grantedToken.ClientId != client.ClientId)
            {
                throw new IdentityServerException(ErrorCodes.InvalidToken, string.Format(ErrorDescriptions.TheTokenHasNotBeenIssuedForTheGivenClientId, client.ClientId));
            }

            // 4. Invalid the granted token
            if (isAccessToken)
            {
                return await _tokenStore.RemoveAccessToken(grantedToken.AccessToken).ConfigureAwait(false);
            }

            return await _tokenStore.RemoveRefreshToken(grantedToken.RefreshToken).ConfigureAwait(false);
        }
    }
}
