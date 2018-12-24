﻿namespace SimpleIdentityServer.Manager.Client.Scopes
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Results;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using SimpleAuth.Shared.Responses;

    internal sealed class GetScopeOperation : IGetScopeOperation
    {
        private readonly HttpClient _httpClientFactory;

        public GetScopeOperation(HttpClient httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetScopeResult> ExecuteAsync(Uri scopesUri, string authorizationHeaderValue = null)
        {
            if (scopesUri == null)
            {
                throw new ArgumentNullException(nameof(scopesUri));
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = scopesUri
            };
            if (!string.IsNullOrWhiteSpace(authorizationHeaderValue))
            {
                request.Headers.Add("Authorization", "Bearer " + authorizationHeaderValue);
            }

            var httpResult = await _httpClientFactory.SendAsync(request).ConfigureAwait(false);
            var content = await httpResult.Content.ReadAsStringAsync().ConfigureAwait(false);
            var rec = JObject.Parse(content);
            try
            {
                httpResult.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return new GetScopeResult
                {
                    ContainsError = true,
                    Error = JsonConvert.DeserializeObject<ErrorResponse>(content),
                    HttpStatus = httpResult.StatusCode
                };
            }

            return new GetScopeResult
            {
                Content = JsonConvert.DeserializeObject<ScopeResponse>(content)
            };
        }
    }
}
