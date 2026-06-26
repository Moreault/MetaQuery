namespace ToolBX.MetaQuery;

[JsonConverter(typeof(JsonStringEnumConverter<LogicalOperator>))]
public enum LogicalOperator
{
    [Description("&&")]
    And,
    [Description("||")]
    Or
}
