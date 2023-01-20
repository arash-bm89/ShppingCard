using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class PaginatedResponseResult<TModel> 
        where TModel : class
    {
        public List<TModel>? Items { get; set; }
        public int TotalCount { get; set; }
        public bool HasAnyItems() => Items != null && Items.Count != 0;
    }
}
