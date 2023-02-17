namespace ToolBX.MetaQuery;

internal static class EnumerableParser
{
    private static readonly Dictionary<LogicalOperator, Binder> Binders = new()
    {
        { LogicalOperator.BitwiseAnd, Expression.And },
        { LogicalOperator.BitwiseOr, Expression.Or },
        { LogicalOperator.And, Expression.AndAlso },
        { LogicalOperator.Or, Expression.OrElse },
    };

    private static class Methods
    {
        internal static readonly MethodInfo Contains = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.Name == nameof(Enumerable.Contains) && x.GetParameters().Length == 2);

    }

    internal static Expression<Func<T, bool>> Parse<T>(MetaQueryPredicate predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        var itemExpression = Expression.Parameter(typeof(T));
        var conditions = predicate.Any() ? ParseTree<T>(predicate, itemExpression) : Expression.Constant(false);
        if (conditions.CanReduce)
            conditions = conditions.ReduceAndCheck();

        return Expression.Lambda<Func<T, bool>>(conditions, itemExpression);
    }

    private static Expression ParseTree<T>(MetaQueryPredicate predicate, ParameterExpression itemExpression)
    {
        if (!predicate.Any()) 
            return Expression.Constant(true);

        Expression left = null!;
        var binder = Binders[predicate.Operator];

        foreach (var node in predicate)
        {
            if (node is MetaQueryPredicate innerPredicate)
            {
                var right = ParseTree<T>(innerPredicate, itemExpression);
                left = binder.Bind(left, right);
            }
            else if (node is MetaQueryCondition condition)
            {
                var propertyPath = typeof(T).GetPropertyPath(condition.Field);

                Expression propertyExpression = itemExpression;
                foreach (var path in propertyPath)
                {
                    propertyExpression = Expression.PropertyOrField(propertyExpression, path.Property.Name);
                }

                if (condition.Operator is ComparisonOperator.IsIn or ComparisonOperator.IsNotIn)
                {
                    var containsMethod = Methods.Contains.MakeGenericMethod(typeof(string));
                    var enumerable = (IEnumerable)condition.Value!;
                    var right = Expression.Call(containsMethod, Expression.Constant(enumerable), propertyExpression);
                    left = condition.Operator == ComparisonOperator.IsIn ? binder.Bind(left, right) : binder.Bind(left, Expression.Not(right));
                }
                else
                {
                    var toCompare = Expression.Constant(condition.Value);
                    var right = condition.Operator.ToBinaryExpression(propertyExpression, toCompare);
                    left = binder.Bind(left, right);
                }
            }
            else throw new NotSupportedException(string.Format(Exceptions.TypeUnsupportedByParser, node.GetType().Name));
        }

        return left;
    }
}