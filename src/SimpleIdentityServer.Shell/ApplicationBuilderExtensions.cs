﻿using Microsoft.AspNetCore.Builder;
using SimpleIdentityServer.Shell.Controllers;
using System;

namespace SimpleIdentityServer.Shell
{
    using Microsoft.Extensions.FileProviders;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseShellStaticFiles(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var assembly = typeof(HomeController).Assembly;
            var embeddedFileProvider = new EmbeddedFileProvider(assembly, "SimpleIdentityServer.Shell.wwwroot");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = embeddedFileProvider
            });
            return app;
        }
    }
}
