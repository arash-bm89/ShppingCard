﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;

namespace ShoppingCard.Domain.Models;

[Table("Order")]
public class Order : ModelBase
{
    /// <summary>
    ///     using for the confirmed payment for the order
    /// </summary>
    [NotMapped]
    public Payment? ConfirmedPayment => Payments?.SingleOrDefault(x => x.IsConfirmed);


    /// <summary>
    ///     getting whole price after the task been calculated
    /// </summary>
    public decimal FinalPrice { get; set; }


    /// <summary>
    /// foreign key of userId
    /// </summary>
    public Guid UserId { get; set; }


    /// <summary>
    /// navigation property of the user profile
    /// </summary>
    public User User { get; set; }


    /// <summary>
    ///     using for many to many relationship between order and product
    /// </summary>
    public ICollection<OrderProduct> Products { get; set; }


    /// <summary>
    ///     using for efforts for paying the order price
    /// </summary>
    public ICollection<Payment> Payments { get; set; }
}