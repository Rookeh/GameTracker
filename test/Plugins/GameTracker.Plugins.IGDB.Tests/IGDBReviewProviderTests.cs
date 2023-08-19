using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.IGDB.Interfaces;
using GameTracker.Plugins.IGDB.Wrappers;
using IGDB;
using NSubstitute;

namespace GameTracker.Plugins.IGDB.Tests
{
    public class IGDBReviewProviderTests
    {
        private readonly IIGDBClientWrapper _client;
        private readonly IGDBReviewProvider _provider;

        public IGDBReviewProviderTests()
        {
            _client = Substitute.For<IIGDBClientWrapper>();
            var clientFactory = Substitute.For<IIGDBClientWrapperFactory>();
            clientFactory.BuildIGDBClientWrapper(Arg.Any<string>(), Arg.Any<string>()).Returns(_client);
            _provider = new IGDBReviewProvider(clientFactory);
        }        
    }
}