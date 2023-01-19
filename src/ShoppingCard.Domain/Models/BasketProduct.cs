using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingCard.Domain.Models
{
    public class BasketProduct: ModelBase
    {

        public Guid BasketId { get; set; }
        public Basket Basket { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        /// <summary>
        /// count of this product in basket
        /// </summary>
        public uint CountOfProduct { get; set; }

        /// <summary>
        /// price of all products of this type in a basket
        /// </summary>
        [NotMapped]
        public decimal BasketProductPrice => CountOfProduct * Product.Price;
    }
    
}
