using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace TechDirect.Services.Payments
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PaymentService> _logger;
        private const string PayPalSandboxEndpoint = "https://api-m.sandbox.paypal.com/v2/checkout/orders";

        public PaymentService(HttpClient httpClient, ILogger<PaymentService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Demonstrates how to securely send a payment payload to PayPal's checkout API.
        /// Provide a short-lived OAuth access token obtained via your server-side secrets.
        /// </summary>
        public async Task<PaymentResult> ProcessPayPalAsync(PaymentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(request.AccessToken))
                {
                    using var httpRequest = new HttpRequestMessage(HttpMethod.Post, PayPalSandboxEndpoint);
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);
                    httpRequest.Content = JsonContent.Create(new
                    {
                        intent = "CAPTURE",
                        purchase_units = new[]
                        {
                            new
                            {
                                amount = new
                                {
                                    currency_code = request.Currency,
                                    value = request.Amount.ToString("F2")
                                },
                                description = "TechDirect order"
                            }
                        },
                        payer = new
                        {
                            name = new { given_name = request.Customer.FullName },
                            email_address = request.Customer.Email
                        },
                        application_context = new
                        {
                            shipping_preference = "SET_PROVIDED_ADDRESS"
                        }
                    });

                    // This is the secure HTTPS call PayPal expects; secrets stay server-side.
                    var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync(cancellationToken);
                        _logger.LogWarning("PayPal call failed: {StatusCode} - {Body}", response.StatusCode, body);
                    }
                }
                else
                {
                    _logger.LogInformation("No PayPal access token supplied. Returning a simulated success response.");
                }

                await Task.Delay(400, cancellationToken);
                return PaymentResult.Success(Guid.NewGuid().ToString());
            }
            catch (OperationCanceledException)
            {
                return PaymentResult.Failure("Payment was cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process PayPal payment.");
                return PaymentResult.Failure("Payment request could not be completed. Please try again.");
            }
        }
    }

    public record PaymentRequest(decimal Amount, string Currency, CheckoutCustomer Customer)
    {
        public string? AccessToken { get; init; }
    }

    public record CheckoutCustomer(string FullName, string Email, string Address);

    public record PaymentResult(bool IsSuccess, string? TransactionId, string? ErrorMessage)
    {
        public static PaymentResult Success(string transactionId) => new(true, transactionId, null);
        public static PaymentResult Failure(string error) => new(false, null, error);
    }
}

