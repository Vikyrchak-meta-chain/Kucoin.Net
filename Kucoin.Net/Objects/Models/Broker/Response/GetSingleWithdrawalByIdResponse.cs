namespace Kucoin.Net.Objects.Models.Broker.Response
{
    public class GetSingleWithdrawalByIdResponse
    {
        public string Id { get; set; }
        public long Uid { get; set; }
        public string Currency { get; set; }
        public string Chain { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string Memo { get; set; }
        public bool IsInner { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string WalletTxId { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
        public string Remark { get; set; }
    }

}
