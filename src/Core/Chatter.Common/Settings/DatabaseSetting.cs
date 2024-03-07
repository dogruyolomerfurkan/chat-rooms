namespace Chatter.Common.Settings;

public class DatabaseSetting
{
    public MongoDbSettings MongoDbSettings { get; set; }
    public string Postgres { get; set; }
    public string SqlServer { get; set; }
    public string Provider { get; set; } 
}

public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}