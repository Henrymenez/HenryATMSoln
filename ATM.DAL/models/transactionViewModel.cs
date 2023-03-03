using System;

namespace ATM.DAL.models
{
    public class transactionViewModel
    {
        public int userId { get; set; }
        public string? ReceiverId { get; set; }

        public string TransactionType { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
