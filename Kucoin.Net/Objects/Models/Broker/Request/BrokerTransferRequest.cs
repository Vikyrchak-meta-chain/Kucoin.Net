namespace Kucoin.Net.Objects.Models.Broker.Request
{
    public class BrokerTransferRequest
    {
        public string ClientOid { get; set; } = string.Empty;  // Unique Client Order ID
        public string Currency { get; set; } = string.Empty;   // Currency for the transfer
        public decimal Amount { get; set; }                   // Transfer amount
        public string Direction { get; set; } = string.Empty;  // Direction: OUT or IN
        public string AccountType { get; set; } = string.Empty; // Account type: MAIN or TRADE
        public long SpecialUid { get; set; }                   // Subaccount UID
        public string SpecialAccountType { get; set; } = string.Empty; // Sub-account type: MAIN or TRADE
    }
}
