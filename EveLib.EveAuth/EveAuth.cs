// ***********************************************************************
// Assembly         : EveLib.EveAuth
// Author           : Lars Kristian
// Created          : 12-10-2014
//
// Last Modified By : Lars Kristian
// Last Modified On : 12-17-2014
// ***********************************************************************
// <copyright file="EveAuth.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using eZet.EveLib.Core.Util;
using Newtonsoft.Json;
using System.Net.Http;

namespace eZet.EveLib.EveAuthModule {
    /// <summary>
    /// Enum CrestScope
    /// </summary>
    public enum CrestScope {
        /// <summary>
        ///     The publicData scope
        /// </summary>
        PublicData,

        /// <summary>
        /// The character statistics read
        /// </summary>
        CharacterStatisticsRead,

        /// <summary>
        /// The character contacts read
        /// </summary>
        CharacterContactsRead,

        /// <summary>
        /// The character fittings read
        /// </summary>
        CharacterFittingsRead,

        /// <summary>
        /// The character fittings write
        /// </summary>
        CharacterFittingsWrite,
    }

    /// <summary>
    ///     Class EveAuth. A helper class for Eve Online SSO authentication
    /// </summary>
    public class EveAuth : IEveAuth {
        private readonly TraceSource _trace = new TraceSource("EveLib", SourceLevels.All);

        /// <summary>
        ///     Initializes a new instance of the <see cref="EveAuth" /> class.
        /// </summary>
        public EveAuth() {
            Host = "login.eveonline.com";
            Protocol = "https://";
        }

        public string Protocol { get; set; }

        /// <summary>
        ///     Gets or sets the base URI.
        /// </summary>
        /// <value>The base URI.</value>
        public string Host { get; set; }

        /// <summary>
        ///     Authenticates the specified encoded key.
        /// </summary>
        /// <param name="encodedKey">The encoded key.</param>
        /// <param name="authCode">The authentication code.</param>
        /// <returns>Task&lt;AuthResponse&gt;.</returns>
        public async Task<AuthResponse> AuthenticateAsync(string encodedKey, string authCode)
        {
            var uri = new Uri(String.Format("{0}{1}/oauth/token"));
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", encodedKey));

            var content = new StringContent(String.Format("grant_type=authorization_code&code={0}", authCode));
            return await HandleRequestAsync<AuthResponse>(await client.PostAsync(uri, content).ConfigureAwait(false));
        }

        /// <summary>
        ///     Refreshes the specified encoded key.
        /// </summary>
        /// <param name="encodedKey">The encoded key.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>Task&lt;AuthResponse&gt;.</returns>
        public async Task<AuthResponse> RefreshAsync(string encodedKey, string refreshToken)
        {
            var uri = new Uri(String.Format("{0}{1}/oauth/token"));
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", encodedKey));

            var content = new StringContent(String.Format("grant_type=refresh_token&refresh_token={0}", refreshToken));
            return await HandleRequestAsync<AuthResponse>(await client.PostAsync(uri, content).ConfigureAwait(false));
        }

        /// <summary>
        ///     Verifies the access token
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns>Task&lt;VerifyResponse&gt;.</returns>
        public async Task<VerifyResponse> VerifyAsync(string accessToken)
        {
            var uri = new Uri(String.Format("{0}{1}/oauth/verify"));
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", accessToken));

            return await HandleRequestAsync<VerifyResponse>(await client.GetAsync(uri).ConfigureAwait(false));
        }

        private static string resolveScope(params CrestScope[] crestScopes) {
            string scope = "";
            foreach (var crestScope in crestScopes)
            switch (crestScope) {
                case CrestScope.PublicData:
                    scope += "publicData ";
                    break;
                case CrestScope.CharacterFittingsRead:
                    scope += "characterFittingsRead ";
                    break;
                case CrestScope.CharacterContactsRead:
                    scope += "characterContactsRead ";
                    break;
                case CrestScope.CharacterStatisticsRead:
                    scope += "characterStatisticsRead ";
                    break;
                case CrestScope.CharacterFittingsWrite:
                    scope += "characterFittingsWrite ";
                    break;
            }
            return scope;
        }


        /// <summary>
        ///     Creates an authentication link.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="crestScope">The crest scope.</param>
        /// <param name="state"></param>
        /// <returns>System.String.</returns>
        public string CreateAuthLink(string clientId, string redirectUri, string state, params CrestScope[] crestScope) {
            string url =
                Protocol + Host + "/oauth/authorize/?response_type=code&redirect_uri=" + redirectUri + "&client_id=" + clientId +
                "&scope=" + resolveScope(crestScope) + "&state=" + state;
            return url;
        }


        /// <summary>
        /// Creates the authentication link.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="state">The state.</param>
        /// <param name="scopes">The scopes.</param>
        /// <returns>System.String.</returns>
        public string CreateAuthLink(string clientId, string redirectUri, string state, string scopes) {
            string url =
                Protocol + Host + "/oauth/authorize/?response_type=code&redirect_uri=" + redirectUri + "&client_id=" + clientId +
                "&scope=" + scopes + "&state=" + state;
            return url;
        }

        /// <summary>
        ///     Encodes the specified client identifier and secret.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns>System.String.</returns>
        public static string Encode(string clientId, string clientSecret) {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(clientId + ":" + clientSecret);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Handles an HTTP response from the client.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to.</typeparam>
        /// <param name="response">The HTTP response to handle.</param>
        /// <returns>The deserialized object.</returns>
        private async Task<T> HandleRequestAsync<T>(HttpResponseMessage response)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var webException = new WebException(ex.Message);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {               
                    throw new EveAuthException(responseData, webException);
                }

                var authError = JsonConvert.DeserializeObject<AuthError>(responseData);
                _trace.TraceEvent(TraceEventType.Verbose, 0, "Message: {0}, Key: {1}",
                    "Exception Type: {2}, Ref ID: {3}", authError.Message, authError.Key, authError.ExceptionType,
                    authError.RefId);

                throw new EveAuthException(authError.Message, webException, authError.Key, authError.ExceptionType, authError.RefId);
            }

            var data = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            return data;
        }
    }
}