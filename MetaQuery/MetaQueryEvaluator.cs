using System.Globalization;

namespace ToolBX.MetaQuery;

/// <summary>
/// Evaluates a single <see cref="MetaQueryCondition"/> against an instance by interpreting the tree
/// directly (no <c>Expression.Compile</c>, no reflection) so it runs on no-JIT/AOT platforms. Field
/// values are read through an <see cref="IMetaQuerySchema{T}"/> and the condition's value is coerced
/// to the field's runtime type before comparison.
/// </summary>
public static class MetaQueryEvaluator
{
    public static bool Evaluate<T>(MetaQueryCondition condition, T item, IMetaQuerySchema<T> schema)
    {
        if (condition is null) throw new ArgumentNullException(nameof(condition));
        if (schema is null) throw new ArgumentNullException(nameof(schema));

        var left = schema.GetAccessor(condition.Field).Invoke(item);
        var right = condition.Value;

        return condition.Operator switch
        {
            ComparisonOperator.Equals => AreEqual(left, right),
            ComparisonOperator.NotEquals => !AreEqual(left, right),
            ComparisonOperator.GreaterThan => Ordered(left, right, c => c > 0),
            ComparisonOperator.GreaterThanOrEquals => Ordered(left, right, c => c >= 0),
            ComparisonOperator.LessThan => Ordered(left, right, c => c < 0),
            ComparisonOperator.LessThanOrEquals => Ordered(left, right, c => c <= 0),
            ComparisonOperator.IsIn => IsIn(left, right),
            ComparisonOperator.IsNotIn => !IsIn(left, right),
            ComparisonOperator.Contains => CollectionContains(left, right),
            _ => throw new NotSupportedException($"Comparison operator '{condition.Operator}' is not supported.")
        };
    }

    private static bool AreEqual(object? left, object? right)
    {
        if (left is null || right is null) return left is null && right is null;
        return Equals(left, Coerce(right, left.GetType()));
    }

    private static bool Ordered(object? left, object? right, Func<int, bool> predicate)
    {
        if (left is null || right is null) return false;
        var coerced = Coerce(right, left.GetType());
        return left is IComparable comparable && coerced is not null && predicate(comparable.CompareTo(coerced));
    }

    private static bool CollectionContains(object? left, object? right)
    {
        if (left is not IEnumerable enumerable || left is string) return false;
        foreach (var element in enumerable)
        {
            if (element is null)
            {
                if (right is null) return true;
                continue;
            }
            if (Equals(element, Coerce(right, element.GetType()))) return true;
        }
        return false;
    }

    private static bool IsIn(object? left, object? right)
    {
        if (right is not IEnumerable enumerable || right is string)
            throw new ArgumentException("The IS IN / IS NOT IN operators require the condition value to be a collection.", nameof(right));

        foreach (var element in enumerable)
        {
            if (left is null)
            {
                if (element is null) return true;
                continue;
            }
            if (element is not null && Equals(left, Coerce(element, left.GetType()))) return true;
        }
        return false;
    }

    private static object? Coerce(object? value, Type targetType)
    {
        if (value is null) return null;
        targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;
        if (targetType.IsInstanceOfType(value)) return value;

        if (targetType == typeof(TimeSpan) && value is string timeSpanString)
            return TimeSpan.Parse(timeSpanString, CultureInfo.InvariantCulture);

        if (value is IConvertible && typeof(IConvertible).IsAssignableFrom(targetType))
        {
            try { return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture); }
            catch (Exception e) when (e is InvalidCastException or FormatException or OverflowException) { return value; }
        }

        return value;
    }
}
