namespace BPEssentials.ExtensionMethods
{
    public static class ExtensionString
    {
        public static string LimitString(this string str, int limit, string suffix = "...")
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            if (str.Length <= limit)
            {
                return str;
            }
            return str.Substring(0, limit - suffix.Length) + suffix;
        }

        public static string Stringify(this bool val, string ifTrue, string ifFalse)
        {
            return val ? ifTrue : ifFalse;
        }

        public static string CleanerMessage(this string san)
        {
            return san.Replace("<", "<<b></b>");
        }
    }
}