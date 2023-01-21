namespace Common.ExtensionMethods;

public static class NpgSqlExtenstionMethods
{
    public static IQueryable<TDestination> Apply<TSource, TDestination>(
        this IQueryable<TSource> source,
        Func<IQueryable<TSource>, IQueryable<TDestination>> builder)
    {
        return builder(source);
    }
}