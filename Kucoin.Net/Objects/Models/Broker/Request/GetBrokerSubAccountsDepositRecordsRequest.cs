namespace Kucoin.Net.Objects.Models.Broker.Request
{
    public class GetBrokerSubAccountsDepositRecordsRequest
    {
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Hash { get; set; }
        public long? StartTimestamp { get; set; }
        public long? EndTimestamp { get; set; }
        public int Limit { get; set; } = 1000; // Default to 1000 records
    }

}
