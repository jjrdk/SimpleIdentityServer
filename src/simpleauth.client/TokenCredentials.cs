﻿namespace SimpleAuth.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    /// <summary>
    /// Defines the token credentials.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{KeyValuePair}" />
    public class TokenCredentials : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly Dictionary<string, string> _form;

        private TokenCredentials(Dictionary<string, string> form, string authorizationValue = null, X509Certificate2 certificate = null)
        {
            _form = form;
            AuthorizationValue = authorizationValue;
            Certificate = certificate;
        }

        /// <summary>
        /// Gets the authorization value.
        /// </summary>
        /// <value>
        /// The authorization value.
        /// </value>
        public string AuthorizationValue { get; }

        /// <summary>
        /// Gets the certificate.
        /// </summary>
        /// <value>
        /// The certificate.
        /// </value>
        public X509Certificate2 Certificate { get; }

        /// <summary>
        /// Creates the credentials from the certificate.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="certificate">The certificate.</param>
        /// <returns></returns>
        public static TokenCredentials FromCertificate(string clientId, X509Certificate2 certificate)
        {
            var dict = new Dictionary<string, string>
            {
                { "client_id", clientId },
            };

            return new TokenCredentials(dict, null, certificate);
        }

        /// <summary>
        /// Creates the credentials from the client credentials.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns></returns>
        public static TokenCredentials FromClientCredentials(string clientId, string clientSecret)
        {
            var dict = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret }
            };

            return new TokenCredentials(dict);
        }

        /// <summary>
        /// Creates the credentials from the basic authentication.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns></returns>
        public static TokenCredentials FromBasicAuthentication(string clientId, string clientSecret)
        {
            var dict = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret }
            };

            return new TokenCredentials(dict, Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret)));
        }

        /// <summary>
        /// Creates the credentials from the client secret.
        /// </summary>
        /// <param name="clientAssertion">The client assertion.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public static TokenCredentials FromClientSecret(string clientAssertion, string clientId)
        {
            var dict = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_assertion", clientAssertion },
                { "client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer" }
            };

            return new TokenCredentials(dict);
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _form.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}