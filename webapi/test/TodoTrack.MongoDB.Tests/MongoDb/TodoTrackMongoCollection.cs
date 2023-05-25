using Xunit;

namespace TodoTrack.MongoDB;

[CollectionDefinition(TodoTrackTestConsts.CollectionDefinitionName)]
public class TodoTrackMongoCollection : TodoTrackMongoDbCollectionFixtureBase
{

}
