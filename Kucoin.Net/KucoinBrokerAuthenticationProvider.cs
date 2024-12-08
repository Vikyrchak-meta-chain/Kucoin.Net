using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System;
using KucoinBroker.Net.Objects;
using CryptoExchange.Net;
using System.Linq;

namespace KucoinBroker.Net
{
    internal class KucoinBrokerAuthenticationProvider : AuthenticationProvider<KucoinBrokerApiCredentials>
    {
        private readonly static ConcurrentDictionary<string, string> _phraseCache = new();

        public KucoinBrokerAuthenticationProvider(KucoinBrokerApiCredentials credentials) : base(credentials)
        {
            if (credentials.CredentialType != ApiCredentialsType.Hmac)
                throw new Exception("Only Hmac authentication is supported");
        }
        public override void AuthenticateRequest(
            RestApiClient apiClient,
            Uri uri,
            HttpMethod method,
            ref IDictionary<string, object>? uriParameters,
            ref IDictionary<string, object>? bodyParameters,
            ref Dictionary<string, string>? headers,
            bool auth,
            ArrayParametersSerialization arraySerialization,
            HttpMethodParameterPosition parameterPosition,
            RequestBodyFormat requestBodyFormat)
        {
            if (!auth)
                return;
            //4.1 Get your KC-BROKER-NAME, KC-API-PARTNER and KC-API-PARTNER-SECRETKEY via 
            var brokerName = ((KucoinBrokerRestApiOptions)apiClient.ApiOptions).BrokerName;
            var brokerKey = ((KucoinBrokerRestApiOptions)apiClient.ApiOptions).BrokerKey;
            var ApiPartner = ((KucoinBrokerRestApiOptions)apiClient.ApiOptions).ApiPartner;
            var ApiPartnerSecretKey = ((KucoinBrokerRestApiOptions)apiClient.ApiOptions).PartnerSecretKey;

            if (string.IsNullOrEmpty(brokerName) && string.IsNullOrEmpty(brokerKey))
            {
                brokerName = apiClient is KucoinRestBrokerClientApi ? "Easytradingfutures" : "Easytrading";
                brokerKey = apiClient is KucoinRestBrokerClientApi ? "9e08c05f-454d-4580-82af-2f4c7027fd00" : "f8ae62cb-2b3d-420c-8c98-e1c17dd4e30a";
            }

            if (uriParameters != null)
                uri = uri.SetParameters(uriParameters, arraySerialization);

            headers ??= new Dictionary<string, string>();
            headers.Add("KC-API-KEY", _credentials.Key);
            headers.Add("KC-API-TIMESTAMP", GetMillisecondTimestamp(apiClient).ToString());
            var phrase = _credentials.PassPhrase;
            if (!_phraseCache.TryGetValue(phrase, out var phraseSign))
            {
                phraseSign = SignHMACSHA256(phrase, SignOutputType.Base64);
                _phraseCache.TryAdd(phrase, phraseSign);
            }
            //4.1 Get your KC-BROKER-NAME, KC-API-PARTNER and KC-API-PARTNER-SECRETKEY via 

            headers.Add("KC-BROKER-NAME", brokerName);
            headers.Add("KC-API-PASSPHRASE", phraseSign);
            headers.Add("KC-API-KEY-VERSION", "3");

            string jsonContent = string.Empty;
            if (parameterPosition == HttpMethodParameterPosition.InBody)
            {
                if (bodyParameters?.Any() == true)
                {
                    jsonContent = bodyParameters.Count == 1 && bodyParameters.First().Key == "<BODY>"
                        ? JsonConvert.SerializeObject(bodyParameters.First().Value)
                        : JsonConvert.SerializeObject(bodyParameters);
                }
                else
                {
                    jsonContent = "{}";
                }
            }

            var signData = headers["KC-API-TIMESTAMP"] + method + Uri.UnescapeDataString(uri.PathAndQuery) + jsonContent;
            headers.Add("KC-API-SIGN", SignHMACSHA256(signData, SignOutputType.Base64));

            // Partner info
            headers.Add("KC-API-PARTNER", brokerName!);
            headers.Add("KC-API-PARTNER-SECRETKEY", ApiPartnerSecretKey!);
            var partnerSignData = headers["KC-API-TIMESTAMP"] + brokerName + _credentials.Key;
            using HMACSHA256 hMACSHA = new HMACSHA256(Encoding.UTF8.GetBytes(brokerKey));
            byte[] buff = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(partnerSignData));
            headers.Add("KC-API-PARTNER-SIGN", BytesToBase64String(buff));
        }
    }
}