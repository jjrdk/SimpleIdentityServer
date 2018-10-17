﻿// Copyright 2016 Habart Thierry
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

namespace SimpleIdentityServer.Uma.Client.ResourceSet
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Common.DTOs;
    using Newtonsoft.Json;
    using Results;
    using SimpleIdentityServer.Common.Dtos.Responses;

    public class AddResourceSetOperation : IAddResourceSetOperation
    {
        private readonly HttpClient _httpClientFactory;

        public AddResourceSetOperation(HttpClient httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AddResourceSetResult> ExecuteAsync(PostResourceSet request, string url, string token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            var serializedPostResourceSet = JsonConvert.SerializeObject(request);
            var body = new StringContent(serializedPostResourceSet, Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage
            {
                Content = body,
                Method = HttpMethod.Post,
                RequestUri = new Uri(url)
            };
            httpRequest.Headers.Add("Authorization", "Bearer " + token);
            var httpResult = await _httpClientFactory.SendAsync(httpRequest).ConfigureAwait(false);
            var content = await httpResult.Content.ReadAsStringAsync().ConfigureAwait(false);
            try
            {
                httpResult.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return new AddResourceSetResult
                {
                    ContainsError = true,
                    Error = JsonConvert.DeserializeObject<ErrorResponse>(content),
                    HttpStatus = httpResult.StatusCode
                };
            }

            return new AddResourceSetResult
            {
                Content = JsonConvert.DeserializeObject<AddResourceSetResponse>(content)
            };
        }
    }
}
