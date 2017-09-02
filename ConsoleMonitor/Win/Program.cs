using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleMonitor
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				try
				{
					MonitorOutput();

				}
				catch
				{

				}
			}
		}

		private static int lastIndex = 0;
		private static void MonitorOutput()
		{
			while (true)
			{
				if (!File.Exists(@"BrokeProtocol_Data\output_log.txt"))
					continue;

				string fileName = @"BrokeProtocol_Data\output_log.txt";
				FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				StreamReader fileReader = new StreamReader(fileStream);

				int i = 0;
				while (!fileReader.EndOfStream)
				{
					string line = fileReader.ReadLine();

					if (i > lastIndex)
					{
						if (!line.StartsWith("(") && line.Length > 2)
							Console.WriteLine(line + "\n");
						lastIndex = i;
					}

					i++;
				}

				Thread.Sleep(10);
			}
		}
	}
}
