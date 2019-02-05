using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using static BP_Essentials.Variables;

namespace BP_Essentials
{
	public static class PlaceholderParser
	{
		public static string ParseString(string str)
		{
				var src = DateTime.Now;
				var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second);
				var minutes = hm.ToString("mm");
				var seconds = hm.ToString("ss");
			    return str
				          .Replace("{YYYY}", hm.ToString("yyyy"))
						  .Replace("{DD}", hm.ToString("dd"))
						  .Replace("{DDDD}", hm.ToString("dddd"))
						  .Replace("{MMMM}", hm.ToString("MMMM"))
						  .Replace("{MM}", hm.ToString("MM"))
						  .Replace("{H}", hm.ToString("HH"))
						  .Replace("{h}", hm.ToString("hh"))
						  .Replace("{M}", minutes)
						  .Replace("{S}", seconds)
						  .Replace("{T}", hm.ToString("t"));
		}
		public static string ParseTimeStamp() => ParseTimeStamp(TimestampFormat);
		public static string ParseTimeStamp(string Timestamp)
		{
			try
			{
				return ParseString(Timestamp);
			}
			catch (Exception)
			{
				return "[Failed] ";
			}
		}
		public static string ParseUserMessage(ShPlayer shplayer, string message, string playerMessage)
		{
			try
			{
				var src = DateTime.Now;
				var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second);
				var minutes = hm.ToString("mm");
				var seconds = hm.ToString("ss");
				// Improve this mess
				return message
					.Replace("{YYYY}", hm.ToString("yyyy"))
					.Replace("{DD}", hm.ToString("dd"))
					.Replace("{DDDD}", hm.ToString("dddd"))
					.Replace("{MMMM}", hm.ToString("MMMM"))
					.Replace("{MM}", hm.ToString("MM"))
					.Replace("{H}", hm.ToString("HH"))
					.Replace("{h}", hm.ToString("hh"))
					.Replace("{M}", minutes)
					.Replace("{S}", seconds)
					.Replace("{T}", hm.ToString("tt"))
					.Replace("{username}", new Regex("(<)").Replace(shplayer.username, "<<b></b>"))
					.Replace("{id}", $"{shplayer.ID}")
					.Replace("{jobname}", Jobs[shplayer.job.jobIndex])
					.Replace("{jobnameofficial}", shplayer.job.info.jobName)
					.Replace("{jobindex}", $"{shplayer.job.jobIndex}")
					.Replace("{jobcolor}", $"#{ColorUtility.ToHtmlStringRGB(shplayer.job.info.jobColor)}")
					.Replace("{discordlink}", MsgDiscord)
					.Replace("{infocolor}", infoColor)
					.Replace("{warningcolor}", warningColor)
					.Replace("{errorcolor}", errorColor)
					.Replace("{argcolor}", argColor)
					.Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(playerMessage), "<<b></b>"));
			}
			catch (Exception ex)
			{
				ErrorLogging.Run(ex);
			}
			return null;
		}
	}
}
