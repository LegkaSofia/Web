using Neo4jClient;
using System;

namespace Cover.Neo4J.Domain.Core.Settings.Factory
{
    public class CoverGraphDatabaseClientFactory
    {
        private readonly ICoverGraphDatabaseSettings _settings;

        public CoverGraphDatabaseClientFactory(ICoverGraphDatabaseSettings settings)
        {
            _settings = settings;
        }

        public GraphClient GetClient()
        {
            return new GraphClient(new Uri(_settings.Url), _settings.DatabaseLogin, _settings.DatabasePassword);
        }
    }
}
