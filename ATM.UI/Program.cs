

using ATM.BL;

namespace ATM.UI
{
    public class Program
    {

        static async Task Main(string[] args)
        {

           await ATMClient.Run();
        }
    }
}