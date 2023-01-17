using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    // todo: ask what is the difference between ifilter and ilistfilter
    public interface IListFilter
    {
        int Offset { get; set; }
        int Count { get; set; }

    }
}
