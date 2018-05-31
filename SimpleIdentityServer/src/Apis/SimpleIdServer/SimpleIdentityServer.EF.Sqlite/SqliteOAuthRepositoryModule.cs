﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Module;
using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.EF.Sqlite
{
    public class SqliteOAuthRepositoryModule : IModule
    {
        private const string _oauthConnectionString = "OAuthConnectionString";

        public void Configure(IApplicationBuilder applicationBuilder)
        {
        }

        public void Configure(IRouteBuilder routeBuilder)
        {
        }

        public void ConfigureServices(IServiceCollection services, IMvcBuilder mvcBuilder = null, IHostingEnvironment env = null,  IDictionary<string, string> options = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (!options.ContainsKey(_oauthConnectionString))
            {
                throw new ModuleException("configuration", $"The {_oauthConnectionString} configuration is missing");
            }

            services.AddOAuthSqliteEF(options["OAuthConnectionString"]);
        }

        public IEnumerable<string> GetOptionKeys()
        {
            return new[]
            {
                _oauthConnectionString
            };
        }
    }
}