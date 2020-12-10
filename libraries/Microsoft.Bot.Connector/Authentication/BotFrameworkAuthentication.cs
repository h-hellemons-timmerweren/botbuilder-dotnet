﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;

namespace Microsoft.Bot.Connector.Authentication
{
    /// <summary>
    /// Represents a Cloud Environment used to authenticate Bot Framework Protocol network calls within this environment.
    /// </summary>
    public abstract class BotFrameworkAuthentication
    {
        /// <summary>
        /// Validate Bot Framework Protocol requests.
        /// </summary>
        /// <param name="activity">The inbound Activity.</param>
        /// <param name="authHeader">The http auth header.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Asynchronous Task with <see cref="AuthenticateRequestResult"/>.</returns>
        /// <exception cref="UnauthorizedAccessException">If the validation returns false.</exception>
        public abstract Task<AuthenticateRequestResult> AuthenticateRequestAsync(Activity activity, string authHeader, CancellationToken cancellationToken);

        /// <summary>
        /// Get the credentials object needed to make a proactive call.
        /// </summary>
        /// <param name="claimsIdentity">The inbound <see cref="Activity"/>'s <see cref="ClaimsIdentity"/>.</param>
        /// <param name="audience">The http auth header.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Asynchronous Task with <see cref="ProactiveCredentialsResult"/>.</returns>
        public abstract Task<ProactiveCredentialsResult> GetProactiveCredentialsAsync(ClaimsIdentity claimsIdentity, string audience, CancellationToken cancellationToken);

        /// <summary>
        /// Creates the appropriate <see cref="UserTokenClient" /> instance.
        /// </summary>
        /// <param name="claimsIdentity">The inbound <see cref="Activity"/>'s <see cref="ClaimsIdentity"/>.</param>
        /// <param name="httpClient">The <see cref="HttpClient" /> to use.</param>
        /// <param name="logger">The <see cref="ILogger" /> to use.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Asynchronous Task with <see cref="UserTokenClient" /> instance.</returns>
        public abstract Task<UserTokenClient> CreateAsync(ClaimsIdentity claimsIdentity, HttpClient httpClient, ILogger logger, CancellationToken cancellationToken);
    }
}
