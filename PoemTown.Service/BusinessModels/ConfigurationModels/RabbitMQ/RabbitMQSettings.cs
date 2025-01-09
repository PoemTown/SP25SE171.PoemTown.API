using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.BusinessModels.ConfigurationModels.RabbitMQ;

public class RabbitMQSettings
{
    public static readonly string ConfigSection = "RabbitMq";
    [Required(ErrorMessage = "RabbiqMQ: Host is required")]
    public string Host { get; set; }

    [Required(ErrorMessage = "RabbitMQ: Port is required")]
    public string Port { get; set; }
    
    [Required(ErrorMessage = "RabbitMQ: Username is required")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "RabbitMQ: Password is required")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "RabbitMQ: VirtualHost is required")]
    public string VirtualHost { get; set; }

}