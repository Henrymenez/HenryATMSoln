using ATM.DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace ATM.BL
{
    public class createDB : IDBServices
    {
        private readonly AtmDBConnect _dbContext;
        private bool _disposed;

        public createDB(AtmDBConnect atmDBConnect)
        {
            _dbContext = atmDBConnect;
        }
        public async Task createDatabase(string dataBase, string sqlQuery)
        {
            SqlConnection DbConnection = await _dbContext.OpenConnection();

            using (SqlCommand command = new SqlCommand(sqlQuery, DbConnection))
            {
                int Result = command.ExecuteNonQuery();
                string Message = $"{dataBase} was created successfully.";
                //Console.WriteLine(Message);
            }
        }
        public async Task createTable(string tableName, string sqlQuery)
        {
            SqlConnection DbConnection = await _dbContext.OpenConnection();

            using (SqlCommand command = new SqlCommand(sqlQuery, DbConnection))
            {
                int Result = await command.ExecuteNonQueryAsync();
                string Message = $"{tableName} was created successfully.";
                //onsole.WriteLine(Message);
            }
        }

        public async Task createUsers(string sqlQuery)
        {
            SqlConnection DbConnection = await _dbContext.OpenConnection();

            using (SqlCommand command = new SqlCommand(sqlQuery, DbConnection))
            {
                int Result = await command.ExecuteNonQueryAsync();
                string Message = $"Data was created successfully.";
              //  Console.WriteLine(Message);
            }
        }

        public async Task<bool> checkIfEmpty(string sqlQuery)
        {
            SqlConnection DbConnection = await _dbContext.OpenConnection();

            using (SqlCommand command = new SqlCommand(sqlQuery, DbConnection))
            {
                int Result = (int)await command.ExecuteScalarAsync();

                if (Result > 0)
                {
                    return false;
                }
                return true;
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
                _dbContext.Dispose();
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
