namespace BPEssentials.Modules
{
    public abstract class Module
    {
        public abstract ModuleInfo ModuleInfo { get; set; }

        public virtual ModuleInfo GetModuleInfo()
        {
            return ModuleInfo;
        }

        public virtual void OnLoad()
        {
        }

        public virtual void OnUnload()
        {
        }

        public string GetName()
        {
            return ModuleInfo?.Plugin?.Info.Name;
        }

        public string GetDescription()
        {
            return ModuleInfo?.Plugin?.Info.Description;
        }
    }
}
