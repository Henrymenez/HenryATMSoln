using ATM.DAL;
using System;
using System.Threading.Tasks;

namespace ATM.BL
{
    public static class ATMImplementation
    {


        public static void printOptions()
        {


            Console.WriteLine("Please choose from one of these following options..!");
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Show Balance");
            Console.WriteLine("4. Transfer");
            Console.WriteLine("5. Exit");


        }

        public static async Task Run()
        {

            Console.WriteLine("Welcome To Henry ATM \n");
        start: Console.WriteLine("Please Insert Your Card Number: ");
            string cardnumber = Console.ReadLine();
            Console.WriteLine(cardnumber);


            using (IAtmServices aTMServices = new ATMServices(new AtmDBConnect()))
            {
                var user = await aTMServices.CheckCardNumber(cardnumber);
                if (user.Name != null)
                {
                    Console.WriteLine("Please Insert Card Pin: ");
                    int cardpin = int.Parse(Console.ReadLine());

                    if (user.cardPin == cardpin)
                    {
                        Console.Clear();
                        printOptions();
                    }
                    else
                    {
                        Console.WriteLine("Incorrect card Pin");
                    }

                }
                else
                {
                    Console.WriteLine("incorrect card number");
                    goto start;
                }

                // await aTMServices.deposit(1, 850);
                // await aTMServices.withdraw(1, 10850);
                // await aTMServices.transfer(1, 2, 10000);
                // await aTMServices.checkBalance(1);\

                /* string str = "8698FEA1-2605-415D-A3C4-951B3A4348F1";
                Guid id = new Guid(str);
                 await aTMServices.checkStatment(id);*/
            };


            // DBServices dBServices = new DBServices();
            // dBServices.createDB();
            //dBServices.createUserTable();
            // dBServices.insertUserDemoData();
            //dBServices.createTransactionTable();
        }
    }
}
