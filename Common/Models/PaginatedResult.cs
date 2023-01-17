using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    // todo: why in main source code they use ipaginatedResult and imodelBase
    public class PaginatedResult<TModelBase> 
    where TModelBase : ModelBase
    {
        public List<TModelBase>? Items { get; set; }
        public int TotalCount { get; set; }
        public bool HasAnyItems() => this.Items != null && this.Items.Count != 0;
    }
}
