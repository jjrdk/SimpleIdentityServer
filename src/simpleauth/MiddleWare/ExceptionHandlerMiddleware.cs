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

namespace SimpleAuth.MiddleWare
{
    using Exceptions;
    using Microsoft.AspNetCore.Http;
    using Shared;
    using SimpleAuth.Shared.Events.Logging;
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Logging;
    using SimpleAuth.Events;
    using SimpleAuth.Shared.Errors;

    internal class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEventPublisher _publisher;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            IEventPublisher publisher,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _publisher = publisher;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                context.Response.Clear();

                switch (exception)
                {
                    case SimpleAuthException serverException:
                        {
                            var state = exception is SimpleAuthExceptionWithState exceptionWithState
                                ? exceptionWithState.State
                                : string.Empty;
                            await _publisher.Publish(new SimpleAuthError(Id.Create(),
                                serverException.Code,
                                serverException.Message,
                                state,
                                DateTimeOffset.UtcNow)).ConfigureAwait(false);

                            _logger.LogError(serverException.StackTrace);
                            SetRedirection(context, exception, "400", serverException.Code);
                            break;
                        }
                    case AggregateException aggregateException:
                        {
                            foreach (var ex in aggregateException.InnerExceptions)
                            {
                                await PublishError(ex).ConfigureAwait(false);
                                _logger.LogError(ex.StackTrace);
                            }

                            SetRedirection(context, exception);
                            break;
                        }
                    default:
                        {
                            await PublishError(exception).ConfigureAwait(false);
                            _logger.LogError(exception.StackTrace);

                            SetRedirection(context, exception);
                            break;
                        }
                }

                if (!(context.Features[typeof(IEndpointFeature)] is IEndpointFeature endpointFeature))
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    return;
                }

                endpointFeature.Endpoint = null;
                await Invoke(context).ConfigureAwait(false);
            }
        }

        private async Task PublishError(Exception exception)
        {
            await _publisher
                .Publish(
                    new SimpleAuthError(
                        Id.Create(),
                        exception.GetType().Name,
                        exception.Message,
                        string.Empty,
                        DateTimeOffset.UtcNow))
                .ConfigureAwait(false);
        }

        private static void SetRedirection(HttpContext context, Exception exception, string code = null, string title = null)
        {
            context.Request.Path = "/error";
            context.Request.QueryString = new QueryString()
                .Add("code", code ?? "500")
                .Add("title", title ?? ErrorCodes.UnhandledExceptionCode)
                .Add("message", exception.Message);
        }
    }
}
