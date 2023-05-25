using TodoTrack.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace TodoTrack.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(TodoTrackMongoDbModule),
    typeof(TodoTrackApplicationContractsModule)
    )]
public class TodoTrackDbMigratorModule : AbpModule
{

}
