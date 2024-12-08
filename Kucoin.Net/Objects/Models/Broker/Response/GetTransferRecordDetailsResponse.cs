namespace Kucoin.Net.Objects.Models.Broker.Response
{
    public class GetTransferRecordDetailsResponse
    {
        public string OrderId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public long FromUid { get; set; }
        public string FromAccountType { get; set; }
        public string FromAccountTag { get; set; }
        public long ToUid { get; set; }
        public string ToAccountType { get; set; }
        public string ToAccountTag { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public long CreatedAt { get; set; }
    }

}
