/*using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ATM.DAL
{
    public class DBServices : IDBServices
    {
        private SqlConnection connection2 = new SqlConnection(@"Data Source=DESKTOP-DM3DDUO\SQLEXPRESS;Initial Catalog=MyAtmDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private bool _disposed;
        private string _connectionString;
        private SqlConnection connection;

        public DBServices()
        {

        }
        public async Task createDB(string serverName)
        {
            string commandString;
            connection = new SqlConnection($@"Server={serverName};Integrated security=SSPI;database=master");

            commandString = "IF NOT EXISTS(SELECT 1 FROM sys.databases WHERE name='MyAtmDB') CREATE DATABASE MyAtmDB ";

            using (SqlCommand myCommand = new SqlCommand(commandString, connection))
            {
                try
                {
                    await myCommand.ExecuteNonQueryAsync();
                    Console.WriteLine("DataBase is Created Successfully");
                    await connection.OpenAsync();

                    commandString = " select  'data source=' + @@servername +   ';initial catalog=' +'ATMDatabase' +   case type_desc when 'WINDOWS_LOGIN' then ';trusted_connection=true'        else           ';user id=' + suser_name() + ';password=<<YourPassword>>'    end    as ConnectionString from sys.server_principals where name = suser_name()";
                    _connectionString = (string)myCommand.ExecuteScalar();
                    Console.WriteLine("Database Created: \n" +
                        "DBName: MyAtmDB");
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

        public void createUserTable()
        {
            string createQ = "CREATE TABLE Users( id INT UNIQUE IDENTITY(1,1) NOT NULL," +
                    "userId uniqueidentifier NOT NULL UNIQUE  DEFAULT newid()," +
                    "name VARCHAR(70) NOT NULL, " +
                    "cardNumber VARCHAR(15) NOT NULL UNIQUE, " +
                    "cardPin VARCHAR(4) NOT NULL, " +
                    "balance DECIMAL(38,2) NOT NULL, " +
                    "status BIT NOT NULL, " +
                    "PRIMARY KEY(Id))";

            SqlCommand myCommand = new SqlCommand(createQ, connection);
            try
            {
                connection.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("User Table Created Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }




        }
        public void insertUserDemoData()
        {

            string insertQuery =
               $"INSERT INTO USERS (Name, cardNumber, cardPin, balance,status)   " +
               $" VALUES ('Dav Hart','33745649437456','1234',100000.89,1), " +
               $" ('Rayn Jim', '33302833330287','5555', 500000000.00,1), " +
               $" ('Pam Micheal', '49684709528753', '1000', 800000.17,1)";




            SqlCommand command = new SqlCommand(insertQuery, connection);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("User Data Created Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        public void createTransactionTable()
        {


            string createQ = "CREATE TABLE Transactions( id INT UNIQUE IDENTITY(1,1) NOT NULL," +
                    "userId uniqueidentifier NOT NULL," +
                    "receiverId uniqueidentifier NULL, " +
                    "transactionType VARCHAR(50) NOT NULL, " +
                    "desctiption TEXT NOT NULL," +
                    "amount DECIMAL(38,2) NOT NULL, " +
                    "status BIT NOT NULL, " +
                    "createdAt DATETIME NOT NULL," +
                    "FOREIGN KEY (userId) REFERENCES Users (userId)," +
                    "FOREIGN KEY (receiverId) REFERENCES Users (userId)," +
                    "PRIMARY KEY(Id))";

            SqlCommand myCommand = new SqlCommand(createQ, connection);
            try
            {
                connection.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("Transaction Table Created Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
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
                connection.Dispose();
            }

            _disposed = true;
        }
        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
*/