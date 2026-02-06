using Dapper;
using FileToSQL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace FileToSQL.Services
{
    public class DatabaseService
    {
        private readonly DatabaseSettings _settings;

        public DatabaseService(IOptions<DatabaseSettings> options)
        {
            _settings = options.Value;
        }

      
        public void CreateTableAndInsert(string databaseName,string tableName, FilePreviewResult data)
        {
            EnsureDatabaseExists(databaseName);

            string BuildConnectionString = $"Server={_settings.Server};Database={databaseName};Trusted_Connection=true;";

            using var connection = new SqlConnection(BuildConnectionString);
            connection.Open();

            var createSql = $"CREATE TABLE [{tableName}] ("  +
                            string.Join(", ", data.ColumnNames.Select(col => $"[{col}] NVARCHAR(MAX)")) + 
                            ")";

            connection.Execute(createSql);
            InsertData(databaseName,tableName, data);

        }

        public void InsertData(string databaseName,string tableName, FilePreviewResult data)
        {
            EnsureDatabaseExists(databaseName);


            string BuildConnectionString = $"Server={_settings.Server};Database={databaseName};Trusted_Connection=true;";
            using var connection = new SqlConnection(BuildConnectionString);
            connection.Open();

            var columnNames = string.Join(", ", data.ColumnNames.Select(col => $"[{col}]"));
            var paramNames = string.Join(", ", data.ColumnNames.Select(col => $"@{col}"));

            var insertSql = $"INSERT INTO [{tableName}] ({columnNames}) VALUES ({paramNames})";


            foreach (var row in data.Rows)
            {
                var parameters = new DynamicParameters();

                for (int i = 0; i < data.ColumnNames.Count; i++)
                {
                    parameters.Add(data.ColumnNames[i], row[i]);
                }
                connection.Execute(insertSql, parameters);
            }
        }

        public void EnsureDatabaseExists(string databaseName)
        {
            string BuildConnectionString = $"Server={_settings.Server};Database=master;Trusted_Connection=true;";

            using var connection = new SqlConnection(BuildConnectionString);
            connection.Open();

            var existsSql = @"
        SELECT COUNT(1)
        FROM sys.databases
        WHERE name = @DatabaseName";

            var exists = connection.ExecuteScalar<int>(
                existsSql,
                new { DatabaseName = databaseName }
            );

            if (exists == 0)
            {
                connection.Execute($"CREATE DATABASE [{databaseName}]");
            }
        }

    }
}
