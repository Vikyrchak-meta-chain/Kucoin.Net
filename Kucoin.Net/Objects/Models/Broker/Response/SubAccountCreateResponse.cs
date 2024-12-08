namespace Kucoin.Net.Objects.Models.Broker.Response
{
    // Model for response when creating a sub-account
    public class SubAccountCreateResponse
    {
        public string AccountName { get; set; } = string.Empty;
        public long Uid { get; set; }
        public long CreatedAt { get; set; } // Unix timestamp in milliseconds
        public int Level { get; set; }
    }
}
