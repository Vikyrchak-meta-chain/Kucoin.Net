using System;
using System.Collections.Generic;
using System.Text;

namespace Kucoin.Net.Objects.Models.Broker.Request
{
    public class DeleteBrokerSubAccountApiKeyRequest
    {
        public string Uid { get; set; }
        public string ApiKey { get; set; }
    }
}
