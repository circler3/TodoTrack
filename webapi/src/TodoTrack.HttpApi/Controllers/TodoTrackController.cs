using TodoTrack.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace TodoTrack.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class TodoTrackController : AbpControllerBase
{
    protected TodoTrackController()
    {
        LocalizationResource = typeof(TodoTrackResource);
    }
}
