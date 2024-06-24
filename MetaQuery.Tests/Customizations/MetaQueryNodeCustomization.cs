namespace MetaQuery.Tests.Customizations;

[AutoCustomization]
public class MetaQueryNodeCustomization : CustomizationBase
{
    protected override IEnumerable<Type> Types { get; } = [typeof(MetaQueryCondition)];

    protected override IDummyBuilder BuildMe(IDummy dummy, Type type) => dummy.Build<MetaQueryCondition>().With(x => x.Value,
        () =>
        {
            var types = new[] { typeof(string), typeof(int), typeof(double), typeof(decimal), typeof(DateTime), typeof(bool) };
            return dummy.Create(types.GetRandom());
        });
}