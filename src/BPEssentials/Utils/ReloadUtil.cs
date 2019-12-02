namespace BPEssentials.Utils
{
    public static class ReloadUtil
    {
        public static void ReloadFiles()
        {
            Core.Instance.ReadConfigurationFiles();
            Core.Instance.RegisterCustomCommands();
        }
    }
}
