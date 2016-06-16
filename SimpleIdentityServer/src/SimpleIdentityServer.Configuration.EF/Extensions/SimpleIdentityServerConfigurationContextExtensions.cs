﻿#region copyright
// Copyright 2015 Habart Thierry
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
#endregion

using SimpleIdentityServer.Configuration.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.Configuration.EF.Extensions
{
    public static class SimpleIdentityServerConfigurationContextExtensions
    {
        #region Public static methods

        public static void EnsureSeedData(this SimpleIdentityServerConfigurationContext context)
        {
            if (context.AllMigrationsApplied())
            {
                InsertAuthenticationProviders(context);
                context.SaveChanges();
            }
        }

        #endregion

        #region Private static methods

        private static void InsertAuthenticationProviders(SimpleIdentityServerConfigurationContext context)
        {
            if (!context.AuthenticationProviders.Any())
            {
                context.AuthenticationProviders.AddRange(new[]
                {
                    new AuthenticationProvider
                    {
                        IsEnabled = true,
                        Name = "Facebook",
                        Options = new List<Option>
                        {
                            new Option
                            {
                                Id = Guid.NewGuid().ToString(),
                                Key = "ClientId",
                                Value = "569242033233529"
                            },
                            new Option
                            {
                                Id = Guid.NewGuid().ToString(),
                                Key = "ClientSecret",
                                Value = "12e0f33817634c0a650c0121d05e53eb"
                            },
                            new Option
                            {
                                Id = Guid.NewGuid().ToString(),
                                Key = "Scope",
                                Value = "email"
                            }
                        }
                    },
                    new AuthenticationProvider
                    {
                        IsEnabled = true,
                        Name = "Microsoft",
                        Options = new List<Option>
                        {
                            new Option
                            {
                                Id = Guid.NewGuid().ToString(),
                                Key = "ClientId",
                                Value = "0000000048185530"
                            },
                            new Option
                            {
                                Id = Guid.NewGuid().ToString(),
                                Key = "ClientSecret",
                                Value = "KN12jxYIAYOr0bCLXFBcXhBrTlZyLNAZ"
                            },
                            new Option
                            {
                                Id = Guid.NewGuid().ToString(),
                                Key = "Scope",
                                Value = "openid"
                            }
                        }
                    }
                });
            }
        }

        #endregion
    }
}
