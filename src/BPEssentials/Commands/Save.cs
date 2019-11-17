using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using Newtonsoft.Json;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Save : Command
    {
        public void Invoke(ShPlayer player)
        {
            player.TS("saving_game");
            player.manager.svManager.SaveAll();


            // TODO: Move this into the Save Event
            foreach (var cooldownPlayer in Core.Instance.Cooldowns)
            {
                Logger.Log(cooldownPlayer.Key.ToString());
                Logger.Log(JsonConvert.SerializeObject(cooldownPlayer.Value));
                var onlinePlayer = EntityCollections.Humans.FirstOrDefault(x => x.svPlayer.steamID == cooldownPlayer.Key);
                if (onlinePlayer)
                {
                    onlinePlayer.svPlayer.CustomData.AddOrUpdate("bpe:cooldowns", cooldownPlayer.Value);
                }
                else
                {
                    var newUser = Core.Instance.SvManager.Database.Users.FindSingle(x => x.ID == cooldownPlayer.Key);
                    newUser.Character.CustomData.AddOrUpdate("bpe:cooldowns", cooldownPlayer.Value);
                    Core.Instance.SvManager.Database.Users.UpdateSingle(newUser);
                }
            }
        }
    }
}
