using Npgsql;

namespace MachineFaultsAPI.DbConnection;

public static class DbConnection
{
    public static NpgsqlConnection GetConnection()
    {
        string host = "127.0.0.1";
        int port = 5432;
        string database = "VanadoAPI";
        string username = "postgres";
        string password = "asddsa";

        string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
        return new NpgsqlConnection(connectionString);
    }
}