using System;
using System.Diagnostics;

namespace Zenseless.ExampleFramework
{
	internal class ConsoleDebugListener : TraceListener
	{
		public ConsoleDebugListener()
		{
		}

		public override void Write(string message)
		{
			Console.Write(message);
		}

		public override void WriteLine(string message)
		{
			Console.WriteLine(message);
		}
	}
}