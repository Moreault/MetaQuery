namespace ToolBX.MetaQuery;

[JsonConverter(typeof(JsonStringEnumConverter<ComparisonOperator>))]
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
    /// <summary>
    /// The (scalar) field's value is one of the values in the supplied collection.
    /// </summary>
    [Description("IS IN")]
    IsIn,
    /// <summary>
    /// The (scalar) field's value is not one of the values in the supplied collection.
    /// </summary>
    [Description("IS NOT IN")]
    IsNotIn,
    /// <summary>
    /// The field is itself a collection and contains the supplied (scalar) value.
    /// </summary>
    [Description("CONTAINS")]
    Contains
}
