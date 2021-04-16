using System;
using System.Globalization;


namespace BPEssentials.Utils.Formatter.Response
{

    public class CustomFormatter : IFormatProvider, ICustomFormatter
    {
        public string ArgColor { get; set; }

        public string InfoColor { get; set; }

        public CustomFormatter(string argColor, string infoColor)
        {
            ArgColor = argColor;
            InfoColor = infoColor;
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }


            var arg_IFormattable = arg as IFormattable;
            if (arg_IFormattable != null)
            {
                return $"</color><color={ArgColor}>" + (arg_IFormattable).ToString(format, CultureInfo.CurrentCulture) + $"</color><color={InfoColor}>";
            }

            return $"</color><color={ArgColor}>{arg.ToString()}</color><color={InfoColor}>";
        }
    }
}
