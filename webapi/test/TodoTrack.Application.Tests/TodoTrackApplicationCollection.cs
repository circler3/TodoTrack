using TodoTrack.MongoDB;
using Xunit;

namespace TodoTrack;

[CollectionDefinition(TodoTrackTestConsts.CollectionDefinitionName)]
public class TodoTrackApplicationCollection : TodoTrackMongoDbCollectionFixtureBase
{

}
