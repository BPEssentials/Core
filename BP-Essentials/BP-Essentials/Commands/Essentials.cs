namespace BP_Essentials.Commands {
    public class Essentials : EssentialsCorePlugin {
        public static bool Run(object oPlayer, string message) {
                var player = (SvPlayer)oPlayer;
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Essentials Created by UserR00T & DeathByKorea & BP");
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Version: " + EssentialsVariablesPlugin.Version);
            return true;

        }
    }
}