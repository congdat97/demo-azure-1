using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Utility.Model;

namespace Domain.Database;

public class ApplicationDbContext
{
    private readonly ConnectDatabase _connectDatabase;

    public ApplicationDbContext(ConnectDatabase connectDatabase) 
    {
        _connectDatabase = connectDatabase;
    }

    public IDbConnection CreateConnection(string connectionString = "DefaultConnection")
    {
        return new NpgsqlConnection(_connectDatabase.DefaultConnection);
    }

}