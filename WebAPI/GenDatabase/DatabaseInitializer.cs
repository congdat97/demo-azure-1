using Dapper;
using Npgsql;

namespace GenericRepo_Dapper.GenDatabase
{
    public class DatabaseInitializer
    {
        private readonly string _masterConnStr;
        private readonly string _defaultConnStr;

        public DatabaseInitializer(IConfiguration config)
        {
            _masterConnStr = config.GetConnectionString("Master");
            _defaultConnStr = config.GetConnectionString("DefaultConnection");
        }

        public void Initialize()
        {
            CreateDatabaseIfNotExists();
            CreateTables();
            InsertSeedData();
        }

        private void CreateDatabaseIfNotExists()
        {
            using var connection = new NpgsqlConnection(_masterConnStr);
            connection.Open();

            var checkDbSql = "SELECT 1 FROM pg_database WHERE datname = 'GenericRepoDapperDb'";
            var exists = connection.ExecuteScalar<int?>(checkDbSql);
            if (exists != 1)
            {
                connection.Execute("CREATE DATABASE \"GenericRepoDapperDb\"");
            }
        }

        private void CreateTables()
        {
            using var connection = new NpgsqlConnection(_defaultConnStr);
            connection.Open();

            var createTableSql = @"
            CREATE TABLE IF NOT EXISTS Categories (
                Id SERIAL PRIMARY KEY,
                Name VARCHAR(100) NOT NULL,
                Description TEXT
            );";
            connection.Execute(createTableSql);
        }

        private void InsertSeedData()
        {
            using var connection = new NpgsqlConnection(_defaultConnStr);
            connection.Open();

            var count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Categories");
            if (count == 0)
            {
                var insertSql = @"
                INSERT INTO Categories (Name, Description)
                VALUES 
                ('Technology', 'Tech stuff'),
                ('Books', 'Books and novels'),
                ('Groceries', 'Daily items');";
                connection.Execute(insertSql);
            }
        }
    }
}
