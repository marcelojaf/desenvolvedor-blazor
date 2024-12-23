﻿using Microsoft.Data.Sqlite;
using System.Reflection;

namespace VelozientComputers.Infrastructure.Data
{
    /// <summary>
    /// Helper class for database initialization and seeding
    /// </summary>
    public static class DatabaseHelper
    {
        public static void InitializeDatabase(string connectionString)
        {
            var builder = new SqliteConnectionStringBuilder(connectionString);
            var dbPath = builder.DataSource;

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // Check if tables exist
            bool tablesExist = CheckIfTablesExist(connection);

            if (!tablesExist)
            {
                // Read schema SQL
                var schemaScript = @"
                    CREATE TABLE computer_manufacturer (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        serial_regex TEXT
                    );

                    CREATE TABLE computer (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        computer_manufacturer_id INTEGER,
                        serial_number TEXT NOT NULL,
                        specifications TEXT,
                        image_url TEXT,
                        purchase_dt DATETIME,
                        warranty_expiration_dt DATETIME,
                        create_dt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (computer_manufacturer_id) REFERENCES computer_manufacturer(id)
                    );

                    CREATE TABLE user (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        first_name TEXT NOT NULL,
                        last_name TEXT NOT NULL,
                        email_address TEXT NOT NULL,
                        create_dt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );

                    CREATE TABLE computer_status (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        localized_name TEXT NOT NULL
                    );

                    CREATE TABLE lnk_computer_user (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        user_id INTEGER,
                        computer_id INTEGER,
                        assign_start_dt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        assign_end_dt DATETIME,
                        FOREIGN KEY (user_id) REFERENCES user(id),
                        FOREIGN KEY (computer_id) REFERENCES computer(id)
                    );

                    CREATE TABLE lnk_computer_computer_status (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        computer_id INTEGER,
                        computer_status_id INTEGER,
                        assign_dt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (computer_id) REFERENCES computer(id),
                        FOREIGN KEY (computer_status_id) REFERENCES computer_status(id)
                    );";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = schemaScript;
                    command.ExecuteNonQuery();
                }

                // Get the Infrastructure project's base directory
                var assembly = typeof(DatabaseHelper).Assembly;
                var resourceName = "VelozientComputers.Infrastructure.Data.Scripts.populate-database.sql";

                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    throw new FileNotFoundException($"SQL script resource not found: {resourceName}");
                }

                using var reader = new StreamReader(stream);
                var dataScript = reader.ReadToEnd();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = dataScript;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static bool CheckIfTablesExist(SqliteConnection connection)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT name 
                FROM sqlite_master 
                WHERE type='table' 
                AND name='computer_manufacturer'";

            var result = command.ExecuteScalar();
            return result != null;
        }
    }
}