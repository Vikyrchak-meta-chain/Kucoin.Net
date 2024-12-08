namespace Kucoin.Net.Objects.Models.Broker.Request
{
    public class DownloadBrokerRebateOrderRequest
    {
        public string Begin { get; set; }
        public string End { get; set; }
        public int TradeType { get; set; }
    }

}
