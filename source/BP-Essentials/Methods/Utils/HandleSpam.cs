using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static BP_Essentials.Variables;

namespace BP_Essentials
{
    class HandleSpam
    {
        public static bool Run(SvPlayer player, string message)
        {
            if (MessagesAllowedPerSecond != -1 && MessagesAllowedPerSecond < 50)
            {
                if (PlayerList.TryGetValue(player.player.ID, out var currObj))
                {
                    if (currObj.MessagesSent >= MessagesAllowedPerSecond)
                    {
                        Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [WARNING] {player.player.username} got kicked for spamming! {currObj.MessagesSent}/s (max: {MessagesAllowedPerSecond}) messages sent.");
                        player.svManager.Kick(player.connection);
                        return true;
                    }
                    PlayerList[player.player.ID].MessagesSent++;
                    if (!currObj.IsCurrentlyAwaiting)
                    {
                        PlayerList[player.player.ID].IsCurrentlyAwaiting = true;
                        Task.Factory.StartNew(async () =>
                        {
                            await Task.Delay(TimeBetweenDelay);
                            if (PlayerList.ContainsKey(player.player.ID))
                            {
                                PlayerList[player.player.ID].MessagesSent = 0;
                                PlayerList[player.player.ID].IsCurrentlyAwaiting = false;
                            }
                        });
                    }
                }
            }
            return false;
        }
    }
}
