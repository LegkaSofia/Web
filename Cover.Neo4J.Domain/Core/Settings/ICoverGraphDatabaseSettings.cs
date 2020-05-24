namespace Cover.Neo4J.Domain.Core.Settings
{
    public interface ICoverGraphDatabaseSettings
    {
        string Url { get; set; }

        string DatabaseLogin { get; set; }

        string DatabasePassword { get; set; }
    }
}
