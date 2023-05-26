using TodoTrack.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace TodoTrack.Permissions;

public class TodoTrackPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(TodoTrackPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(TodoTrackPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TodoTrackResource>(name);
    }
}
