using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ATM.DAL
{
    public class AtmDBConnect : IDisposable
    {
        private readonly string _connString;

        private bool _disposed;

        private SqlConnection _dbConnection = null;

        private static readonly string _connectionString = "Data Source=DESKTOP-DM3DDUO\\SQLEXPRESS;Initial Catalog=AtmDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //ESKTOP-DM3DDUO\\SQLEXPRESS";


        public AtmDBConnect() : this(@$"{_connectionString}")
        {

        }

        public AtmDBConnect(string connString)
        {
            _connString = connString;
        }

        public async Task<SqlConnection> OpenConnection()
        {
            _dbConnection =  new SqlConnection(_connString);
           await _dbConnection.OpenAsync();
            return _dbConnection;
        }

        public async Task CloseConnection()
        {
            if (_dbConnection?.State != ConnectionState.Closed)
            {
                  await  _dbConnection?.CloseAsync();
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
                _dbConnection.Dispose();
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
