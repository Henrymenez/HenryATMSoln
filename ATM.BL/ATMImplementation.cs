using ATM.DAL;
using System;
using System.Threading.Tasks;

namespace ATM.BL
{
    public static class ATMImplementation
    {


        public static void printOptions()
        {
            Console.WriteLine("Please choose from one of these following options..! \n" +
                "1. Deposit \n" +
                "2. Withdraw \n" +
                "3. Show Balance \n" +
                "4. Transfer \n" +
                "5. Statement of Account \n" +
                "6. Exit");
        }

        public static decimal collectAmount()
        {
            Console.Clear();
            Console.Write("Amount: ");
            decimal amount;
            bool check = decimal.TryParse(Console.ReadLine(), out amount);

            if (check && amount > 0)
            {
                return Math.Abs(amount);
            }

            return 0;
        }





        public static async Task Run()
        {
            await createAndUpdateDB.Start();
            Console.Clear();
            Console.WriteLine("\t \t Welcome To Henry ATM");
        start: Console.WriteLine("\t Please Insert Your Card Number:  \n");
            string cardnumber = Console.ReadLine();

            using (IAtmServices aTMServices = new ATMServices(new AtmDBConnect()))
            {
                while (true)
                {
                    try
                    {
                        var user = await aTMServices.CheckCardNumber(cardnumber);
                        if (user.Name != null)
                        {

                        start2: Console.WriteLine($"\t \tHello {user.Name} \n \t Please Insert Card Pin: ");


                            string pinNumber = Console.ReadLine();
                            if (user.cardPin == pinNumber)
                            {
                                Console.Clear();
                            start3: printOptions();
                                string option = Console.ReadLine();
                                decimal amount;
                                switch (option)
                                {
                                    case "1":
                                        amount = collectAmount();
                                        await aTMServices.deposit(user.userId, amount);
                                        break;
                                    case "2":

                                        amount = collectAmount();
                                        await aTMServices.withdraw(user.userId, amount);
                                        break;
                                    case "3":
                                        await aTMServices.checkBalance(user.userId);
                                        break;
                                    case "4":
                                        Console.WriteLine("Account Number you want to transfer to: ");
                                        string cardNumberTransferTo = Console.ReadLine();

                                        var transferUser = await aTMServices.CheckAccountNumber(cardNumberTransferTo);
                                        if (transferUser.Name != null)
                                        {
                                            amount = collectAmount();
                                            await aTMServices.transfer(user.userId, transferUser.userId, amount);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Incorrect Account number");
                                        }
                                        break;
                                    case "5":
                                        await aTMServices.checkStatment(user.userId);
                                        break;
                                    case "6":
                                        Console.WriteLine("Thank you and Goodbye");
                                        Environment.Exit(0);
                                        break;
                                    default:
                                        Console.Clear();
                                        goto start3;

                                }
                            }
                            else
                            {
                                Console.WriteLine("Incorrect card Pin");
                                goto start2;
                            }

                        }
                        else
                        {
                            Console.WriteLine("incorrect card number");
                            goto start;
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            };


            // DBServices dBServices = new DBServices();
            // dBServices.createDB();
            //dBServices.createUserTable();
            // dBServices.insertUserDemoData();
            //dBServices.createTransactionTable();
        }



    }
}
