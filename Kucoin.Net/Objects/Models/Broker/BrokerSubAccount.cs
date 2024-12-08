using System;
using System.Collections.Generic;
using System.Text;

namespace Kucoin.Net.Objects.Models.Broker
{
    // Model for each sub-account
    public class BrokerSubAccount
    {
        public string AccountName { get; set; } // Sub-account name
        public long Uid { get; set; }           // Sub-account UID
        public long CreatedAt { get; set; }     // Creation time in Unix timestamp (milliseconds)
        public int Level { get; set; }          // Sub-account VIP level
    }
}
