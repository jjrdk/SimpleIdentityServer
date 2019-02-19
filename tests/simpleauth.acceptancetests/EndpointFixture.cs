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

namespace SimpleAuth.AcceptanceTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Logging;
    using Xunit;

    public class EndpointFixture
    {
        private const string BaseUrl = "http://localhost:5000";
        private readonly TestServerFixture _server;

        public EndpointFixture()
        {
            IdentityModelEventSource.ShowPII = true;
            _server = new TestServerFixture(BaseUrl);
        }

        [Theory]
        [InlineData("", HttpStatusCode.MovedPermanently)]
        [InlineData("home", HttpStatusCode.MovedPermanently)]
        [InlineData(".well-known/openid-configuration", HttpStatusCode.OK)]
        [InlineData("authenticate", HttpStatusCode.OK)]
        public async Task WhenRequestingEndpointThenReturnsExpectedStatus(string path, HttpStatusCode statusCode)
        {
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseUrl}/{path}")
            };

            var httpResult = await _server.Client.SendAsync(httpRequest).ConfigureAwait(false);

            Assert.Equal(statusCode, httpResult.StatusCode);
        }
    }
}
