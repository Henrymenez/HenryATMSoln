using ATM.DAL;
using System.Threading.Tasks;

namespace ATM.BL
{
    public class ATMClient
    {


        public async static Task Run()
        {


            using (IAtmServices aTMServices = new ATMServices(new AtmDBConnect()))
            {
                //  await aTMServices.deposit(1, 850);
                // await aTMServices.withdraw(1, 1850);
               await aTMServices.transfer(1, 2, 10000);
            };

            
           

            // DBServices dBServices = new DBServices();
            // dBServices.createDB();
            //dBServices.createUserTable();
            // dBServices.insertUserDemoData();
            //dBServices.createTransactionTable();
        }
    }
}
