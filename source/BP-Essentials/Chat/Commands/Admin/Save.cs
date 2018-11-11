using System.Threading;

namespace BP_Essentials.Commands
{
    public class Save
    {
        public static void Run(SvPlayer player, string message)
        {
            new Thread(BP_Essentials.Save.Run)
            {
                IsBackground = true
            }.Start();
        }
    }
}