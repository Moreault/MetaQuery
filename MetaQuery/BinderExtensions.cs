namespace ToolBX.MetaQuery;

internal delegate Expression Binder(Expression left, Expression right);

internal static class BinderExtensions
{
    internal static Expression Bind(this Binder binder, Expression? left, Expression right) => left == null ? right : binder(left, right);
}