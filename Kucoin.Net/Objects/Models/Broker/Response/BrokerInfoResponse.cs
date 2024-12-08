namespace Kucoin.Net.Objects.Models.Broker.Response
{
    // Model for response when fetching Broker information
    public class BrokerInfoResponse
    {
        public int AccountSize { get; set; }   // Number of sub-accounts created
        public int? MaxAccountSize { get; set; } // The maximum number of sub-accounts allowed (null means no limit)
        public int Level { get; set; }          // Broker level
    }

}
