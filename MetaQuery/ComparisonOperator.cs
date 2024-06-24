namespace ToolBX.MetaQuery;

[JsonConverter(typeof(ComparisonOperatorJsonConverter))]
public enum ComparisonOperator
{
    [Description("==")]
    Equals,
    [Description("!=")]
    NotEquals,
    [Description(">")]
    GreaterThan,
    [Description("<")]
    LessThan,
    [Description(">=")]
    GreaterThanOrEquals,
    [Description("<=")]
    LessThanOrEquals,
    [Description("IS IN")]
    IsIn,
    [Description("IS NOT IN")]
    IsNotIn
}

internal static class ComparisonOperatorExtensions
{
    internal static BinaryExpression ToBinaryExpression(this ComparisonOperator value, Expression left, ConstantExpression right)
    {
        if (left == null) throw new ArgumentNullException(nameof(left));
        if (right == null) throw new ArgumentNullException(nameof(right));

        switch (value)
        {
            case ComparisonOperator.Equals:
                return Expression.Equal(left, right);
            case ComparisonOperator.NotEquals:
                return Expression.NotEqual(left, right);
            case ComparisonOperator.GreaterThan:
               return Expression.GreaterThan(left, right);
            case ComparisonOperator.GreaterThanOrEquals:
                return Expression.GreaterThanOrEqual(left, right);
            case ComparisonOperator.LessThan:
                return Expression.LessThan(left, right);
            case ComparisonOperator.LessThanOrEquals:
                return Expression.LessThanOrEqual(left, right);
            default:
                throw new NotSupportedException(string.Format(Exceptions.ComparisonOperatorUnsupported, value));
        }
    }
}