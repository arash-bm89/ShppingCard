using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCard.Domain.Dtos;

public class UserLoginDto
{
    public string Name { get; set; }

    public string Password { get; set; }
}