using ATM.DAL;
using System;
using System.Threading.Tasks;

namespace ATM.BL
{
    public class ATMClient
    {


        public async static Task Run()
        {


            using (IAtmServices aTMServices = new ATMServices(new AtmDBConnect()))
            {
                // await aTMServices.deposit(1, 850);
                await aTMServices.withdraw(1, 10850);
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
