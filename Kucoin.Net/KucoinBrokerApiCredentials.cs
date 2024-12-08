using System;
using System.IO;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Converters.MessageParsing;

namespace KucoinBroker.Net.Objects
{
    /// <summary>
    /// Credentials for the Kucoin Broker API
    /// </summary>
    public class KucoinBrokerApiCredentials : ApiCredentials
    {
        /// <summary>
        /// The pass phrase
        /// </summary>
        public string PassPhrase { get; }

        /// <summary>
        /// The broker key
        /// </summary>
        public string BrokerKey { get; }

        /// <summary>
        /// Credentials type (HMAC, RSA XML, or RSA PEM)
        /// </summary>
        public ApiCredentialsType CredentialsType { get; }

        /// <summary>
        /// Creates new API credentials. Keep this information safe.
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="apiSecret">The API secret</param>
        /// <param name="apiPassPhrase">The API passPhrase</param>
        /// <param name="apiBrokerKey">The API broker key</param>
        /// <param name="credentialsType">The type of credentials being used</param>
        /// <exception cref="ArgumentException">Thrown when any of the arguments are invalid or empty.</exception>
        public KucoinBrokerApiCredentials(string apiKey, string apiSecret, string apiPassPhrase, string apiBrokerKey, ApiCredentialsType credentialsType = ApiCredentialsType.Hmac)
            : base(apiKey, apiSecret)
        {
            if (string.IsNullOrWhiteSpace(apiPassPhrase))
                throw new ArgumentException("Passphrase cannot be null or empty.", nameof(apiPassPhrase));

            if (string.IsNullOrWhiteSpace(apiBrokerKey))
                throw new ArgumentException("BrokerKey cannot be null or empty.", nameof(apiBrokerKey));

            PassPhrase = apiPassPhrase;
            BrokerKey = apiBrokerKey;
            CredentialsType = credentialsType;
        }

        /// <summary>
        /// Create Api credentials providing a stream containing JSON data. The JSON data should include four values: apiKey, apiSecret, apiPassPhrase, and apiBrokerKey.
        /// </summary>
        /// <param name="jsonData">The JSON data as a string</param>
        /// <param name="identifierKey">A key to identify the credentials for the API. Defaults to 'apiKey'.</param>
        /// <param name="identifierSecret">A key to identify the credentials for the API. Defaults to 'apiSecret'.</param>
        /// <param name="identifierPassPhrase">A key to identify the credentials for the API. Defaults to 'apiPassPhrase'.</param>
        /// <param name="identifierBrokerKey">A key to identify the credentials for the API. Defaults to 'apiBrokerKey'.</param>
        /// <param name="credentialsType">The type of credentials being used</param>
        /// <exception cref="ArgumentException">Thrown when the input stream does not contain valid JSON or required keys.</exception>
        public KucoinBrokerApiCredentials(string jsonData, string identifierKey = "apiKey", string identifierSecret = "apiSecret", string identifierPassPhrase = "apiPassPhrase", string identifierBrokerKey = "apiBrokerKey", ApiCredentialsType credentialsType = ApiCredentialsType.Hmac)
            : base(identifierKey ?? "", identifierSecret ?? "", credentialsType)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                throw new ArgumentNullException(nameof(jsonData), "JSON data cannot be null or empty.");

            try
            {
                using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonData)))
                {
                    var accessor = new JsonNetStreamMessageAccessor();
                    if (!accessor.Read(stream, false).Result)
                        throw new ArgumentException("Input stream does not contain valid JSON data.");

                    PassPhrase = accessor.GetValue<string>(MessagePath.Get().Property(identifierPassPhrase)) ??
                                 throw new ArgumentException("apiPassPhrase value not found in JSON credential file.");

                    BrokerKey = accessor.GetValue<string>(MessagePath.Get().Property(identifierBrokerKey)) ??
                                throw new ArgumentException("apiBrokerKey value not found in JSON credential file.");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error occurred while parsing the JSON data.", ex);
            }
        }

        /// <inheritdoc />
        public override ApiCredentials Copy()
        {
            return new KucoinBrokerApiCredentials(Key, Secret, PassPhrase, BrokerKey, CredentialsType);
        }
    }
}
