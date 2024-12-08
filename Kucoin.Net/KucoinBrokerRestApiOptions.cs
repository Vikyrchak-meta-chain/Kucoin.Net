using KucoinBroker.Net.Objects;
using CryptoExchange.Net.Objects.Options;

namespace KucoinBroker.Net
{
    public class KucoinBrokerRestApiOptions : RestApiOptions<KucoinBrokerApiCredentials>
    {
        /// <summary>
        /// The broker reference name to use
        /// </summary>
        public string? BrokerName { get; set; }

        /// <summary>
        /// The private key of the broker
        /// </summary>
        public string? BrokerKey { get; set; }
        public string ApiPartner { get; internal set; }
        public string PartnerSecretKey { get; internal set; }

        internal KucoinBrokerRestApiOptions Copy()
        {
            var result = new KucoinBrokerRestApiOptions();
            base.Set(result);
            result.BrokerKey = BrokerKey;
            result.BrokerName = BrokerName;
            result.ApiPartner = ApiPartner;
            result.PartnerSecretKey = PartnerSecretKey;
            return result;
        }
    }
}