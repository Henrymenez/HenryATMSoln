using ATM.DAL.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DAL
{
    public  interface IAtmServices : IDisposable
    {
        
        Task deposit(int id, decimal amount);
        Task withdraw(int id, decimal amount);
        Task transfer(int sender, int receiver, decimal amount);

        Task checkBalance(int sender);
        Task<userViewModel> CheckCardNumber(string cardNumber);
        // Task checkStatment(Guid id);
    }
}
