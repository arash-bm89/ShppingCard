using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace ShoppingCard.Domain.Models;

public class User : ModelBase
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string HashedPassword { get; set; }

    public ICollection<Order> Orders { get; set; }

    [NotMapped] public ICollection<Payment> Payments => Orders.SelectMany(x => x.Payments).ToList();
}