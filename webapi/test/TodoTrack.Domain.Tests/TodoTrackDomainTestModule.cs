using TodoTrack.MongoDB;
using Volo.Abp.Modularity;

namespace TodoTrack;

[DependsOn(
    typeof(TodoTrackMongoDbTestModule)
    )]
public class TodoTrackDomainTestModule : AbpModule
{

}
