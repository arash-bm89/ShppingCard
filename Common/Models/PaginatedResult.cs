using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class PaginatedResult<TModelBase> 
    where TModelBase : ModelBase
    {
        public List<TModelBase>? Items { get; set; }
        public int TotalCount { get; set; }
        public bool HasAnyItems() => Items != null && Items.Count != 0;

    }

    
}
