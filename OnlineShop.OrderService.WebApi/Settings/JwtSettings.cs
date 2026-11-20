namespace OnlineShop.OrderService.WebApi.Settings;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string SigningKey { get; set; } = null!;
}