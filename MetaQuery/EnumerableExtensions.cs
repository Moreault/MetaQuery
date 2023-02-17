namespace ToolBX.MetaQuery;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, MetaQueryPredicate predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        var expression = EnumerableParser.Parse<T>(predicate).Compile();
        return source.Where(expression);
    }
}