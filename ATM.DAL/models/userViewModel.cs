using System;
using System.Collections.Generic;
using System.Text;

namespace ATM.DAL.models
{
    public class userViewModel
    {
        public string Name { get; set; }
        public int userId { get; set; }
        public string cardnumber { get; set; }
        public string cardPin { get; set; }
        public decimal balance { get; set; }
        public bool? status { get; set; }

    }
}
