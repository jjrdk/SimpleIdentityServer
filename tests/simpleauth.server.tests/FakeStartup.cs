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

namespace SimpleAuth.Server.Tests
{
    using Client;
    using Controllers;
    using Extensions;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.Extensions.DependencyInjection;
    using MiddleWares;
    using SimpleAuth;
    using SimpleAuth.Services;
    using Stores;
    using System;
    using System.Net.Http;
    using Twilio;
    using Twilio.Actions;
    using Twilio.Controllers;
    using Twilio.Services;

    public class FakeStartup : IStartup
    {
        public const string ScimEndPoint = "http://localhost:5555/";
        public const string DefaultSchema = CookieAuthenticationDefaults.AuthenticationScheme;
        private readonly SimpleAuthOptions _options;
        private readonly SharedContext _context;

        public FakeStartup(SharedContext context)
        {
            _options = new SimpleAuthOptions
            {
                Configuration = new OpenIdServerConfiguration
                {
                    Clients = DefaultStores.Clients(context),
                    Consents = DefaultStores.Consents(),
                    Users = DefaultStores.Users()
                },
                Scim = new ScimOptions
                {
                    IsEnabled = true,
                    EndPoint = ScimEndPoint
                }
            };
            _context = context;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // 1. Add the dependencies needed to enable CORS
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            // 2. Configure server
            ConfigureIdServer(services);
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = DefaultSchema;
                opts.DefaultChallengeScheme = DefaultSchema;
            })
            .AddFakeCustomAuth(o => { })
            .AddFakeOAuth2Introspection(o =>
            {
                o.WellKnownConfigurationUrl = "http://localhost:5000/.well-known/openid-configuration";
                o.ClientId = "stateless_client";
                o.ClientSecret = "stateless_client";
                o.Client = _context.Client;
                // o.IdentityServerClientFactory = new IdentityServerClientFactory(_context.Oauth2IntrospectionHttpClientFactory.Object);
            })
            .AddFakeUserInfoIntrospection(o => { });
            services.AddAuthorization(opt =>
            {
                opt.AddAuthPolicies(DefaultSchema);
            });
            // 3. Configure MVC
            var mvc = services.AddMvc();
            var parts = mvc.PartManager.ApplicationParts;
            parts.Clear();
            parts.Add(new AssemblyPart(typeof(DiscoveryController).Assembly));
            parts.Add(new AssemblyPart(typeof(CodeController).Assembly));

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            //1 . Enable CORS.
            app.UseCors("AllowAll");
            // 4. Use simple identity server.
            app.UseSimpleAuth(o => { });
            //// 5. Client JWKS endpoint
            //app.Map("/jwks_client", a =>
            //{
            //    a.Run(async ctx =>
            //    {
            //        var jwks = new[]
            //        {
            //            _context.EncryptionKey,
            //            _context.SignatureKey
            //        };
            //        var repo = app.ApplicationServices.GetService<IJwksActions>();
            //        var result = await repo.GetJwks().ConfigureAwait(false);

            //        var json = JsonConvert.SerializeObject(result);
            //        var data = Encoding.UTF8.GetBytes(json);
            //        ctx.Response.ContentType = "application/json";
            //        await ctx.Response.Body.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
            //    });
            //});
            // 5. Use MVC.
            app.UseMvc();
        }

        private void ConfigureIdServer(IServiceCollection services)
        {
            _options.Configuration = new OpenIdServerConfiguration
            {
                Clients = DefaultStores.Clients(_context),
                Consents = DefaultStores.Consents(),
                //JsonWebKeys = DefaultStores.JsonWebKeys(_context),
                Profiles = DefaultStores.Profiles(),
                Users = DefaultStores.Users()
            };
            services.AddSingleton(new SmsAuthenticationOptions());
            services.AddSingleton(_context.TwilioClient.Object);
            services.AddTransient<ISmsAuthenticationOperation, SmsAuthenticationOperation>();
            services.AddTransient<IGenerateAndSendSmsCodeOperation, GenerateAndSendSmsCodeOperation>();
            services.AddTransient<IAuthenticateResourceOwnerService, SmsAuthenticateResourceOwnerService>();
            services.AddSimpleAuth(_options)
                .AddDefaultTokenStore()
                .AddLogging()
                .AddAccountFilter();
            services.AddSingleton(_context.ConfirmationCodeStore.Object);
            services.AddSingleton(sp => _context.Client);
            services.AddSingleton<IUsersClient>(sp =>
            {
                var baseUrl = _options.Scim.EndPoint;
                return new UsersClient(new Uri(baseUrl), sp.GetService<HttpClient>());
            });
        }
    }
}
