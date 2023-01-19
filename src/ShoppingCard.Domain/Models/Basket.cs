using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingCard.Domain.Models
{
    public class Basket: ModelBase
    {
        /// <summary>
        /// using for efforts for paying the basket price 
        /// </summary>
        public ICollection<Payment> Payments { get; set; }

        /// <summary>
        /// using for the confirmed payment for the basket
        /// </summary>
        [NotMapped]
        public Payment? ConfirmedPayment => Payments?.Single(x => x.IsConfirmed);

        /// <summary>
        /// using for many to many relationship between basket and product
        /// </summary>
        public ICollection<BasketProduct> BasketProducts { get; set; }

        /// <summary>
        /// getting whole price after the task been calculated
        /// </summary>
        [NotMapped]
        public decimal? FinalPrice => BasketProducts?.ToList().Sum(x => x.BasketProductPrice) * (decimal)1.09;

    }

}
