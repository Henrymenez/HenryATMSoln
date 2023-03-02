using System;
using System.Collections.Generic;
using System.Text;

namespace ATM.DAL.models
{
    public class transactionViewModel
    {
        public int userId { get; set; }
        public int ReceiverId { get; set; }

        public string TransactionType { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
