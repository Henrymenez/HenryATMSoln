using ATM.DAL;
using System;
using System.Threading.Tasks;

namespace ATM.BL
{
    public class createAndUpdateDB
    {
        private readonly static createDB create = new createDB(new AtmDBConnect());


        public static async Task Start()
        {
            try
            {

                await create.createTable("Atminfo", createQueryStrings.atmTableString);
                await create.createTable("Users", createQueryStrings.UserTableString);
                await create.createTable("Transactions", createQueryStrings.transactionTableString);
                bool resut = await create.checkIfEmpty(createQueryStrings.checkIfEmpty);
                if (resut)
                {
                    await create.createUsers(createQueryStrings.createUserSql);

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

            }
        }

    }
}
