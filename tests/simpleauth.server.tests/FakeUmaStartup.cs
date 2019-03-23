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
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Reflection;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.Extensions.DependencyInjection;
    using SimpleAuth;
    using SimpleAuth.Controllers;
    using SimpleAuth.Extensions;
    using SimpleAuth.Repositories;
    using SimpleAuth.Server.Tests.MiddleWares;
    using SimpleAuth.Server.Tests.Stores;
    using SimpleAuth.Shared.Repositories;

    public class FakeUmaStartup : IStartup
    {
        public const string DefaultSchema = "OAuth2Introspection";

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // 1. Add the dependencies.
            RegisterServices(services);
            // 2. Add authorization policies.
            services.AddAuthentication(
                    opts =>
                    {
                        opts.DefaultAuthenticateScheme = DefaultSchema;
                        opts.DefaultChallengeScheme = DefaultSchema;
                    })
                .AddUmaCustomAuth(o => { });
            services.AddAuthorization(
                opts =>
                {
                    opts.AddAuthPolicies(DefaultSchema)
                        .AddPolicy(
                            "UmaProtection",
                            policy =>
                            {
                                policy.AddAuthenticationSchemes(DefaultSchema);
                                policy.RequireAssertion(p => true);
                            });
                });
            // 3. Add the dependencies needed to enable CORS
            services.AddCors(
                options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            // 4. Add authentication.
            services.AddAuthentication();
            // 5. Add the dependencies needed to run ASP.NET API.
            var mvc = services.AddMvc();
            var parts = mvc.PartManager.ApplicationParts;
            parts.Clear();
            parts.Add(new AssemblyPart(typeof(TokenController).GetTypeInfo().Assembly));
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Use(
                async (context, next) =>
                {
                    var claimsIdentity = new ClaimsIdentity(
                        new List<Claim> {new Claim("client_id", "resource_server")},
                        "fakests");
                    context.User = new ClaimsPrincipal(claimsIdentity);
                    await next.Invoke().ConfigureAwait(false);
                });

            // 3. Enable CORS
            app.UseCors("AllowAll").UseSimpleAuthExceptionHandler().UseMvcWithDefaultRoute();
            //(routes => { routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}"); });
        }

        private void RegisterServices(IServiceCollection services)
        {
            // 1. Add CORE.
            services.AddSimpleAuth(
                new SimpleAuthOptions
                {
                    Clients = sp => new InMemoryClientRepository(
                        new HttpClient(),
                        sp.GetService<IScopeStore>(),
                        OAuthStores.GetClients()),
                    Scopes = sp => new InMemoryScopeRepository(OAuthStores.GetScopes()),
                    ResourceSets = sp => new InMemoryResourceSetRepository(UmaStores.GetResources())
                });

            // 3. Enable logging.
            services.AddLogging();
            // 5. Register other classes.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
