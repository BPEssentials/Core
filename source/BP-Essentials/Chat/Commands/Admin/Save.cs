using System.Threading;

namespace BP_Essentials.Commands
{
    public class Save
    {
        public static void Run()
        {
            new Thread(BP_Essentials.Save.Run)
            {
                IsBackground = true
            }.Start();
        }
    }
}