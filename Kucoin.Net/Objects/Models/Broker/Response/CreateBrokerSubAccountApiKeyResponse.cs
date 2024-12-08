using System.Collections.Generic;

namespace Kucoin.Net.Objects.Models.Broker.Response
{
    public class CreateBrokerSubAccountApiKeyResponse
    {
        public string Uid { get; set; }
        public string Label { get; set; }
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public List<string> Permissions { get; set; }
        public List<string> IpWhitelist { get; set; }
        public long CreatedAt { get; set; }
    }

}
