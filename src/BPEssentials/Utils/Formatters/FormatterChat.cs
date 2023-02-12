using BPEssentials.ExtensionMethods;
using BrokeProtocol.Utility;
using System;
using System.Globalization;
using BPCoreLib.ExtensionMethods;


namespace BPEssentials.Utils.Formatter.Chat
{
    public class CustomFormatter : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            switch (format)
            {
                case "lcase":
                    return arg.ToString().ToLowerInvariant();

                case "ucase":
                    return arg.ToString().ToUpperInvariant();

                case "nospace":
                    return arg.ToString().Replace(" ", "");

                case "parsecolor":
                    return arg.ToString().CleanerMessage().ParseColorCodes();

                case "unsanitized":
                    return arg.ToString().ParseColorCodes();

                case "sanitized":
                    return arg.ToString().CleanerMessage();

                default:
                    if (arg is IFormattable formatter)
                    {
                        return formatter.ToString(format, CultureInfo.CurrentCulture).CleanerMessage();
                    }

                    break;
            }

            return arg.ToString().CleanerMessage();
        }
    }
}
