using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace TodoTrack.Data;

/* This is used if database provider does't define
 * ITodoTrackDbSchemaMigrator implementation.
 */
public class NullTodoTrackDbSchemaMigrator : ITodoTrackDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
