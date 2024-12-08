using System.Collections.Generic;

namespace Kucoin.Net.Objects.Models.Broker.Response
{
    // Model for response when querying Broker sub-accounts
    public class BrokerSubAccountResponse
    {
        public int TotalPage { get; set; }   // Total number of pages
        public int CurrentPage { get; set; } // Current page
        public int PageSize { get; set; }    // Number of items per page
        public int TotalNum { get; set; }    // Total number of sub-accounts
        public List<BrokerSubAccount> Items { get; set; } // List of sub-accounts
    }
}
