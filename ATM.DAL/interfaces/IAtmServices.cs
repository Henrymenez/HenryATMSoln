using ATM.DAL.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DAL
{
    public  interface IAtmServices : IDisposable
    {
        Task deposit(Guid id, decimal amount);
        Task withdraw(Guid id, decimal amount);
        Task transfer(Guid sender, Guid receiver, decimal amount);

        Task checkBalance(Guid sender);
        Task<userViewModel> CheckCardNumber(string cardNumber);
        // Task checkStatment(Guid id);
    }
}
