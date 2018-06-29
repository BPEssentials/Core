namespace BP_Essentials
{
    class IsOnline : EssentialsChatPlugin
    {
        public static bool Run(ShPlayer player)
        {
            foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                if (shPlayer == player && !shPlayer.svPlayer.IsServerside())
                    return true;
            return false;
        }
    }
}
