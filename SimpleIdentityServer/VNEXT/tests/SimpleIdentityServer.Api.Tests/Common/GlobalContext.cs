﻿using System.Linq;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.Unity;
using Moq;
using SimpleIdentityServer.Api.Configuration;
using SimpleIdentityServer.Host.Parsers;
using SimpleIdentityServer.Api.Tests.Common.Fakes;
using SimpleIdentityServer.Core.Api.Authorization;
using SimpleIdentityServer.Core.Api.Authorization.Actions;
using SimpleIdentityServer.Core.Api.Authorization.Common;
using SimpleIdentityServer.Core.Api.Discovery;
using SimpleIdentityServer.Core.Api.Discovery.Actions;
using SimpleIdentityServer.Core.Api.Jwks;
using SimpleIdentityServer.Core.Api.Jwks.Actions;
using SimpleIdentityServer.Core.Api.Token;
using SimpleIdentityServer.Core.Api.Token.Actions;
using SimpleIdentityServer.Core.Authenticate;
using SimpleIdentityServer.Core.Common;
using SimpleIdentityServer.Core.Factories;
using SimpleIdentityServer.Core.Helpers;
using SimpleIdentityServer.Core.Jwt.Converter;
using SimpleIdentityServer.Core.Jwt.Encrypt;
using SimpleIdentityServer.Core.Jwt.Mapping;
using SimpleIdentityServer.Core.Jwt.Signature;
using SimpleIdentityServer.Core.Protector;
using SimpleIdentityServer.Core.Repositories;
using SimpleIdentityServer.Core.Validators;
using SimpleIdentityServer.Core.WebSite.Authenticate;
using SimpleIdentityServer.Core.WebSite.Authenticate.Actions;
using SimpleIdentityServer.Core.WebSite.Authenticate.Common;
using SimpleIdentityServer.Core.WebSite.Consent;
using SimpleIdentityServer.Core.WebSite.Consent.Actions;
using SimpleIdentityServer.DataAccess.Fake.Repositories;
using SimpleIdentityServer.Logging;
using SimpleIdentityServer.RateLimitation.Configuration;
using SimpleIdentityServer.Core.JwtToken;
using System;
using SimpleIdentityServer.Core.Jwt.Encrypt.Encryption;
using SimpleIdentityServer.Core.Api.UserInfo;
using SimpleIdentityServer.Core.Api.UserInfo.Actions;
using SimpleIdentityServer.Core.Translation;
using SimpleIdentityServer.Core.Jwt.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.TestHost;
using Microsoft.AspNet.Builder;

namespace SimpleIdentityServer.Api.Tests.Common
{
    public class GlobalContext
    {
        private readonly ISimpleIdentityServerEventSource _simpleIdentityServerEventSource;
        
        private readonly ICacheManagerProvider _cacheManagerProvider;

        #region Constructor

