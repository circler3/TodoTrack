using TodoTrack.MongoDB;
using Xunit;

namespace TodoTrack;

[CollectionDefinition(TodoTrackTestConsts.CollectionDefinitionName)]
public class TodoTrackDomainCollection : TodoTrackMongoDbCollectionFixtureBase
{

}
