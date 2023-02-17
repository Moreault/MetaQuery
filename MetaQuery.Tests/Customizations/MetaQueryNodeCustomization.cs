using ToolBX.Eloquentest.Customizations;

namespace MetaQuery.Tests.Customizations;

[AutoCustomization]
public class MetaQueryNodeCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<IMetaQueryNode>(x => x.FromFactory(fixture.Create<MetaQueryCondition>));
    }
}