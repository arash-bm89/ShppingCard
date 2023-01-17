using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingCard.Domain.Models
{
    public class Product: ModelBase
    {
        /// <summary>
        /// name of the product
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// price of the product
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// number of available products
        /// </summary>
        [Required]
        public uint NumberOfAvailable { get; set; }

        [MaxLength(100)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// using for many to many relationship between basket and product
        /// </summary>
        
        public ICollection<BasketProduct> BasketProducts { get; set; }
    }

}
