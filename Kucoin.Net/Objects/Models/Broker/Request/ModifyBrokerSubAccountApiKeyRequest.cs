using System.Collections.Generic;

namespace Kucoin.Net.Objects.Models.Broker.Request
{
    public class ModifyBrokerSubAccountApiKeyRequest
    {
        public string Uid { get; set; }
        public string ApiKey { get; set; }
        public List<string>? IpWhitelist { get; set; }
        public List<string>? Permissions { get; set; }
        public string? Label { get; set; }
    }
}
