using CryptoExchange.Net.Objects;

namespace KucoinBroker.Net
{
    public class KucoinBrokerEnvironment : TradeEnvironment
    {
        private string _name;

        public KucoinBrokerEnvironment(string name) : base(name)
        {
            this._name = name;
        }

        /// <summary>
        /// Live environment
        /// </summary>
        public static KucoinBrokerEnvironment Live { get; } = new KucoinBrokerEnvironment(TradeEnvironmentNames.Live);
    }
}