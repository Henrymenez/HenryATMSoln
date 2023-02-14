using System;
using System.Collections.Generic;
using System.Text;

namespace ATM.DAL
{
    public interface IDBServices: IDisposable
    {
        void createDB();

        void createUserTable();
        void insertUserDemoData();
        void createTransactionTable();
    }
}
