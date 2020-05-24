namespace Cover.Neo4J.Domain.Core.Settings
{
    public class CoverGraphDatabaseSettings : ICoverGraphDatabaseSettings
    {
        public string Url { get; set; }

        public string DatabaseLogin { get; set; }

        public string DatabasePassword { get; set; }
    }
}
