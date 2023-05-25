using System;
using System.Collections.Generic;
using System.Text;
using TodoTrack.Localization;
using Volo.Abp.Application.Services;

namespace TodoTrack;

/* Inherit your application services from this class.
 */
public abstract class TodoTrackAppService : ApplicationService
{
    protected TodoTrackAppService()
    {
        LocalizationResource = typeof(TodoTrackResource);
    }
}
