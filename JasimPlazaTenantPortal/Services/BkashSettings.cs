namespace Jasim_Plaza_Tenant_Portal.Services;

public class BkashSettings
{
    public bool Enabled { get; set; }
    public bool UseSandbox { get; set; } = true;
    public string BaseUrl { get; set; } = "https://tokenized.sandbox.bka.sh/v1.2.0-beta/tokenized/checkout";
    public string GrantTokenEndpoint { get; set; } = "/token/grant";
    public string CreatePaymentEndpoint { get; set; } = "/create";
    public string ExecutePaymentEndpoint { get; set; } = "/execute";
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AppKey { get; set; } = string.Empty;
    public string AppSecret { get; set; } = string.Empty;
}
