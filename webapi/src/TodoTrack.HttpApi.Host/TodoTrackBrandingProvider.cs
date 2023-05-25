using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace TodoTrack;

[Dependency(ReplaceServices = true)]
public class TodoTrackBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "TodoTrack";
}
