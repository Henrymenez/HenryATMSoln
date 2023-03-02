using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DAL
{
    public interface IDBServices: IDisposable
    {
        Task createDatabase(string dataBase, string sqlQuery);

        Task createTable(string tableName, string sqlQuery);
     
        Task createUsers( string sqlQuery);

        Task<bool> checkIfEmpty(string sqlQuery);
    }
}
