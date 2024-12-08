using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.SharedApis;
using static System.Net.WebRequestMethods;
using System.Collections.Generic;
using Kucoin.Net.Objects.Models.Broker.Response;
using Kucoin.Net.Objects.Models.Broker.Request;
using CryptoExchange.Net.Interfaces;

namespace KucoinBroker.Net
{
    public interface IKucoinRestBrokerClientApi: IRestApiClient
    {
        /// <summary>
        /// Get the current server time.
        /// </summary>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Create a Broker sub-account.
        /// Documentation: https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/create-broker-subaccount
        /// </summary>
        Task<WebCallResult<SubAccountCreateResponse>> CreateSubAccountAsync(
            SubAccountCreateRequest request,
            CancellationToken ct = default);

        /// <summary>
        /// Transfer funds between Broker account and Broker sub-accounts.
        /// Documentation: https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/broker-transfer
        /// </summary>
        Task<WebCallResult<BrokerTransferResponse>> TransferFundsAsync(
            BrokerTransferRequest request,
            CancellationToken ct = default);

        /// <summary>
        /// Get basic information about the current Broker.
        /// Documentation: https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/get-broker-info
        /// </summary>
        Task<WebCallResult<BrokerInfoResponse>> GetBrokerInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the sub-accounts created by the Broker.
        /// Documentation: https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/get-broker-subaccount
        /// </summary>
        Task<WebCallResult<BrokerSubAccountResponse>> GetBrokerSubAccountsAsync(int? uid = null, int currentPage = 1, int pageSize = 20, CancellationToken ct = default);

        /// <summary>
        /// Creates a Broker subaccount APIKEY.
        /// </summary>
        /// <param name="request">The request object containing all the parameters.</param>
        /// <returns>A WebCallResult with the API key data.</returns>
        /// <remarks>For more details, refer to the [documentation](https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/create-broker-subaccount-apikey).</remarks>
        Task<WebCallResult<CreateBrokerSubAccountApiKeyResponse>> CreateBrokerSubAccountApiKeyAsync(CreateBrokerSubAccountApiKeyRequest request, CancellationToken ct = default);

        /// <summary>
        /// Get the API keys for a Broker subaccount.
        /// </summary>
        /// <param name="uid">Subaccount UID.</param>
        /// <param name="apiKey">Optional API key to filter by.</param>
        /// <param name="ct">Cancellation token for asynchronous operations.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing the response.</returns>
        /// <seealso cref="https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/get-broker-subaccount-apikey"/>
        Task<WebCallResult<List<GetBrokerSubAccountApiKeyResponse>>> GetBrokerSubAccountApiKeyAsync(string uid, string? apiKey = null, CancellationToken ct = default);

        /// <summary>
        /// Modify the API key for a Broker subaccount.
        /// </summary>
        /// <param name="request">Request data for modifying the Broker subaccount API key.</param>
        /// <param name="ct">Cancellation token for asynchronous operations.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing the response.</returns>
        /// <seealso cref="https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/modify-broker-subaccount-apikey"/>
        Task<WebCallResult<ModifyBrokerSubAccountApiKeyResponse>> ModifyBrokerSubAccountApiKeyAsync(
            ModifyBrokerSubAccountApiKeyRequest request,
            CancellationToken ct = default);

        /// <summary>
        /// Deletes the Broker's sub-account API key.
        /// </summary>
        /// <param name="request">Request data for deleting the Broker subaccount API key.</param>
        /// <param name="ct">Cancellation token for asynchronous operations.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing the response.</returns>
        /// <seealso cref="https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/delete-broker-subaccount-apikey"/>
        Task<WebCallResult<DeleteBrokerSubAccountApiKeyResponse>> DeleteBrokerSubAccountApiKeyAsync(
            DeleteBrokerSubAccountApiKeyRequest request,
            CancellationToken ct = default);

        /// <summary>
        /// Gets the deposit records for the Broker's sub-accounts.
        /// </summary>
        /// <param name="request">Request object containing filter parameters for the deposit records.</param>
        /// <param name="ct">Cancellation token for asynchronous operations.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing the deposit records.</returns>
        /// <seealso cref="https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/get-broker-sub-accounts-deposit-records"/>
        Task<WebCallResult<GetBrokerSubAccountsDepositRecordsResponse>> GetBrokerSubAccountsDepositRecordsAsync(
            GetBrokerSubAccountsDepositRecordsRequest request,
            CancellationToken ct = default);
        /// <summary>
        /// Gets the details of a transfer record for the broker and its sub-accounts.
        /// </summary>
        /// <param name="request">The transfer record details request object containing the transfer order ID.</param>
        /// <param name="ct">Cancellation token for asynchronous operations.</param>
        /// <returns>A task representing the asynchronous operation, with the result containing the transfer record details.</returns>
        /// <seealso cref="https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/get-transfer-record-details"/>
        Task<WebCallResult<GetTransferRecordDetailsResponse>> GetTransferRecordDetailsAsync(
            GetTransferRecordDetailsRequest request,
            CancellationToken ct = default);

        /// <summary>
        /// Gets a single deposit record by hash for the broker's sub-accounts.
        /// </summary>
        /// <param name="request">The request object containing the hash and currency of the deposit record.</param>
        /// <param name="ct">Cancellation token for asynchronous operations.</param>
        /// <returns>A task representing the asynchronous operation, with the result containing the deposit record details.</returns>
        /// <seealso cref="https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/get-single-deposit-by-hash"/>
        Task<WebCallResult<GetSingleDepositByHashResponse>> GetSingleDepositByHashAsync(
            GetSingleDepositByHashRequest request,
            CancellationToken ct = default);

        /// <summary>
        /// Gets a single withdrawal record by withdrawal ID for the broker's sub-accounts.
        /// </summary>
        /// <param name="request">The request object containing the withdrawal ID.</param>
        /// <param name="ct">Cancellation token for asynchronous operations.</param>
        /// <returns>A task representing the asynchronous operation, with the result containing the withdrawal record details.</returns>
        /// <seealso cref="https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/get-single-withdrawal-by-withdrawal-id"/>
        Task<WebCallResult<GetSingleWithdrawalByIdResponse>> GetSingleWithdrawalByIdAsync(
            GetSingleWithdrawalByIdRequest request,
            CancellationToken ct = default);

        /// <summary>
        /// Downloads the Broker rebate orders within a specified date range and transaction type.
        /// </summary>
        /// <param name="request">The request object containing the parameters for the rebate order download.</param>
        /// <param name="ct">Cancellation token for asynchronous operations.</param>
        /// <returns>A task representing the asynchronous operation, with the result containing the download URL of the rebate order file.</returns>
        /// <seealso cref="https://www.kucoin.com/docs/broker/exchange-broker/api-endpoint/download-broker-rebate-order"/>
        Task<WebCallResult<DownloadBrokerRebateOrderResponse>> DownloadBrokerRebateOrderAsync(
            DownloadBrokerRebateOrderRequest request,
            CancellationToken ct = default);
    }
}
