using BrokeProtocol.API.ExtensionMethods;
using System;
using System.Globalization;

namespace BPEssentials.Utils
{
    public class CustomFormatter : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null) {  return string.Empty;
            }
            if (format == "lcase")
            {
                return arg.ToString().ToLower();
            }
            else if (format == "ucase")
            {
                return arg.ToString().ToUpper();
            }
            else if (format == "nospace")
            {
                return arg.ToString().Replace(" ", "");
            }
            else if (format == "parsecolor")
            {
                return arg.ToString().SanitizeString().ParseColorCodes();
            }
            else if (format == "unsanitized")
            {
                return arg.ToString().ParseColorCodes();
            }
            else if (arg is IFormattable)
            {
                return ((IFormattable)arg).ToString(format, CultureInfo.CurrentCulture).SanitizeString();
            }
            return arg.ToString().SanitizeString();
        }
    }
}
