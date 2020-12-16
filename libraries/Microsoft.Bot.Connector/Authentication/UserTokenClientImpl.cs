// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;

namespace Microsoft.Bot.Connector.Authentication
{
    internal class UserTokenClientImpl : UserTokenClient
    {
        private readonly string _appId;
        private readonly OAuthClient _client;
        private readonly ILogger _logger;

        public UserTokenClientImpl(
            string appId,
            ServiceClientCredentials credentials,
            string oauthEndpoint,
            HttpClient httpClient = null,
            ILogger logger = null)
        {
            _appId = appId;
            _client = new OAuthClient(credentials, httpClient, disposeHttpClient: httpClient == null) { BaseUri = new Uri(oauthEndpoint) };
            _logger = logger;
        }

        public override async Task<TokenResponse> GetUserTokenAsync(string userId, string connectionName, string channelId, string magicCode, CancellationToken cancellationToken)
        {
            return await _client.UserToken.GetTokenAsync(userId, connectionName, channelId, magicCode, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<SignInResource> GetSignInResourceAsync(string connectionName, Activity activity, string finalRedirect, CancellationToken cancellationToken)
        {
            var state = CreateTokenExchangeState(_appId, connectionName, activity);
            return await _client.GetSignInResourceAsync(state, null, null, finalRedirect, cancellationToken).ConfigureAwait(false);
        }

        public override async Task SignOutUserAsync(string userId, string channelId, string connectionName, CancellationToken cancellationToken)
        {
            await _client.UserToken.SignOutAsync(userId, channelId, connectionName, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<TokenStatus[]> GetTokenStatusAsync(string userId, string channelId, string includeFilter, CancellationToken cancellationToken)
        {
            var result = await _client.UserToken.GetTokenStatusAsync(userId, channelId, includeFilter, cancellationToken).ConfigureAwait(false);
            return result?.ToArray();
        }

        public override async Task<Dictionary<string, TokenResponse>> GetAadTokensAsync(string userId, string connectionName, string[] resourceUrls, string channelId, CancellationToken cancellationToken)
        {
            return (Dictionary<string, TokenResponse>)await _client.UserToken.GetAadTokensAsync(userId, connectionName, new AadResourceUrls() { ResourceUrls = resourceUrls?.ToList() }, channelId, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<TokenResponse> ExchangeTokenAsync(string userId, string connectionName, string channelId, TokenExchangeRequest exchangeRequest, CancellationToken cancellationToken)
        {
            var result = await _client.ExchangeAsyncAsync(userId, connectionName, channelId, exchangeRequest, cancellationToken).ConfigureAwait(false);

            if (result is ErrorResponse errorResponse)
            {
                throw new InvalidOperationException($"Unable to exchange token: ({errorResponse?.Error?.Code}) {errorResponse?.Error?.Message}");
            }
            else if (result is TokenResponse tokenResponse)
            {
                return tokenResponse;
            }
            else
            {
                throw new InvalidOperationException($"ExchangeAsyncAsync returned improper result: {result.GetType()}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _client.Dispose();
            base.Dispose(disposing);
        }
    }
}
