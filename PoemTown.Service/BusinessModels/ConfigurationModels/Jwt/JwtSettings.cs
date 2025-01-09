using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.BusinessModels.ConfigurationModels.Jwt;

public class JwtSettings
{
    public static readonly string ConfigSection = "JwtSettings";
    [Required(ErrorMessage = "Jwt: Key is required")]
    public string Key { get; set; }
    
    [Required(ErrorMessage = "Jwt: Issuer is required")]
    public string Issuer { get; set; }
    
    [Required(ErrorMessage = "Jwt: Audience is required")]
    public string Audience { get; set; }
    
    [Required(ErrorMessage = "Jwt: AccessTokenExpirationMinutes is required")]
    public int AccessTokenExpirationMinutes { get; set; }
    
    [Required(ErrorMessage = "Jwt: RefreshTokenExpirationHours is required")]
    public int RefreshTokenExpirationHours { get; set; }

}