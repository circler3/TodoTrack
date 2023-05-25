using Volo.Abp.Settings;

namespace TodoTrack.Settings;

public class TodoTrackSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(TodoTrackSettings.MySetting1));
    }
}
