using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Kucoin.Net.Objects.Internal;
using KucoinBroker.Net.Objects;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Kucoin.Net;
using Kucoin.Net.Objects.Models.Broker.Response;
using Kucoin.Net.Objects.Models.Broker.Request;
using Kucoin.Net.Clients;
using Kucoin.Net.Objects.Options;

namespace KucoinBroker.Net
{
    /// <inheritdoc cref="IKucoinRestBrokerClientApi" />
    public class KucoinRestBrokerClientApi : RestApiClient, IKucoinRestBrokerClientApi
    {
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly RestApiClient _baseClient;
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Broker Api");
        private static string baseAddress = "https://api-broker.kucoin.com";

        public event Action<OrderId>? OnOrderPlaced;
        public event Action<OrderId>? OnOrderCanceled;

        public string ExchangeName => "Kucoin";

        private ILogger? logger;

        private static RestApiOptions restApiOptions = new RestApiOptions();

        public KucoinRestBrokerClientApi(ILogger logger, HttpClient? httpClient, KucoinRestClient baseClient, KucoinRestOptions options)
            : base(logger, httpClient, baseAddress, options, restApiOptions)
        {
            ParameterPositions[HttpMethod.Delete] = HttpMethodParameterPosition.InUri;
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KucoinBrokerAuthenticationProvider((KucoinBrokerApiCredentials)credentials);

        internal async Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<KucoinResult<T>>(BaseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result)
                return result.AsError<T>(result.Error!);

            if (result.Data.Code != 200000 && result.Data.Code != 200)
                return result.AsError<T>(new ServerError(result.Data.Code, result.Data.Message ?? "-"));

            return result.As(result.Data.Data);
        }

        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"api/v1/timestamp", KucoinExchange.RateLimiter.PublicRest, 2);
            var result = await SendAsync<long>(request, null, ct).ConfigureAwait(false);
            return result.As(result ? new DateTime(1970, 1, 1).AddMilliseconds(result.Data) : default);
        }

        public async Task<WebCallResult<SubAccountCreateResponse>> CreateSubAccountAsync(
            SubAccountCreateRequest request,
            CancellationToken ct = default)
        {
            // Create request definition for creating a sub-account
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Post, "api/v1/broker/nd/account", KucoinExchange.RateLimiter.PublicRest, 3);

            // Set request parameters
            var parameters = new ParameterCollection
    {
        { "accountName", request.AccountName }
    };

            // Send request asynchronously and get result
            var result = await SendAsync<SubAccountCreateResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            // Return result
            return result;
        }
        public async Task<WebCallResult<BrokerTransferResponse>> TransferFundsAsync(
            BrokerTransferRequest request,
            CancellationToken ct = default)
        {
            // Create request definition for fund transfer
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Post, "api/v1/broker/nd/transfer", KucoinExchange.RateLimiter.PublicRest, 1);

            // Set request parameters
            var parameters = new ParameterCollection
        {
            { "clientOid", request.ClientOid },
            { "currency", request.Currency },
            { "amount", request.Amount },
            { "direction", request.Direction },
            { "accountType", request.AccountType },
            { "specialUid", request.SpecialUid },
            { "specialAccountType", request.SpecialAccountType }
        };

            // Send request asynchronously and get result
            var result = await SendAsync<BrokerTransferResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            // Return result
            return result;
        }
        public async Task<WebCallResult<BrokerInfoResponse>> GetBrokerInfoAsync(CancellationToken ct = default)
        {
            // Create request definition for fetching broker info
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/broker/nd/info", KucoinExchange.RateLimiter.PublicRest, 2);

            // Send request asynchronously and get result
            var result = await SendAsync<BrokerInfoResponse>(requestDefinition, null, ct).ConfigureAwait(false);

            // Return result
            return result;
        }

        public async Task<WebCallResult<BrokerSubAccountResponse>> GetBrokerSubAccountsAsync(int? uid = null, int currentPage = 1, int pageSize = 20, CancellationToken ct = default)
        {
            // Create request parameters
            var parameters = new ParameterCollection
    {
        { "uid", uid },
        { "currentPage", currentPage },
        { "pageSize", pageSize }
    };

            // Create request definition
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/broker/nd/account", KucoinExchange.RateLimiter.PublicRest, 2);

            // Send request asynchronously and get result
            var result = await SendAsync<BrokerSubAccountResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            // Return result
            return result;
        }

        public async Task<WebCallResult<CreateBrokerSubAccountApiKeyResponse>> CreateBrokerSubAccountApiKeyAsync(CreateBrokerSubAccountApiKeyRequest request, CancellationToken ct = default)
        {
            // Prepare the parameters for the request
            var parameters = new ParameterCollection
    {
        { "uid", request.Uid },
        { "passphrase", request.Passphrase },
        { "ipWhitelist", string.Join(",", request.IpWhitelist) }, // Join IPs with commas
        { "permissions", string.Join(",", request.Permissions) }, // Join permissions with commas
        { "label", request.Label }
    };

            // Define the request for creating the API key for the sub-account
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Post, "api/v1/broker/nd/account/apikey", KucoinExchange.RateLimiter.PublicRest, 3);

            // Send the request asynchronously
            var result = await SendAsync<CreateBrokerSubAccountApiKeyResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            // Return the result
            return result;
        }

        public async Task<WebCallResult<List<GetBrokerSubAccountApiKeyResponse>>> GetBrokerSubAccountApiKeyAsync(string uid, string? apiKey = null, CancellationToken ct = default)
        {
            // Create a dictionary of parameters
            var parameters = new ParameterCollection
    {
        { "uid", uid }
    };

            if (!string.IsNullOrEmpty(apiKey))
            {
                parameters.Add("apiKey", apiKey);
            }

            // Define the request for getting the API key information for the sub-account
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/broker/nd/account/apikey", KucoinExchange.RateLimiter.PublicRest, 2);

            // Send the request asynchronously with the parameters
            var result = await SendAsync<List<GetBrokerSubAccountApiKeyResponse>>(requestDefinition, parameters, ct).ConfigureAwait(false);

            return result;
        }

        public async Task<WebCallResult<ModifyBrokerSubAccountApiKeyResponse>> ModifyBrokerSubAccountApiKeyAsync(
    ModifyBrokerSubAccountApiKeyRequest request,
    CancellationToken ct = default)
        {
            // Create a dictionary of parameters based on the request object
            var parameters = new ParameterCollection
    {
        { "uid", request.Uid },
        { "apiKey", request.ApiKey }
    };

            if (request.IpWhitelist != null && request.IpWhitelist.Any())
            {
                parameters.Add("ipWhitelist", string.Join(",", request.IpWhitelist));
            }

            if (request.Permissions != null && request.Permissions.Any())
            {
                parameters.Add("permissions", string.Join(",", request.Permissions));
            }

            if (!string.IsNullOrEmpty(request.Label))
            {
                parameters.Add("label", request.Label);
            }

            // Define the request for modifying the API key information for the sub-account
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Post, "api/v1/broker/nd/account/update-apikey", KucoinExchange.RateLimiter.PublicRest, 3);

            // Send the request asynchronously with the parameters
            var result = await SendAsync<ModifyBrokerSubAccountApiKeyResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            return result;
        }

        public async Task<WebCallResult<DeleteBrokerSubAccountApiKeyResponse>> DeleteBrokerSubAccountApiKeyAsync(
    DeleteBrokerSubAccountApiKeyRequest request,
    CancellationToken ct = default)
        {
            // Define the parameters for the DELETE request
            var parameters = new ParameterCollection
    {
        { "uid", request.Uid },
        { "apiKey", request.ApiKey }
    };

            // Define the request for deleting the sub-account API key
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Delete, "api/v1/broker/nd/account/apikey", KucoinExchange.RateLimiter.PublicRest, 3);

            // Send the request asynchronously with the parameters
            var result = await SendAsync<DeleteBrokerSubAccountApiKeyResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            return result;
        }

        public async Task<WebCallResult<GetBrokerSubAccountsDepositRecordsResponse>> GetBrokerSubAccountsDepositRecordsAsync(
    GetBrokerSubAccountsDepositRecordsRequest request,
    CancellationToken ct = default)
        {
            // Define the parameters for the GET request using the request model
            var parameters = new ParameterCollection
    {
        { "currency", request.Currency },
        { "status", request.Status },
        { "hash", request.Hash },
        { "startTimestamp", request.StartTimestamp },
        { "endTimestamp", request.EndTimestamp },
        { "limit", request.Limit }
    };

            // Define the request for getting the deposit records
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/asset/ndbroker/deposit/list", KucoinExchange.RateLimiter.PublicRest, 10);

            // Send the request asynchronously with the parameters
            var result = await SendAsync<GetBrokerSubAccountsDepositRecordsResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            return result;
        }

        public async Task<WebCallResult<GetTransferRecordDetailsResponse>> GetTransferRecordDetailsAsync(
    GetTransferRecordDetailsRequest request,
    CancellationToken ct = default)
        {
            // Create the parameters for the GET request using the orderId from the request
            var parameters = new ParameterCollection
    {
        { "orderId", request.OrderId }
    };

            // Define the request for getting the transfer record details
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/broker/nd/transfer/detail", KucoinExchange.RateLimiter.PublicRest, 1);

            // Send the request asynchronously with the provided parameters
            var result = await SendAsync<GetTransferRecordDetailsResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            return result;
        }

        public async Task<WebCallResult<GetSingleDepositByHashResponse>> GetSingleDepositByHashAsync(
    GetSingleDepositByHashRequest request,
    CancellationToken ct = default)
        {
            // Create the parameters for the GET request using the hash and currency from the request
            var parameters = new ParameterCollection
    {
        { "hash", request.Hash },
        { "currency", request.Currency }
    };

            // Define the request for getting the deposit record by hash
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/broker/nd/deposit/detail", KucoinExchange.RateLimiter.PublicRest, 1);

            // Send the request asynchronously with the provided parameters
            var result = await SendAsync<GetSingleDepositByHashResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            return result;
        }

        public async Task<WebCallResult<GetSingleWithdrawalByIdResponse>> GetSingleWithdrawalByIdAsync(
    GetSingleWithdrawalByIdRequest request,
    CancellationToken ct = default)
        {
            // Create the parameters for the GET request using the withdrawalId from the request
            var parameters = new ParameterCollection
    {
        { "withdrawalId", request.WithdrawalId }
    };

            // Define the request for getting the withdrawal record by withdrawal ID
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/broker/nd/withdraw/detail", KucoinExchange.RateLimiter.PublicRest, 1);

            // Send the request asynchronously with the provided parameters
            var result = await SendAsync<GetSingleWithdrawalByIdResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            return result;
        }

        public async Task<WebCallResult<DownloadBrokerRebateOrderResponse>> DownloadBrokerRebateOrderAsync(
    DownloadBrokerRebateOrderRequest request,
    CancellationToken ct = default)
        {
            // Create the parameters for the GET request using the provided request object
            var parameters = new ParameterCollection
    {
        { "begin", request.Begin },
        { "end", request.End },
        { "tradeType", request.TradeType.ToString() }
    };

            // Define the request for downloading the rebate order file
            var requestDefinition = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/broker/nd/rebase/download", KucoinExchange.RateLimiter.PublicRest, 3);

            // Send the request asynchronously with the provided parameters
            var result = await SendAsync<DownloadBrokerRebateOrderResponse>(requestDefinition, parameters, ct).ConfigureAwait(false);

            return result;
        }


        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, (ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp), (ApiOptions.TimestampRecalculationInterval ?? ClientOptions.TimestampRecalculationInterval), _timeSyncState);

        public override TimeSpan? GetTimeOffset()
           => _timeSyncState.TimeOffset;


        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
           => KucoinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);
    }
}
