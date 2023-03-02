/*using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using static System.Console;

namespace ATM.DAL.DBConnection
{
    internal class DBScript : IDBScript
    {
        private SqlConnection connection;
        private DbService _dbService;
        private bool _disposed;
        private string _connectionString;
        public DBScript(DbService dbService)
        {
            _dbService = dbService;
        }





        public async Task CreateDBAsync(string serverName)
        {

            string commandString;
            connection = new SqlConnection(serverName);

            commandString = "IF NOT EXISTS(SELECT 1 FROM sys.databases WHERE name='ATMDatabase') CREATE DATABASE ATMDatabase ";

            using (SqlCommand myCommand = new SqlCommand(commandString, connection))
            {
                try
                {
                    await myCommand.ExecuteNonQueryAsync();
                    Console.WriteLine("DataBase is Created Successfully");
                    connection.Open();

                    commandString = " select  'data source=' + @@servername +   ';initial catalog=' +'ATMDatabase' +   case type_desc        when 'WINDOWS_LOGIN'            then ';trusted_connection=true'        else           ';user id=' + suser_name() + ';password=<<YourPassword>>'    end    as ConnectionString from sys.server_principals where name = suser_name()";
                    _connectionString = (string)myCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        await connection.CloseAsync();
                    }

                }
            }
        }
        public async Task<string> GetConnectionString(string serverName)
        {

            string commandString;
            connection = new SqlConnection(serverName);

            commandString = " select  'data source=' + @@servername +   ';initial catalog=' +'ATMDatabase' +   case type_desc  when 'WINDOWS_LOGIN'   then ';trusted_connection=true;Integrated security = True;ENCRYPT = False'        else           ';user id=' + suser_name() + ';password=<<YourPassword>>'    end    as ConnectionString from sys.server_principals where name = suser_name()";


            using (SqlCommand myCommand = new SqlCommand(commandString, connection))
            {
                try
                {
                    connection.Open();
                    return (string)myCommand.ExecuteScalar();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        await connection.CloseAsync();
                    }

                }
            }
        }
        public async Task CreateDatabaseTablesAsync()
        {
            string connectionString = await GetConnectionString(@"Server=CHARLES\MYDB;TrustServerCertificate=True;Integrated security = True; Initial Catalog = master; Encrypt=False");

            WriteLine(connectionString);
            connection = new SqlConnection(connectionString);
            string createQuery = @"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AccountUser' and xtype='U')
            CREATE TABLE AccountUser(
	            userId UNIQUEIDENTIFIER UNIQUE  NOT NULL Default newID(),   
  	            accountNumber BIGINT UNIQUE NOT NULL,
	            accountBalance DECIMAL,
	            userName VARCHAR(10) NOT NULL,
	            pin VARCHAR(50) NOT NULL,
	            PRIMARY KEY(userId));
	              
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ATM' and xtype='U')
            CREATE TABLE ATM(
            	atmId UNIQUEIDENTIFIER UNIQUE  NOT NULL Default newID(), 
  	            currentUser UNIQUEIDENTIFIER ,
	            availableCash DECIMAL,
	            isActive BIT NOT NULL,
	            FOREIGN KEY (currentUser) REFERENCES AccountUser (userId),
	            PRIMARY KEY(atmId));
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Transactions' and xtype='U')
            CREATE TABLE Transactions(
            	transactionId UNIQUEIDENTIFIER UNIQUE  NOT NULL Default newID(),  
  	            currentUser UNIQUEIDENTIFIER ,
	            receiver UNIQUEIDENTIFIER ,
	            amount DECIMAL,
	            balance DECIMAL,
                remarks VARCHAR,
	            transactionType VARCHAR,
	            FOREIGN KEY (currentUser) REFERENCES AccountUser (userId),
	            FOREIGN KEY (receiver) REFERENCES AccountUser (userId),
	            PRIMARY KEY(transactionId));
";

            SqlCommand myCommand = new SqlCommand(createQuery, connection);
            try
            {
                await connection.OpenAsync();
                await myCommand.ExecuteNonQueryAsync();
                Console.WriteLine("Tables Created Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }

        }
        public async Task CreateAccountAsync()
        {
            string connectionString = await GetConnectionString(@"Server=CHARLES\MYDB;TrustServerCertificate=True;Integrated security = True; Initial Catalog = master; Encrypt=False");

            WriteLine(connectionString);
            connection = new SqlConnection(connectionString);
            string createQuery = @"
            
";

            SqlCommand myCommand = new SqlCommand(createQuery, connection);
            try
            {
                await connection.OpenAsync();
                await myCommand.ExecuteNonQueryAsync();
                Console.WriteLine("Account Created Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }

        }


        protected virtual void Dispose(bool disposing)
        {

            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbService.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}*/