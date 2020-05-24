namespace Cover.Domain.Data
{
    public class CoverDatabaseSettings : ICoverDatabaseSettings
    {
        public string PostsCollectionName { get; set; }

        public string UsersCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}