using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCard.Domain.Dtos;

public class UserCreateDto
{
    /// <summary>
    /// this is going to be generated at the first point, possibly the controller
    /// </summary>
    public Guid Id { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// is going to get hashed in userRepository 
    /// </summary>
    public string Password { get; set; }

    public string Email { get; set; }
}