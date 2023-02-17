namespace ToolBX.MetaQuery;

public enum LogicalOperator
{
    [Description("&&")]
    And,
    [Description("||")]
    Or,
    [Description("&")]
    BitwiseAnd,
    [Description("|")]
    BitwiseOr
}