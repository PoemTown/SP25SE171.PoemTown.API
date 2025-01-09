using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.BusinessModels.ConfigurationModels.Email;

public class EmailSettings
{
    [Required(ErrorMessage = "Smtp: Host is required")]
    public string Host { get; set; }
    [Required(ErrorMessage = "Smtp: Port is required")]
    public int Port { get; set; }
    [Required(ErrorMessage = "Smtp: Username is required")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Smtp: Password is required")]
    public string Password { get; set; }
}