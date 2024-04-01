using dotenv.net.Utilities;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

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
        
        Console.WriteLine("Connection String: " + connectionBuilder.ConnectionString);
        
        options.UseSqlServer(connectionBuilder.ConnectionString);
    }
}