namespace Chatter.Persistence.Constants;

public record Provider(string Name, string Assembly) 
{
    public static readonly Provider SqlServer = new (nameof(SqlServer), "Chatter.SqlServerMigrations");
    public static readonly Provider Postgres = new (nameof(Postgres), "Chatter.PostgreMigrations");
}

