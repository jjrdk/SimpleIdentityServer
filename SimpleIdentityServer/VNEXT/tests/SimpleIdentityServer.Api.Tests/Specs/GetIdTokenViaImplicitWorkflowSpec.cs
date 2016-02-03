﻿using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

using SimpleIdentityServer.Host.DTOs.Request;
using SimpleIdentityServer.Host.DTOs.Response;
using SimpleIdentityServer.Host.Extensions;
using SimpleIdentityServer.Api.Tests.Common;
using SimpleIdentityServer.Core.Common.Extensions;
using SimpleIdentityServer.Core.Jwt.Encrypt;
using SimpleIdentityServer.Core.Jwt.Signature;
using SimpleIdentityServer.DataAccess.Fake.Extensions;
using SimpleIdentityServer.RateLimitation.Configuration;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;
using MODELS = SimpleIdentityServer.DataAccess.Fake.Models;

using System.Web;
using System.Web.Script.Serialization;
using SimpleIdentityServer.Core.Jwt;
using SimpleIdentityServer.Api.Tests.Common.Fakes;
using SimpleIdentityServer.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleIdentityServer.Api.Tests.Specs
{
    [Binding, Scope(Feature = "GetIdTokenViaImplicitWorkflow")]
    public sealed class GetIdTokenViaImplicitWorkflowSpec
    {
        private readonly GlobalContext _globalContext;

        private readonly FakeHttpClientFactory _fakeHttpClientFactory;

        private HttpResponseMessage _httpResponseMessage;

        private MODELS.ResourceOwner _resourceOwner;

        private JwsProtectedHeader _jwsProtectedHeader;

        private string _jws;

        private JwsPayload _jwsPayLoad;

        private string _signedPayLoad;

        private string _combinedHeaderAndPayload;

        private AuthorizationRequest _authorizationRequest;

        private string _request;

        public GetIdTokenViaImplicitWorkflowSpec(GlobalContext context)
        {
            var fakeGetRateLimitationElementOperation = new FakeGetRateLimitationElementOperation
            {
                Enabled = false
            };

            _fakeHttpClientFactory = new FakeHttpClientFactory();
            
            _globalContext = context;
            _globalContext.CreateServer(services =>
            {                
                services.AddInstance<IGetRateLimitationElementOperation>(fakeGetRateLimitationElementOperation);
                services.AddTransient<ISimpleIdentityServerConfigurator, SimpleIdentityServerConfigurator>();
            });
        }

        [Given("create a resource owner")]
        public void GivenCreateAResourceOwner(Table table)
        {
            _resourceOwner = table.CreateInstance<MODELS.ResourceOwner>();
        }

        [Given("the following address is assigned to the resource owner")]
        public void GivenTheAddressIsAssignedToTheAuthenticatedResourceOwner(Table table)
        {
            var address = table.CreateInstance<MODELS.Address>();
            _resourceOwner.Address = address;
        }

        [Given("authenticate the resource owner")]
        public void GivenAuthenticateTheResourceOwner()
        {
            _globalContext.FakeDataSource.ResourceOwners.Add(_resourceOwner);
            _globalContext.AuthenticationMiddleWareOptions.IsEnabled = true;
            _globalContext.AuthenticationMiddleWareOptions.ResourceOwner = _resourceOwner;
        }

        [Given("create an authorization request")]
        public void GivenCreateAnAuthorizationRequest(Table table)
        {
            _authorizationRequest = table.CreateInstance<AuthorizationRequest>();
        }

        [Given("sign the authorization request with (.*) kid and algorithm (.*)")]
        public void GivenSignTheAuthorizationRequestWithKid(string kid, JwsAlg jwsAlg)
        {
            var jsonWebKey = _globalContext.FakeDataSource.JsonWebKeys.FirstOrDefault(j => j.Kid == kid);
            var jwsPayload = _authorizationRequest.ToJwsPayload();
            Assert.NotNull(jsonWebKey);
            var jwsGenerator = _globalContext.ServiceProvider.GetService<IJwsGenerator>();
            _request = jwsGenerator.Generate(jwsPayload, jwsAlg, jsonWebKey.ToBusiness());

           _globalContext.FakeDataSource.Clients.First().JsonWebKeys = _globalContext.FakeDataSource.JsonWebKeys;
        }

        [Given("encrypt the authorization request with (.*) kid, JweAlg: (.*) and JweEnc: (.*)")]
        public void GivenSignTheAuthorizationRequestWithKid(string kid, JweAlg jweAlg, JweEnc jweEnc)
        {
            var jsonWebKey = _globalContext.FakeDataSource.JsonWebKeys.FirstOrDefault(j => j.Kid == kid);
            Assert.NotNull(jsonWebKey);
            var jweGenerator = _globalContext.ServiceProvider.GetService<IJweGenerator>();
            _request = jweGenerator.GenerateJwe(_request, jweAlg, jweEnc, jsonWebKey.ToBusiness());
        }

        [Given("set the request parameter with signed AND/OR encrypted authorization request")]
        public void GivenSetTheRequestParameterWithEncryptedAndOrSignedAuthorizationRequest()
        {
            _authorizationRequest.request = _request;
        }

        [When("requesting an authorization")]
        public void WhenRequestingAnAuthorizationCode()
        {            
            var httpClient = _globalContext.TestServer.CreateClient();
            _fakeHttpClientFactory.HttpClient = httpClient;
            var url = string.Format(
                "/authorization?scope={0}&response_type={1}&client_id={2}&redirect_uri={3}&prompt={4}&state={5}&nonce={6}&claims={7}&request={8}&request_uri={9}",
                _authorizationRequest.scope,
                _authorizationRequest.response_type,
                _authorizationRequest.client_id,
                _authorizationRequest.redirect_uri,
                _authorizationRequest.prompt,
                _authorizationRequest.state,
                _authorizationRequest.nonce,
                _authorizationRequest.claims,
                _authorizationRequest.request,
                _authorizationRequest.request_uri);
            _httpResponseMessage = httpClient.GetAsync(url).Result;
        }
        
        [Then("the http status code is (.*)")]
        public void ThenHttpStatusCodeIsCorrect(HttpStatusCode code)
        {
            Assert.True(code.Equals(_httpResponseMessage.StatusCode));
        }

        [Then("the error code is (.*)")]
        public void ThenTheErrorCodeIs(string errorCode)
        {
            var errorResponse = _httpResponseMessage.Content.ReadAsAsync<ErrorResponse>().Result;
            Assert.NotNull(errorResponse);
            Assert.True(errorResponse.error.Equals(errorCode));
        }

        [Then("redirect to (.*) controller")]
        public void ThenRedirectToController(string controller)
        {
            var location = _httpResponseMessage.Headers.Location;
            Assert.True(location.AbsolutePath.Equals(controller));
        }

        [Then("decrypt the id_token parameter from the fragment")]
        public void ThenDecryptTheIdTokenFromTheQueryString()
        {
            var location = _httpResponseMessage.Headers.Location;
            var query = HttpUtility.ParseQueryString(location.Fragment.TrimStart('#'));
            var idToken = query["id_token"];

            Assert.NotNull(idToken);

            var parts = idToken.Split('.');

            Assert.True(parts.Count() >= 3);
            _jws = idToken;

            var secondPart = parts[1].Base64Decode();

            var javascriptSerializer = new JavaScriptSerializer();
            _jwsProtectedHeader = javascriptSerializer.Deserialize<JwsProtectedHeader>(parts[0].Base64Decode());
            _jwsPayLoad = javascriptSerializer.Deserialize<JwsPayload>(secondPart);
            _combinedHeaderAndPayload = parts[0] + "." + parts[1];
            _signedPayLoad = parts[2];
        }

        [Then("decrypt the jwe parameter from the fragment with the following kid (.*)")]
        public void ThenDecryptTheJweParameterFromTheQueryString(string kid)
        {
            var location = _httpResponseMessage.Headers.Location;
            var query = HttpUtility.ParseQueryString(location.Fragment.TrimStart('#'));
            var idToken = query["id_token"];

            Assert.NotNull(idToken);

            var jsonWebKey = _globalContext.FakeDataSource.JsonWebKeys.FirstOrDefault(j => j.Kid == kid);
            Assert.NotNull(jsonWebKey);

            var jweParser = _globalContext.ServiceProvider.GetService<IJweParser>();
            var result = jweParser.Parse(idToken, jsonWebKey.ToBusiness());
            _jws = result;
            var parts = result.Split('.');

            Assert.True(parts.Count() >= 3);

            var secondPart = parts[1].Base64Decode();

            var javascriptSerializer = new JavaScriptSerializer();
            _jwsProtectedHeader = javascriptSerializer.Deserialize<JwsProtectedHeader>(parts[0].Base64Decode());
            _jwsPayLoad = javascriptSerializer.Deserialize<JwsPayload>(secondPart);
            _combinedHeaderAndPayload = parts[0] + "." + parts[1];
            _signedPayLoad = parts[2];
        }

        [Then("check the signature is correct with the kid (.*)")]
        public void ThenCheckSignatureIsCorrectWithKid(string kid)
        {
            var jsonWebKey = _globalContext.FakeDataSource.JsonWebKeys.FirstOrDefault(j => j.Kid == kid);
            Assert.NotNull(jsonWebKey);

            var jwsParser = _globalContext.ServiceProvider.GetService<IJwsParser>();
            var result = jwsParser.ValidateSignature(_jws, jsonWebKey.ToBusiness());

            Assert.NotNull(result);
        }

        [Then("the signature of the JWS payload is valid")]
        public void ThenTheSignatureIsCorrect()
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                var serializedRsa = _globalContext.FakeDataSource.JsonWebKeys.First().SerializedKey;
                provider.FromXmlString(serializedRsa);
                var signature = _signedPayLoad.Base64DecodeBytes();
                var payLoad = ASCIIEncoding.ASCII.GetBytes(_combinedHeaderAndPayload);
                var signatureIsCorrect = provider.VerifyData(payLoad, "SHA256", signature);
                Assert.True(signatureIsCorrect);
            }
        }

        [Then("the protected JWS header is returned")]
        public void ThenProtectedJwsHeaderIsReturned(Table table)
        {
            var record = table.CreateInstance<JwsProtectedHeader>();

            Assert.True(record.Alg.Equals(_jwsProtectedHeader.Alg));
        }

        [Then("the audience parameter with value (.*) is returned by the JWS payload")]
        public void ThenAudienceIsReturnedInJwsPayLoad(string audience)
        {
            Assert.True(_jwsPayLoad.Audiences.Contains(audience));
        }

        [Then("the parameter nonce with value (.*) is returned by the JWS payload")]
        public void ThenNonceIsReturnedInJwsPayLoad(string nonce)
        {
            Assert.True(_jwsPayLoad.Nonce.Equals(nonce));
        }

        [Then("the claim (.*) with value (.*) is returned by the JWS payload")]
        public void ThenTheClaimWithValueIsReturnedByJwsPayLoad(string claimName, string val)
        {
            var claimValue = _jwsPayLoad.GetClaimValue(claimName);

            Assert.NotNull(claimValue);
            Assert.True(claimValue.Equals(val));
        }

        [Then("the JWS payload contains (.*) claims")]
        public void ThenTheJwsPayloadContainsNumberOfClaims(int numberOfClaims)
        {
            Assert.True(_jwsPayLoad.Count.Equals(numberOfClaims));
        }

        [Then("the callback contains the following query name (.*)")]
        public void ThenTheCallbackContainsTheQueryName(string queryName)
        {
            var location = _httpResponseMessage.Headers.Location;
            var query = HttpUtility.ParseQueryString(location.Fragment.TrimStart('#'));
            var queryValue = query[queryName];
            Assert.NotEmpty(queryValue);
        }

        private MODELS.Client GetClient(string clientId)
        {
            var client = _globalContext.FakeDataSource.Clients.SingleOrDefault(c => c.ClientId == clientId);
            return client;
        }
    }
}
