using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace TodoTrack.MongoDB;

[DependsOn(
    typeof(TodoTrackTestBaseModule),
    typeof(TodoTrackMongoDbModule)
    )]
public class TodoTrackMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var stringArray = TodoTrackMongoDbFixture.ConnectionString.Split('?');
        var connectionString = stringArray[0].EnsureEndsWith('/') +
                                   "Db_" +
                               Guid.NewGuid().ToString("N") + "/?" + stringArray[1];

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = connectionString;
        });
    }
}
