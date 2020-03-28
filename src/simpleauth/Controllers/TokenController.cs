﻿namespace SimpleAuth.Controllers
{
    using Api.Token;
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Shared;
    using Shared.Models;
    using SimpleAuth.Common;
    using SimpleAuth.Shared.Errors;
    using SimpleAuth.Shared.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Net.Http.Headers;
    using SimpleAuth.Filters;

    /// <summary>
    /// Defines the token controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route(UmaConstants.RouteValues.Token)]
    [ThrottleFilter]
    public class TokenController : ControllerBase
    {
        private readonly TokenActions _tokenActions;
        private readonly UmaTokenActions _umaTokenActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="authorizationCodeStore">The authorization code store.</param>
        /// <param name="clientStore">The client store.</param>
        /// <param name="scopeRepository">The scope repository.</param>
        /// <param name="resourceOwnerRepository"></param>
        /// <param name="authenticateResourceOwnerServices">The authenticate resource owner services.</param>
        /// <param name="tokenStore">The token store.</param>
        /// <param name="ticketStore">The ticket store.</param>
        /// <param name="jwksStore"></param>
        /// <param name="resourceSetRepository">The resource set repository.</param>
        /// <param name="eventPublisher">The event publisher.</param>
        public TokenController(
            RuntimeSettings settings,
            IAuthorizationCodeStore authorizationCodeStore,
            IClientStore clientStore,
            IScopeRepository scopeRepository,
            IResourceOwnerRepository resourceOwnerRepository,
            IEnumerable<IAuthenticateResourceOwnerService> authenticateResourceOwnerServices,
            ITokenStore tokenStore,
            ITicketStore ticketStore,
            IJwksStore jwksStore,
            IResourceSetRepository resourceSetRepository,
            IEventPublisher eventPublisher)
        {
            _tokenActions = new TokenActions(
                settings,
                authorizationCodeStore,
                clientStore,
                scopeRepository,
                jwksStore,
                resourceOwnerRepository,
                authenticateResourceOwnerServices,
                eventPublisher,
                tokenStore);
            _umaTokenActions = new UmaTokenActions(
                ticketStore,
                settings,
                clientStore,
                scopeRepository,
                tokenStore,
                resourceSetRepository,
                jwksStore,
                eventPublisher);
        }

        /// <summary>
        /// Handles the token request.
        /// </summary>
        /// <param name="tokenRequest">The token request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [HttpPost]
        public async Task<IActionResult> PostToken(
            [FromForm] TokenRequest tokenRequest,
            CancellationToken cancellationToken)
        {
            var certificate = Request.GetCertificate();
            if (tokenRequest.grant_type == null)
            {
                return BadRequest(
                    new ErrorDetails
                    {
                        Status = HttpStatusCode.BadRequest,
                        Title = ErrorCodes.InvalidRequest,
                        Detail = string.Format(ErrorDescriptions.MissingParameter, RequestTokenNames.GrantType)
                    });
            }

            AuthenticationHeaderValue authenticationHeaderValue = null;
            if (Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizationHeader))
            {
                AuthenticationHeaderValue.TryParse(authorizationHeader[0], out authenticationHeaderValue);
            }

            var issuerName = Request.GetAbsoluteUriWithVirtualPath();
            var result = await GetGrantedToken(
                    tokenRequest,
                    cancellationToken,
                    authenticationHeaderValue,
                    certificate,
                    issuerName)
                .ConfigureAwait(false);

            return result.StatusCode == HttpStatusCode.OK
                ? (IActionResult)new OkObjectResult(result.Content.ToDto())
                : new BadRequestObjectResult(result.Error);
        }

        private async Task<GenericResponse<GrantedToken>> GetGrantedToken(
            TokenRequest tokenRequest,
            CancellationToken cancellationToken,
            AuthenticationHeaderValue authenticationHeaderValue,
            X509Certificate2 certificate,
            string issuerName)
        {
            switch (tokenRequest.grant_type)
            {
                case GrantTypes.Password:
                    var resourceOwnerParameter = tokenRequest.ToResourceOwnerGrantTypeParameter();
                    return await _tokenActions.GetTokenByResourceOwnerCredentialsGrantType(
                        resourceOwnerParameter,
                        authenticationHeaderValue,
                        certificate,
                        issuerName,
                        cancellationToken)
                        .ConfigureAwait(false);
                case GrantTypes.AuthorizationCode:
                    var authCodeParameter = tokenRequest.ToAuthorizationCodeGrantTypeParameter();
                    return await _tokenActions.GetTokenByAuthorizationCodeGrantType(
                            authCodeParameter,
                            authenticationHeaderValue,
                            certificate,
                            issuerName,
                            cancellationToken)
                        .ConfigureAwait(false);
                case GrantTypes.RefreshToken:
                    var refreshTokenParameter = tokenRequest.ToRefreshTokenGrantTypeParameter();
                    return await _tokenActions.GetTokenByRefreshTokenGrantType(
                            refreshTokenParameter,
                            authenticationHeaderValue,
                            certificate,
                            issuerName,
                            cancellationToken)
                        .ConfigureAwait(false);
                case GrantTypes.ClientCredentials:
                    var clientCredentialsParameter = tokenRequest.ToClientCredentialsGrantTypeParameter();
                    return await _tokenActions.GetTokenByClientCredentialsGrantType(
                            clientCredentialsParameter,
                            authenticationHeaderValue,
                            certificate,
                            issuerName,
                            cancellationToken)
                        .ConfigureAwait(false);
                case GrantTypes.UmaTicket:
                    var tokenIdParameter = tokenRequest.ToTokenIdGrantTypeParameter();

                    return await _umaTokenActions.GetTokenByTicketId(
                            tokenIdParameter,
                            authenticationHeaderValue,
                            certificate,
                            issuerName,
                            cancellationToken)
                        .ConfigureAwait(false);
                case GrantTypes.ValidateBearer:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Handles the token revocation.
        /// </summary>
        /// <param name="revocationRequest">The revocation request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost("revoke")]
        public async Task<IActionResult> PostRevoke(
            [FromForm] RevocationRequest revocationRequest,
            CancellationToken cancellationToken)
        {
            // 1. Fetch the authorization header
            AuthenticationHeaderValue authenticationHeaderValue = null;
            if (Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizationHeader))
            {
                var authorizationHeaderValue = authorizationHeader.First();
                var splittedAuthorizationHeaderValue = authorizationHeaderValue.Split(' ');
                if (splittedAuthorizationHeaderValue.Length == 2)
                {
                    authenticationHeaderValue = new AuthenticationHeaderValue(
                        splittedAuthorizationHeaderValue[0],
                        splittedAuthorizationHeaderValue[1]);
                }
            }

            // 2. Revoke the token
            var issuerName = Request.GetAbsoluteUriWithVirtualPath();
            var result = await _tokenActions.RevokeToken(
                    revocationRequest.ToParameter(),
                    authenticationHeaderValue,
                    Request.GetCertificate(),
                    issuerName,
                    cancellationToken)
                .ConfigureAwait(false);
            return result ? new OkResult() : StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
