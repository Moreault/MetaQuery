namespace ToolBX.MetaQuery;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, MetaQueryFilter filter, IMetaQuerySchema<T> schema)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (filter is null) throw new ArgumentNullException(nameof(filter));
        if (schema is null) throw new ArgumentNullException(nameof(schema));
        return source.Where(x => filter.IsSatisfiedBy(x, schema));
    }

    public static bool Any<T>(this IEnumerable<T> source, MetaQueryFilter filter, IMetaQuerySchema<T> schema)
        => source.Where(filter, schema).Any();
}
