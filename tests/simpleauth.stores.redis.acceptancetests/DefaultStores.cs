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

namespace SimpleAuth.Stores.Redis.AcceptanceTests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.IdentityModel.Tokens;
    using SimpleAuth;
    using SimpleAuth.Shared;
    using SimpleAuth.Shared.Models;

    public static class DefaultStores
    {
        public static IEnumerable<Consent> Consents()
        {
            return new[]
            {
                new Consent
                {
                    Id = "1",
                    ClientId = "authcode_client",
                    Subject = "administrator",
                    GrantedScopes = new[] {"api1", "openid"}
                },
                new Consent
                {
                    Id = "2",
                    ClientId = "implicit_client",
                    Subject = "administrator",
                    GrantedScopes = new[] {"api1", "openid"}
                },
                new Consent
                {
                    Id = "3",
                    ClientId = "hybrid_client",
                    Subject = "administrator",
                    GrantedScopes = new[] {"api1", "openid"}
                },
                new Consent
                {
                    Id = "4",
                    ClientId = "pkce_client",
                    Subject = "administrator",
                    GrantedScopes = new[] {"api1", "openid"}
                }
            };
        }

        public static IEnumerable<ResourceOwner> Users()
        {
            return new[]
            {
                new ResourceOwner
                {
                    Subject = "administrator",
                    Claims = new[]
                    {
                        new Claim(OpenIdClaimTypes.Subject, "administrator"),
                        new Claim(OpenIdClaimTypes.Role, "administrator"),
                        new Claim(OpenIdClaimTypes.Address, "{ country : 'france' }")
                    },
                    Password = "password".ToSha256Hash(string.Empty),
                    IsLocalAccount = true
                },
                new ResourceOwner
                {
                    Subject = "user",
                    Password = "password".ToSha256Hash(string.Empty),
                    Claims = new[]
                    {
                        new Claim(OpenIdClaimTypes.Subject, "user"),
                        new Claim(OpenIdClaimTypes.Name, "John Doe")
                    },
                    IsLocalAccount = true
                },
                new ResourceOwner
                {
                    Subject = "superuser",
                    Password = "password".ToSha256Hash(string.Empty),
                    Claims = new[]
                    {
                        new Claim(OpenIdClaimTypes.Subject, "superuser"),
                        new Claim(OpenIdClaimTypes.Role, "administrator"),
                        new Claim(OpenIdClaimTypes.Role, "role")
                    },
                    IsLocalAccount = true
                }
            };
        }

        public static IEnumerable<Client> Clients(SharedContext sharedCtx)
        {
            return new[]
            {
                new Client
                {
                    ClientId = "client",
                    ClientName = "client",
                    Secrets = new[] {new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "client"}},
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"openid", "role", "profile", "address", "offline"},
                    GrantTypes = new[] {GrantTypes.RefreshToken, GrantTypes.Password},
                    ResponseTypes = new[] {ResponseTypeNames.Code, ResponseTypeNames.Token, ResponseTypeNames.IdToken},
                    JsonWebKeys =
                        TestKeys.SecretKey.CreateJwk(JsonWebKeyUseNames.Sig, KeyOperations.Sign, KeyOperations.Verify)
                            .ToSet(),
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("https://localhost:4200/callback")}
                },
                new Client
                {
                    ClientId = "client_userinfo_sig_rs256",
                    ClientName = "client_userinfo_sig_rs256",
                    Secrets = new[]
                    {
                        new ClientSecret
                        {
                            Type = ClientSecretTypes.SharedSecret, Value = "client_userinfo_sig_rs256"
                        }
                    },
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"openid", "role", "profile"},
                    GrantTypes = new[] {GrantTypes.RefreshToken, GrantTypes.Password},
                    ResponseTypes = new[] {ResponseTypeNames.Code, ResponseTypeNames.Token, ResponseTypeNames.IdToken},
                    JsonWebKeys = TestKeys.SecretKey.CreateSignatureJwk().ToSet(),
                    IdTokenSignedResponseAlg = SecurityAlgorithms.RsaSha256,
                    UserInfoSignedResponseAlg = SecurityAlgorithms.RsaSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("https://localhost:4200/callback")}
                },
                new Client
                {
                    ClientId = "client_userinfo_enc_rsa15",
                    ClientName = "client_userinfo_enc_rsa15",
                    Secrets = new[]
                    {
                        new ClientSecret
                        {
                            Type = ClientSecretTypes.SharedSecret, Value = "client_userinfo_enc_rsa15"
                        }
                    },
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"openid", "role", "profile"},
                    GrantTypes = new[] {GrantTypes.RefreshToken, GrantTypes.Password},
                    ResponseTypes = new[] {ResponseTypeNames.Code, ResponseTypeNames.Token, ResponseTypeNames.IdToken},
                    JsonWebKeys =
                        new JsonWebKeySet().AddKey(TestKeys.SecretKey.CreateSignatureJwk())
                            .AddKey(TestKeys.SecretKey.CreateEncryptionJwk()),
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    UserInfoSignedResponseAlg = SecurityAlgorithms.RsaSha256,
                    UserInfoEncryptedResponseAlg = SecurityAlgorithms.EcdsaSha256,
                    UserInfoEncryptedResponseEnc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("https://localhost:4200/callback")}
                },
                new Client
                {
                    ClientId = "clientWithWrongResponseType",
                    ClientName = "clientWithWrongResponseType",
                    Secrets = new[]
                    {
                        new ClientSecret
                        {
                            Type = ClientSecretTypes.SharedSecret, Value = "clientWithWrongResponseType"
                        }
                    },
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"openid", "role", "profile"},
                    GrantTypes = new[] {GrantTypes.RefreshToken, GrantTypes.ClientCredentials},
                    ResponseTypes = new[] {ResponseTypeNames.IdToken},
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("https://localhost:4200/callback")}
                },
                new Client
                {
                    ClientId = "clientCredentials",
                    ClientName = "clientCredentials",
                    Secrets = new[]
                    {
                        new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "clientCredentials"}
                    },
                    Claims = new[] {new Claim("test", "test"), new Claim("sub", "ClientCredentials"), },
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"api1", "uma_protection", "offline"},
                    GrantTypes = new[] {GrantTypes.RefreshToken, GrantTypes.ClientCredentials},
                    ResponseTypes = new[] {ResponseTypeNames.Token},
                    JsonWebKeys =
                        TestKeys.SecretKey.CreateJwk(JsonWebKeyUseNames.Sig, KeyOperations.Sign, KeyOperations.Verify)
                            .ToSet(),
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("https://localhost:4200/callback")}
                },
                new Client
                {
                    ClientId = "basic_client",
                    ClientName = "basic_client",
                    Secrets = new[] {new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "basic_client"}},
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretBasic,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"api1"},
                    GrantTypes = new[] {GrantTypes.ClientCredentials},
                    ResponseTypes = new[] {ResponseTypeNames.Token},
                    JsonWebKeys = new JsonWebKeySet().AddKey(TestKeys.SecretKey.CreateSignatureJwk()),
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("https://localhost:4200/callback")}
                },
                new Client
                {
                    ClientId = "post_client",
                    ClientName = "post_client",
                    Secrets = new[] {new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "post_client"}},
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"api1", "uma_protection"},
                    GrantTypes = new[] {GrantTypes.ClientCredentials},
                    ResponseTypes = new[] {ResponseTypeNames.Token},
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("https://localhost:4200/callback")}
                },
                new Client
                {
                    ClientId = "jwt_client",
                    ClientName = "jwt_client",
                    Secrets = new[] {new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "jwt_client"}},
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretJwt,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"api1"},
                    GrantTypes = new[] {GrantTypes.ClientCredentials},
                    ResponseTypes = new[] {ResponseTypeNames.Token},
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("https://localhost:4200/callback")},
                    JsonWebKeys = new[] {sharedCtx.ModelSignatureKey, sharedCtx.ModelEncryptionKey}.ToJwks()
                },
                new Client
                {
                    ClientId = "private_key_client",
                    ClientName = "private_key_client",
                    Secrets = new[]
                    {
                        new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "private_key_client"}
                    },
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.PrivateKeyJwt,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"api1"},
                    GrantTypes = new[] {GrantTypes.ClientCredentials},
                    ResponseTypes = new[] {ResponseTypeNames.Token},
                    JsonWebKeys = new JsonWebKeySet().AddKey(TestKeys.SecretKey.CreateSignatureJwk()),
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256, //SecurityAlgorithms.RsaSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("https://localhost:4200/callback")},
                    //JwksUri = new Uri("http://localhost:5000/jwks_client")
                },
                new Client
                {
                    ClientId = "authcode_client",
                    ClientName = "authcode_client",
                    Secrets = new[]
                    {
                        new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "authcode_client"}
                    },
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"api1", "openid"},
                    GrantTypes = new[] {GrantTypes.AuthorizationCode},
                    ResponseTypes = new[] {ResponseTypeNames.Code, ResponseTypeNames.Token, ResponseTypeNames.IdToken},
                    JsonWebKeys =
                        TestKeys.SecretKey.CreateSignatureJwk()
                            .ToSet()
                            .AddKey(TestKeys.SuperSecretKey.CreateEncryptionJwk()),
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256, //SecurityAlgorithms.RsaSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("http://localhost:5000/callback")}
                },
                new Client
                {
                    ClientId = "incomplete_authcode_client",
                    ClientName = "incomplete_authcode_client",
                    Secrets = new[]
                    {
                        new ClientSecret
                        {
                            Type = ClientSecretTypes.SharedSecret, Value = "incomplete_authcode_client"
                        }
                    },
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"api1", "openid"},
                    GrantTypes = new[] {GrantTypes.AuthorizationCode},
                    ResponseTypes = new[] {ResponseTypeNames.IdToken},
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("http://localhost:5000/callback")}
                },
                new Client
                {
                    ClientId = "implicit_client",
                    ClientName = "implicit_client",
                    Secrets = new[]
                    {
                        new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "implicit_client"}
                    },
                    JsonWebKeys = new JsonWebKeySet().AddKey(TestKeys.SecretKey.CreateSignatureJwk()),
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"api1", "openid"},
                    GrantTypes = new[] {GrantTypes.Implicit},
                    ResponseTypes = new[] {ResponseTypeNames.Token, ResponseTypeNames.IdToken},
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("http://localhost:5000/callback")}
                },
                new Client
                {
                    ClientId = "pkce_client",
                    ClientName = "pkce_client",
                    Secrets = new[] {new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "pkce_client"}},
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"api1", "openid"},
                    GrantTypes = new[] {GrantTypes.AuthorizationCode},
                    ResponseTypes = new[] {ResponseTypeNames.Code, ResponseTypeNames.Token, ResponseTypeNames.IdToken},
                    JsonWebKeys = TestKeys.SecretKey.CreateSignatureJwk().ToSet(),
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("http://localhost:5000/callback")},
                    RequirePkce = true
                },
                new Client
                {
                    ClientId = "hybrid_client",
                    ClientName = "hybrid_client",
                    Secrets = new[] {new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "hybrid_client"}},
                    JsonWebKeys = new JsonWebKeySet().AddKey(TestKeys.SecretKey.CreateSignatureJwk()),
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    PolicyUri = new Uri("http://openid.net"),
                    TosUri = new Uri("http://openid.net"),
                    AllowedScopes = new[] {"api1", "openid"},
                    GrantTypes = new[] {GrantTypes.AuthorizationCode, GrantTypes.Implicit},
                    ResponseTypes = new[] {ResponseTypeNames.Code, ResponseTypeNames.Token, ResponseTypeNames.IdToken},
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256,
                    ApplicationType = ApplicationTypes.Web,
                    RedirectionUrls = new [] {new Uri("http://localhost:5000/callback")},
                },
                // Certificate test client.
                new Client
                {
                    ClientId = "certificate_client",
                    ClientName = "Certificate test client",
                    Secrets =
                        new[]
                        {
                            new ClientSecret
                            {
                                Type = ClientSecretTypes.X509Thumbprint,
                                Value = "0772F57C594FA1EFD619AF8D84A48F4C1741C715"
                            },
                            new ClientSecret
                            {
                                Type = ClientSecretTypes.X509Name, Value = "O=reimers.dk, L=Zurich, S=ZH, C=CH"
                            }
                        },
                    JsonWebKeys =
                        new JsonWebKeySet()
                            .AddKey(
                                new X509Certificate2("mycert.pfx", "simpleauth", X509KeyStorageFlags.Exportable)
                                    .CreateJwk(JsonWebKeyUseNames.Sig, KeyOperations.Sign, KeyOperations.Verify))
                            .AddKey(
                                new X509Certificate2("mycert.pfx", "simpleauth", X509KeyStorageFlags.Exportable)
                                    .CreateJwk(JsonWebKeyUseNames.Enc, KeyOperations.Encrypt, KeyOperations.Decrypt)),
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.TlsClientAuth,
                    //LogoUri = null,
                    AllowedScopes = new[] {"openid"},
                    GrantTypes = new[] {GrantTypes.Password},
                    ResponseTypes = new[] {ResponseTypeNames.Token, ResponseTypeNames.IdToken},
                    IdTokenSignedResponseAlg = SecurityAlgorithms.RsaV15KeyWrap, //SecurityAlgorithms.RsaSha256,
                    ApplicationType = ApplicationTypes.Native
                },
                // Client credentials + stateless access token.
                new Client
                {
                    ClientId = "stateless_client",
                    ClientName = "Stateless client",
                    Secrets = new[]
                    {
                        new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "stateless_client"}
                    },
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    AllowedScopes = new[] {"openid", "register_client", "manage_account_filtering"},
                    GrantTypes = new[] {GrantTypes.ClientCredentials},
                    JsonWebKeys =
                        new JsonWebKeySet().AddKey(
                            TestKeys.SecretKey.CreateJwk(
                                JsonWebKeyUseNames.Sig,
                                KeyOperations.Sign,
                                KeyOperations.Verify)),
                    ResponseTypes = new[] {ResponseTypeNames.Token},
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256, // SecurityAlgorithms.RsaSha256,
                    ApplicationType = ApplicationTypes.Native
                },
                new Client
                {
                    ClientId = "manager_client",
                    ClientName = "Manager client",
                    Secrets =
                        new[] {new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "manager_client"}},
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    AllowedScopes = new[] {"manager"},
                    GrantTypes = new[] {GrantTypes.ClientCredentials},
                    JsonWebKeys =
                        new JsonWebKeySet().AddKey(
                            TestKeys.SecretKey.CreateJwk(
                                JsonWebKeyUseNames.Sig,
                                KeyOperations.Sign,
                                KeyOperations.Verify)),
                    ResponseTypes = new[] {ResponseTypeNames.Token},
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256, // SecurityAlgorithms.RsaSha256,
                    ApplicationType = ApplicationTypes.Native
                },
                new Client
                {
                    ClientId = "admin_client",
                    ClientName = "Admin client",
                    Secrets =
                        new[] {new ClientSecret {Type = ClientSecretTypes.SharedSecret, Value = "admin_client" } },
                    TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.ClientSecretPost,
                    //LogoUri = null,
                    AllowedScopes = new[] {"admin"},
                    GrantTypes = new[] {GrantTypes.ClientCredentials},
                    JsonWebKeys =
                        new JsonWebKeySet().AddKey(
                            TestKeys.SecretKey.CreateJwk(
                                JsonWebKeyUseNames.Sig,
                                KeyOperations.Sign,
                                KeyOperations.Verify)),
                    ResponseTypes = new[] {ResponseTypeNames.Token},
                    IdTokenSignedResponseAlg = SecurityAlgorithms.HmacSha256, // SecurityAlgorithms.RsaSha256,
                    ApplicationType = ApplicationTypes.Native
                }
            };
        }

        public static IEnumerable<Scope> Scopes()
        {
            return new[]
            {
                new Scope
                {
                    Claims = Array.Empty<string>(),
                    Description = "test scope",
                    IsDisplayedInConsent = true,
                    IsExposed = true,
                    Name = "test",
                    Type = ScopeTypes.ProtectedApi,
                },
            };
        }
    }
}
