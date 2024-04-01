using dotenv.net.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend.DB;

public class PersonContext : DbContext
{
    public DbSet<Person> Persons { get; set; }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // build sql connection string
        var connectionBuilder = new SqlConnectionStringBuilder();
     
        // get the connection string from the environment
        if (EnvReader.TryGetStringValue("DB_CONNECTION",out var connectionString))
            connectionBuilder.ConnectionString = connectionString;

        if (EnvReader.TryGetStringValue("DB_ADDRESS", out var address))
        {
            if (EnvReader.TryGetStringValue("DB_PORT", out var port))
            {
                //connectionBuilder.
                connectionBuilder.DataSource = $"{address},{port}";
            }
            else
            {
                connectionBuilder.DataSource = address;
            }
        }
        
        if (EnvReader.TryGetStringValue("DB_USER",out var user))
            connectionBuilder.UserID = user;
        
        if (EnvReader.TryGetStringValue("DB_PASSWORD",out var pass))
            connectionBuilder.Password = pass;
         
        if (EnvReader.TryGetBooleanValue("DB_TRUST",out var trust))
            connectionBuilder.TrustServerCertificate = trust;

        #if DEBUG
        Console.WriteLine("Connection String: " + connectionBuilder.ConnectionString);
        #endif
        
        options.UseSqlServer(connectionBuilder.ConnectionString);
    }
}