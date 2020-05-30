namespace BPEssentials.Utils
{
    public static class IO
    {
        public static void ReloadFiles()
        {
            Core.Instance.ReadConfigurationFiles();
            Core.Instance.RegisterCustomCommands();
            Core.Instance.RegisterCustomTriggers();
        }
    }
}