using System.ComponentModel.DataAnnotations;

namespace ShoppingCard.Api.Models;

public class UserCreateRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Password { get; set; }
    [Required]

    public string Email { get; set; }
}