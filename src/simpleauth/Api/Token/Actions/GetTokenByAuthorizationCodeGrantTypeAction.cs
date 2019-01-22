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

using SimpleAuth.Shared.Repositories;

namespace SimpleAuth.Api.Token.Actions
{
    using Authenticate;
    using Errors;
    using Exceptions;
    using Helpers;
    using JwtToken;
    using Logging;
    using Parameters;
    using Shared;
    using Shared.Models;
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Validators;

    public class GetTokenByAuthorizationCodeGrantTypeAction
    {
        private class ValidationResult
        {
            public AuthorizationCode AuthCode { get; set; }
            public Client Client { get; set; }
        }

        private readonly ClientValidator _clientValidator;
        private readonly IAuthorizationCodeStore _authorizationCodeStore;
        private readonly OAuthConfigurationOptions _configurationService;
        private readonly AuthenticateClient _authenticateClient;
        private readonly IEventPublisher _eventPublisher;
        private readonly ITokenStore _tokenStore;
        private readonly IJwtGenerator _jwtGenerator;

        public GetTokenByAuthorizationCodeGrantTypeAction(
            IAuthorizationCodeStore authorizationCodeStore,
            OAuthConfigurationOptions configurationService,
            IClientStore clientStore,
            IEventPublisher eventPublisher,
            ITokenStore tokenStore,
            IJwtGenerator jwtGenerator)
        {
            _clientValidator = new ClientValidator();
            _authorizationCodeStore = authorizationCodeStore;
            _configurationService = configurationService;
            _authenticateClient = new AuthenticateClient(clientStore);
            _eventPublisher = eventPublisher;
            _tokenStore = tokenStore;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<GrantedToken> Execute(AuthorizationCodeGrantTypeParameter authorizationCodeGrantTypeParameter, AuthenticationHeaderValue authenticationHeaderValue, X509Certificate2 certificate, string issuerName)
        {
            if (authorizationCodeGrantTypeParameter == null)
            {
                throw new ArgumentNullException(nameof(authorizationCodeGrantTypeParameter));
            }

            var result = await ValidateParameter(authorizationCodeGrantTypeParameter, authenticationHeaderValue, certificate, issuerName).ConfigureAwait(false);
            await _authorizationCodeStore.RemoveAuthorizationCode(result.AuthCode.Code).ConfigureAwait(false); // 1. Invalidate the authorization code by removing it !
            var grantedToken = await _tokenStore.GetValidGrantedTokenAsync(
                result.AuthCode.Scopes,
                result.AuthCode.ClientId,
                result.AuthCode.IdTokenPayload,
                result.AuthCode.UserInfoPayLoad).ConfigureAwait(false);
            if (grantedToken == null)
            {
                grantedToken = await result.Client.GenerateToken(
                        result.AuthCode.Scopes,
                        issuerName,
                        result.AuthCode.UserInfoPayLoad,
                        result.AuthCode.IdTokenPayload,
                        result.AuthCode.IdTokenPayload?.Claims
                            .Where(c => _configurationService.UserClaimsToIncludeInAuthToken.Contains(c.Type))
                            .ToArray())
                    .ConfigureAwait(false);
                await _eventPublisher.Publish(new AccessToClientGranted(
                    Id.Create(),
                    result.AuthCode.ClientId,
                    grantedToken.AccessToken,
                    grantedToken.IdToken,
                    DateTime.UtcNow)).ConfigureAwait(false);
                // Fill-in the id-token
                if (grantedToken.IdTokenPayLoad != null)
                {
                    _jwtGenerator.UpdatePayloadDate(grantedToken.IdTokenPayLoad, result.Client);
                    grantedToken.IdToken = await result.Client.GenerateIdTokenAsync(grantedToken.IdTokenPayLoad).ConfigureAwait(false);
                }

                await _tokenStore.AddToken(grantedToken).ConfigureAwait(false);
            }

            return grantedToken;
        }

        /// <summary>
        /// Check the parameters based on the RFC : http://openid.net/specs/openid-connect-core-1_0.html#TokenRequestValidation
        /// </summary>
        /// <param name="authorizationCodeGrantTypeParameter"></param>
        /// <param name="authenticationHeaderValue"></param>
        /// <param name="certificate"></param>
        /// <param name="issuerName"></param>
        /// <returns></returns>
        private async Task<ValidationResult> ValidateParameter(
            AuthorizationCodeGrantTypeParameter authorizationCodeGrantTypeParameter,
            AuthenticationHeaderValue authenticationHeaderValue,
            X509Certificate2 certificate,
            string issuerName)
        {
            // 1. Authenticate the client
            var instruction = authenticationHeaderValue.GetAuthenticateInstruction(authorizationCodeGrantTypeParameter, certificate);
            var authResult = await _authenticateClient.Authenticate(instruction, issuerName).ConfigureAwait(false);
            var client = authResult.Client;
            if (client == null)
            {
                throw new SimpleAuthException(ErrorCodes.InvalidClient, authResult.ErrorMessage);
            }

            // 2. Check the client
            if (client.GrantTypes == null || !client.GrantTypes.Contains(GrantType.authorization_code))
            {
                throw new SimpleAuthException(ErrorCodes.InvalidClient, string.Format(ErrorDescriptions.TheClientDoesntSupportTheGrantType, client.ClientId, GrantType.authorization_code));
            }

            if (client.ResponseTypes == null || !client.ResponseTypes.Contains(ResponseTypeNames.Code))
            {
                throw new SimpleAuthException(ErrorCodes.InvalidClient,
                    string.Format(ErrorDescriptions.TheClientDoesntSupportTheResponseType, client.ClientId, ResponseTypeNames.Code));
            }

            var authorizationCode = await _authorizationCodeStore.GetAuthorizationCode(authorizationCodeGrantTypeParameter.Code).ConfigureAwait(false);
            // 2. Check if the authorization code is valid
            if (authorizationCode == null)
            {
                throw new SimpleAuthException(ErrorCodes.InvalidGrant,
                    ErrorDescriptions.TheAuthorizationCodeIsNotCorrect);
            }

            // 3. Check PKCE
            if (!_clientValidator.CheckPkce(client, authorizationCodeGrantTypeParameter.CodeVerifier, authorizationCode))
            {
                throw new SimpleAuthException(ErrorCodes.InvalidGrant, ErrorDescriptions.TheCodeVerifierIsNotCorrect);
            }

            // 4. Ensure the authorization code was issued to the authenticated client.
            var authorizationClientId = authorizationCode.ClientId;
            if (authorizationClientId != client.ClientId)
            {
                throw new SimpleAuthException(ErrorCodes.InvalidGrant,
                    string.Format(ErrorDescriptions.TheAuthorizationCodeHasNotBeenIssuedForTheGivenClientId,
                        client.ClientId));
            }

            if (authorizationCode.RedirectUri != authorizationCodeGrantTypeParameter.RedirectUri)
            {
                throw new SimpleAuthException(ErrorCodes.InvalidGrant,
                    ErrorDescriptions.TheRedirectionUrlIsNotTheSame);
            }

            // 5. Ensure the authorization code is still valid.
            var authCodeValidity = _configurationService.AuthorizationCodeValidityPeriod;
            var expirationDateTime = authorizationCode.CreateDateTime.Add(authCodeValidity);
            var currentDateTime = DateTime.UtcNow;
            if (currentDateTime > expirationDateTime)
            {
                throw new SimpleAuthException(ErrorCodes.InvalidGrant,
                    ErrorDescriptions.TheAuthorizationCodeIsObsolete);
            }

            // Ensure that the redirect_uri parameter value is identical to the redirect_uri parameter value.
            var redirectionUrl = _clientValidator.GetRedirectionUrls(client, authorizationCodeGrantTypeParameter.RedirectUri);
            if (!redirectionUrl.Any())
            {
                throw new SimpleAuthException(
                    ErrorCodes.InvalidGrant,
                    string.Format(ErrorDescriptions.RedirectUrlIsNotValid, authorizationCodeGrantTypeParameter.RedirectUri));
            }

            return new ValidationResult
            {
                Client = client,
                AuthCode = authorizationCode
            };
        }
    }
}
