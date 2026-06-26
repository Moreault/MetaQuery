namespace ToolBX.MetaQuery;

/// <summary>
/// Maps the string field names used by <see cref="MetaQueryCondition"/> to strongly-typed accessors
/// on <typeparamref name="T"/>. This is the AOT-safe, reflection-free replacement for resolving
/// fields by reflection : consumers register every queryable field explicitly.
/// </summary>
public interface IMetaQuerySchema<T>
{
    bool TryGetAccessor(string field, out Func<T, object?> accessor);
    Func<T, object?> GetAccessor(string field);
}

public sealed class MetaQuerySchema<T> : IMetaQuerySchema<T>
{
    private readonly Dictionary<string, Func<T, object?>> _accessors = new(StringComparer.Ordinal);

    public MetaQuerySchema<T> Field(string name, Func<T, object?> accessor)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (accessor is null) throw new ArgumentNullException(nameof(accessor));
        _accessors[name] = accessor;
        return this;
    }

    public bool TryGetAccessor(string field, out Func<T, object?> accessor) => _accessors.TryGetValue(field, out accessor!);

    public Func<T, object?> GetAccessor(string field)
    {
        if (!_accessors.TryGetValue(field, out var accessor))
            throw new ArgumentException($"Field '{field}' is not registered in the schema for type '{typeof(T).Name}'.", nameof(field));
        return accessor;
    }
}
