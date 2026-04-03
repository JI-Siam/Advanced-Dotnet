using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Jasim_Plaza_Tenant_Portal.Services;

public interface IBkashPaymentService
{
    Task<BkashCreatePaymentResult> CreatePaymentAsync(decimal amount, string invoiceId, string callbackUrl);
    Task<BkashExecutePaymentResult> ExecutePaymentAsync(string paymentId);
}

public sealed class BkashCreatePaymentResult
{
    public bool Success { get; init; }
    public string? RedirectUrl { get; init; }
    public string? PaymentId { get; init; }
    public string? Message { get; init; }
}

public sealed class BkashExecutePaymentResult
{
    public bool Success { get; init; }
    public string? TransactionId { get; init; }
    public string? Message { get; init; }
}

public class BkashPaymentService : IBkashPaymentService
{
    private readonly HttpClient httpClient;
    private readonly BkashSettings settings;
    private readonly ILogger<BkashPaymentService> logger;

    public BkashPaymentService(HttpClient httpClient, IOptions<BkashSettings> settings, ILogger<BkashPaymentService> logger)
    {
        this.httpClient = httpClient;
        this.settings = settings.Value;
        this.logger = logger;
    }

    public async Task<BkashCreatePaymentResult> CreatePaymentAsync(decimal amount, string invoiceId, string callbackUrl)
    {
        if (!settings.Enabled)
        {
            return new BkashCreatePaymentResult { Success = false, Message = "bKash payment is not enabled by admin." };
        }

        var token = await GetAccessTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            return new BkashCreatePaymentResult { Success = false, Message = "Unable to authenticate with bKash." };
        }

        var request = new HttpRequestMessage(HttpMethod.Post, BuildUrl(settings.CreatePaymentEndpoint));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Add("X-APP-Key", settings.AppKey);

        request.Content = JsonContent.Create(new
        {
            mode = "0011",
            payerReference = "shopkeeper",
            callbackURL = callbackUrl,
            amount = amount.ToString("0.00", CultureInfo.InvariantCulture),
            currency = "BDT",
            intent = "sale",
            merchantInvoiceNumber = invoiceId
        });

        try
        {
            var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(string.IsNullOrWhiteSpace(json) ? "{}" : json);
            var root = doc.RootElement;
            var bkashUrl = ReadString(root, "bkashURL");
            var paymentId = ReadString(root, "paymentID");
            var statusMessage = ReadString(root, "statusMessage");

            if (!string.IsNullOrWhiteSpace(bkashUrl) && !string.IsNullOrWhiteSpace(paymentId))
            {
                return new BkashCreatePaymentResult
                {
                    Success = true,
                    RedirectUrl = bkashUrl,
                    PaymentId = paymentId
                };
            }

            return new BkashCreatePaymentResult
            {
                Success = false,
                Message = string.IsNullOrWhiteSpace(statusMessage) ? "bKash did not return a payment URL." : statusMessage
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "bKash create payment call failed");
            return new BkashCreatePaymentResult { Success = false, Message = "bKash create payment failed." };
        }
    }

    public async Task<BkashExecutePaymentResult> ExecutePaymentAsync(string paymentId)
    {
        if (!settings.Enabled)
        {
            return new BkashExecutePaymentResult { Success = false, Message = "bKash payment is not enabled by admin." };
        }

        var token = await GetAccessTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            return new BkashExecutePaymentResult { Success = false, Message = "Unable to authenticate with bKash." };
        }

        var request = new HttpRequestMessage(HttpMethod.Post, BuildUrl(settings.ExecutePaymentEndpoint));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Add("X-APP-Key", settings.AppKey);
        request.Content = JsonContent.Create(new { paymentID = paymentId });

        try
        {
            var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(string.IsNullOrWhiteSpace(json) ? "{}" : json);
            var root = doc.RootElement;
            var transactionId = ReadString(root, "trxID");
            var statusMessage = ReadString(root, "statusMessage");

            if (!string.IsNullOrWhiteSpace(transactionId))
            {
                return new BkashExecutePaymentResult { Success = true, TransactionId = transactionId };
            }

            return new BkashExecutePaymentResult
            {
                Success = false,
                Message = string.IsNullOrWhiteSpace(statusMessage) ? "bKash did not confirm the payment." : statusMessage
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "bKash execute payment call failed");
            return new BkashExecutePaymentResult { Success = false, Message = "bKash execute payment failed." };
        }
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(settings.Username) ||
            string.IsNullOrWhiteSpace(settings.Password) ||
            string.IsNullOrWhiteSpace(settings.AppKey) ||
            string.IsNullOrWhiteSpace(settings.AppSecret))
        {
            return null;
        }

        var request = new HttpRequestMessage(HttpMethod.Post, BuildUrl(settings.GrantTokenEndpoint));
        request.Headers.Add("username", settings.Username);
        request.Headers.Add("password", settings.Password);
        request.Content = JsonContent.Create(new { app_key = settings.AppKey, app_secret = settings.AppSecret });

        try
        {
            var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(string.IsNullOrWhiteSpace(json) ? "{}" : json);
            return ReadString(doc.RootElement, "id_token");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "bKash token request failed");
            return null;
        }
    }

    private string BuildUrl(string endpoint)
    {
        var baseUrl = settings.BaseUrl.TrimEnd('/');
        var path = endpoint.StartsWith('/') ? endpoint : "/" + endpoint;
        return baseUrl + path;
    }

    private static string? ReadString(JsonElement element, string name)
    {
        return element.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;
    }
}
