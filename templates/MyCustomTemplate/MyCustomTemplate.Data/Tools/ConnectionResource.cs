namespace MyCustomTemplate.Data.Tools
{
    public class ConnectionResource
    {
        #region Simplest contruction

        public ConnectionResource(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string? ConnectionString { get; }

        #endregion

        #region Assemble connection string

        public ConnectionResource(string metaData, string dataSource, string initialCatalog, string userId, string password)
        {
            MetaData = metaData;
            DataSource = dataSource;
            InitialCatalog = initialCatalog;
            UserId = userId;
            Password = password;
        }

        public string? MetaData { get; }

        public string? DataSource { get; }

        public string? InitialCatalog { get; }

        public string? UserId { get; }

        public string? Password { get; }

        #endregion
    }
}
