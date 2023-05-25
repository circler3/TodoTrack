using System.Threading.Tasks;

namespace TodoTrack.Data;

public interface ITodoTrackDbSchemaMigrator
{
    Task MigrateAsync();
}