        public GlobalContext()
        {
            var cache = new Cache(new NullBackingStore(),
                new CachingInstrumentationProvider("apiCache", false, false, "simpleIdServer"));
            var instrumentationProvider = new CachingInstrumentationProvider("apiCache", false, false,
                new NoPrefixNameFormatter());
            var cacheManager = new CacheManager(cache, new BackgroundScheduler(new ExpirationTask(cache, instrumentationProvider), new ScavengerTask(
                10, 
                1000, 
                cache, 
                instrumentationProvider), 
                instrumentationProvider), new ExpirationPollTimer(1));

            _cacheManagerProvider = new FakeCacheManagerProvider
            {
                CacheManager = cacheManager
            };
            _simpleIdentityServerEventSource = new Mock<ISimpleIdentityServerEventSource>().Object;
            var serviceCollection = new ServiceCollection();
            ConfigureServiceCollection(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
        
        #endregion
        
        #region Properties
        
        public TestServer TestServer { get; private set;}
        
        public IServiceProvider ServiceProvider { get; private set;}
        
        
        #endregion
        
        #region Public methods
        
        
        public void CreateServer(Action<IServiceCollection> callback) 
        {
            TestServer = TestServer.Create(app => {
                app.UseMvc();
            }, services => {
                ConfigureServiceCollection(services);
                if (callback != null) {
                    callback(services);
                }
            });
        }
        
        #endregion
        
        #region Private methods

        private void ConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICacheManager, CacheManager>();
            serviceCollection.AddTransient<ISecurityHelper, SecurityHelper>();
            serviceCollection.AddTransient<IClientHelper, ClientHelper>();
            serviceCollection.AddTransient<IConsentHelper, ConsentHelper>();
            serviceCollection.AddTransient<IAuthorizationFlowHelper, AuthorizationFlowHelper>();
            serviceCollection.AddTransient<IGrantedTokenGeneratorHelper, GrantedTokenGeneratorHelper>();            
            serviceCollection.AddTransient<IUserInfoActions, UserInfoActions>();
            serviceCollection.AddTransient<IGetJwsPayload, GetJwsPayload>();
            serviceCollection.AddTransient<IClientValidator, ClientValidator>();
            serviceCollection.AddTransient<IResourceOwnerValidator, ResourceOwnerValidator>();
            serviceCollection.AddTransient<IScopeValidator, ScopeValidator>();
            serviceCollection.AddTransient<IGrantedTokenValidator, GrantedTokenValidator>();
            serviceCollection.AddTransient<IAuthorizationCodeGrantTypeParameterAuthEdpValidator, AuthorizationCodeGrantTypeParameterAuthEdpValidator>();
            serviceCollection.AddTransient<IResourceOwnerValidator, ResourceOwnerValidator>();
            serviceCollection.AddTransient<IResourceOwnerGrantTypeParameterValidator, ResourceOwnerGrantTypeParameterValidator>();
            serviceCollection.AddTransient<IAuthorizationCodeGrantTypeParameterTokenEdpValidator,AuthorizationCodeGrantTypeParameterTokenEdpValidator>();
            serviceCollection.AddTransient<IClientRepository, FakeClientRepository>();
            serviceCollection.AddTransient<IScopeRepository, FakeScopeRepository>();
            serviceCollection.AddTransient<IResourceOwnerRepository, FakeResourceOwnerRepository>();
            serviceCollection.AddTransient<IGrantedTokenRepository, FakeGrantedTokenRepository>();
            serviceCollection.AddTransient<IConsentRepository, FakeConsentRepository>();
            serviceCollection.AddTransient<IAuthorizationCodeRepository, FakeAuthorizationCodeRepository>();
            serviceCollection.AddTransient<IJsonWebKeyRepository, FakeJsonWebKeyRepository>();
            serviceCollection.AddTransient<ITranslationRepository, FakeTranslationRepository>();
            serviceCollection.AddTransient<IParameterParserHelper, ParameterParserHelper>();
            serviceCollection.AddTransient<IActionResultFactory, ActionResultFactory>();
            serviceCollection.AddTransient<IHttpClientFactory, HttpClientFactory>();
            serviceCollection.AddTransient<IAuthorizationActions, AuthorizationActions>();
            serviceCollection.AddTransient<IGetAuthorizationCodeOperation, GetAuthorizationCodeOperation>();
            serviceCollection.AddTransient<IGetTokenViaImplicitWorkflowOperation, GetTokenViaImplicitWorkflowOperation>();
            serviceCollection.AddTransient<ITokenActions, TokenActions>();
            serviceCollection.AddTransient<IGetTokenByResourceOwnerCredentialsGrantTypeAction, GetTokenByResourceOwnerCredentialsGrantTypeAction>();
            serviceCollection.AddTransient<IGetTokenByAuthorizationCodeGrantTypeAction, GetTokenByAuthorizationCodeGrantTypeAction>();
            serviceCollection.AddTransient<IGetAuthorizationCodeAndTokenViaHybridWorkflowOperation, GetAuthorizationCodeAndTokenViaHybridWorkflowOperation>();
            serviceCollection.AddTransient<IConsentActions, ConsentActions>();
            serviceCollection.AddTransient<IConfirmConsentAction, ConfirmConsentAction>();
            serviceCollection.AddTransient<IDisplayConsentAction, DisplayConsentAction>();
            serviceCollection.AddTransient<IAuthenticateActions, AuthenticateActions>();
            serviceCollection.AddTransient<IAuthenticateResourceOwnerAction, AuthenticateResourceOwnerAction>();
            serviceCollection.AddTransient<ILocalUserAuthenticationAction, LocalUserAuthenticationAction>();
            serviceCollection.AddTransient<IRedirectInstructionParser, RedirectInstructionParser>();
            serviceCollection.AddTransient<IActionResultParser, ActionResultParser>();
            serviceCollection.AddTransient<IDiscoveryActions, DiscoveryActions>();
            serviceCollection.AddTransient<ICreateDiscoveryDocumentationAction, CreateDiscoveryDocumentationAction>();
            serviceCollection.AddTransient<IJwksActions, JwksActions>();
            serviceCollection.AddTransient<IRotateJsonWebKeysOperation, RotateJsonWebKeysOperation>();
            serviceCollection.AddTransient<IGetSetOfPublicKeysUsedToValidateJwsAction, GetSetOfPublicKeysUsedToValidateJwsAction>();
            serviceCollection.AddTransient<IJsonWebKeyEnricher, JsonWebKeyEnricher>();
            serviceCollection.AddTransient<IGetSetOfPublicKeysUsedByTheClientToEncryptJwsTokenAction, GetSetOfPublicKeysUsedByTheClientToEncryptJwsTokenAction>();
            serviceCollection.AddTransient<IProtector, FakeProtector>();
            serviceCollection.AddTransient<IEncoder, Encoder>();
            serviceCollection.AddTransient<ICertificateStore, CertificateStore>();
            serviceCollection.AddTransient<ICompressor, Compressor>();
            serviceCollection.AddTransient<IProcessAuthorizationRequest, ProcessAuthorizationRequest>();
            serviceCollection.AddTransient<IJwsGenerator, JwsGenerator>();
            serviceCollection.AddTransient<IJweGenerator, JweGenerator>();
            serviceCollection.AddTransient<IJwtParser, JwtParser>();
            serviceCollection.AddTransient<IJweParser, JweParser>();
            serviceCollection.AddTransient<IJweHelper, JweHelper>();
            serviceCollection.AddTransient<IJwtGenerator, JwtGenerator>();
            serviceCollection.AddTransient<IAesEncryptionHelper, AesEncryptionHelper>();
            serviceCollection.AddTransient<IJwsParser, JwsParser>();
            serviceCollection.AddTransient<ICreateJwsSignature, CreateJwsSignature>();
            serviceCollection.AddTransient<IGenerateAuthorizationResponse, GenerateAuthorizationResponse>();
            serviceCollection.AddTransient<IClaimsMapping, ClaimsMapping>();
            serviceCollection.AddTransient<IAuthenticateClient, AuthenticateClient>();
            serviceCollection.AddTransient<IAuthenticateHelper, AuthenticateHelper>();
            serviceCollection.AddTransient<IClientSecretBasicAuthentication, ClientSecretBasicAuthentication>();
            serviceCollection.AddTransient<IClientSecretPostAuthentication, ClientSecretPostAuthentication>();
            serviceCollection.AddTransient<IClientAssertionAuthentication, ClientAssertionAuthentication>();
            serviceCollection.AddTransient<IGetTokenByRefreshTokenGrantTypeAction, GetTokenByRefreshTokenGrantTypeAction>();
            serviceCollection.AddTransient<IRefreshTokenGrantTypeParameterValidator, RefreshTokenGrantTypeParameterValidator>();
            serviceCollection.AddTransient<IJsonWebKeyConverter, JsonWebKeyConverter>();            
            serviceCollection.AddTransient<ICngKeySerializer, CngKeySerializer>();
            serviceCollection.AddInstance(_simpleIdentityServerEventSource);
            serviceCollection.AddTransient<ITranslationManager, TranslationManager>();
            serviceCollection.AddInstance(_cacheManagerProvider);
        }
        
        #endregion
    }
}
