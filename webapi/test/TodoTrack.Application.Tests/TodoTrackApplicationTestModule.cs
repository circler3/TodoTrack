using Volo.Abp.Modularity;

namespace TodoTrack;

[DependsOn(
    typeof(TodoTrackApplicationModule),
    typeof(TodoTrackDomainTestModule)
    )]
public class TodoTrackApplicationTestModule : AbpModule
{

}
