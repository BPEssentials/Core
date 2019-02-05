using System;
using static BP_Essentials.Variables;
namespace BP_Essentials
{
    class IsOnline
    {
        public static bool Run(ShPlayer player)
        {
            foreach (var currPlayer in SvMan.players.Values)
                if (currPlayer == player)
                    return true;
            return false;
        }
    }
}
