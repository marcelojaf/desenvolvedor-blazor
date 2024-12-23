using Microsoft.Data.Sqlite;
using System.Reflection;

namespace VelozientComputers.Infrastructure.Data
{
    /// <summary>
    /// Helper class for database initialization and seeding.
    /// Provides methods to create database tables and populate initial data.
    /// </summary>
    public static class DatabaseHelper
    {
        /// <summary>
        /// SQL script containing the CREATE TABLE statements for all required database tables.
        /// Uses IF NOT EXISTS clause to avoid errors if tables already exist.
        /// </summary>
        private const string CREATE_TABLES_SCRIPT = @"
        CREATE TABLE IF NOT EXISTS computer_manufacturer (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            serial_regex TEXT
        );

        CREATE TABLE IF NOT EXISTS computer (
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

        CREATE TABLE IF NOT EXISTS user (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            first_name TEXT NOT NULL,
            last_name TEXT NOT NULL,
            email_address TEXT NOT NULL,
            create_dt DATETIME DEFAULT CURRENT_TIMESTAMP
        );

        CREATE TABLE IF NOT EXISTS computer_status (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            localized_name TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS lnk_computer_user (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER,
            computer_id INTEGER,
            assign_start_dt DATETIME DEFAULT CURRENT_TIMESTAMP,
            assign_end_dt DATETIME,
            FOREIGN KEY (user_id) REFERENCES user(id),
            FOREIGN KEY (computer_id) REFERENCES computer(id)
        );

        CREATE TABLE IF NOT EXISTS lnk_computer_computer_status (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            computer_id INTEGER,
            computer_status_id INTEGER,
            assign_dt DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (computer_id) REFERENCES computer(id),
            FOREIGN KEY (computer_status_id) REFERENCES computer_status(id)
        );";

        /// <summary>
        /// Initializes the database by ensuring all tables exist and contain initial data.
        /// This method orchestrates the database setup process by:
        /// 1. Creating the database directory if it doesn't exist
        /// 2. Creating tables if they don't exist
        /// 3. Populating initial data if the database is empty
        /// </summary>
        /// <param name="connectionString">The SQLite connection string pointing to the database file</param>
        /// <exception cref="Exception">Thrown when database initialization fails</exception>
        public static void InitializeDatabase(string connectionString)
        {
            try
            {
                var builder = new SqliteConnectionStringBuilder(connectionString);
                EnsureDirectoryExists(builder.DataSource);

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                CreateTablesIfNotExist(connection);

                if (!HasAnyData(connection))
                {
                    PopulateInitialData(connection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new Exception("Failed to initialize database", ex);
            }
        }

        /// <summary>
        /// Ensures that the directory for the database file exists.
        /// Creates the directory and any necessary parent directories if they don't exist.
        /// </summary>
        /// <param name="dbPath">The full path to the database file</param>
        /// <remarks>
        /// This method is important for first-time setup when the database
        /// directory might not exist yet.
        /// </remarks>
        private static void EnsureDirectoryExists(string dbPath)
        {
            var directory = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Creates all required database tables if they don't already exist.
        /// Uses the CREATE_TABLES_SCRIPT constant which contains CREATE TABLE IF NOT EXISTS statements.
        /// </summary>
        /// <param name="connection">An open SQLite connection</param>
        /// <exception cref="Exception">Thrown when table creation fails</exception>
        /// <remarks>
        /// This method is safe to call multiple times as it uses IF NOT EXISTS clauses
        /// in all CREATE TABLE statements.
        /// </remarks>
        private static void CreateTablesIfNotExist(SqliteConnection connection)
        {
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = CREATE_TABLES_SCRIPT;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create database tables", ex);
            }
        }

        /// <summary>
        /// Checks if the database contains any data by counting records in the computer_manufacturer table.
        /// Used to determine if initial data population is needed.
        /// </summary>
        /// <param name="connection">An open SQLite connection</param>
        /// <returns>True if the database contains any data, false otherwise</returns>
        /// <remarks>
        /// This method uses the computer_manufacturer table as an indicator for whether
        /// the database has been populated, as this table should always contain data
        /// in a properly initialized database.
        /// </remarks>
        private static bool HasAnyData(SqliteConnection connection)
        {
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM computer_manufacturer";
                var count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking for existing data: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Populates the database with initial data by executing an embedded SQL script.
        /// The script is expected to be an embedded resource in the assembly.
        /// </summary>
        /// <param name="connection">An open SQLite connection</param>
        /// <exception cref="Exception">Thrown when data population fails</exception>
        /// <exception cref="FileNotFoundException">Thrown when the population script cannot be found</exception>
        /// <exception cref="InvalidOperationException">Thrown when the population script is empty</exception>
        /// <remarks>
        /// The population script must be included in the project as an embedded resource
        /// with the name "VelozientComputers.Infrastructure.Data.Scripts.populate-database.sql"
        /// </remarks>
        private static void PopulateInitialData(SqliteConnection connection)
        {
            try
            {
                var assembly = typeof(DatabaseHelper).Assembly;
                var resourceName = "VelozientComputers.Infrastructure.Data.Scripts.populate-database.sql";

                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    throw new FileNotFoundException($"Population script not found: {resourceName}");
                }

                using var reader = new StreamReader(stream);
                var dataScript = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(dataScript))
                {
                    throw new InvalidOperationException("Population script is empty");
                }

                using var command = connection.CreateCommand();
                command.CommandText = dataScript;
                command.ExecuteNonQuery();

                Console.WriteLine("Successfully populated initial data");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to populate initial data", ex);
            }
        }
    }
}