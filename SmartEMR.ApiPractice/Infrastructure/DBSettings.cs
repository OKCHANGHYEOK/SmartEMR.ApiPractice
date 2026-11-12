namespace SmartEMR.ApiPractice.Infrastructure;

public class DBSettings
{
    public string Server { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public bool TrustedConnection { get; set; }
    public bool TrustServerCertificate { get; set; }
}
