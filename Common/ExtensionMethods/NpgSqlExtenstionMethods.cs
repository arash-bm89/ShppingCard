using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethods
{
    public static class NpgSqlExtenstionMethods
    {
        // todo: what is this?
        public static IQueryable<TDestination> Apply<TSource, TDestination>(
            this IQueryable<TSource> source,
            Func<IQueryable<TSource>, IQueryable<TDestination>> builder)
        {
            return builder(source);
        }
    }
}
