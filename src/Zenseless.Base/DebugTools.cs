using System.Runtime.CompilerServices;

namespace Zenseless.Base
{
	/// <summary>
	/// 
	/// </summary>
	public static class DebugTools
	{
		/// <summary>
		/// Returns the full path of the source file that contains the caller. This is the file path at the time of compile.
		/// </summary>
		/// <param name="doNotAssignCallerFilePath">Dummy default parameter. Needed for internal attribute evaluation. Do not assign.</param>
		/// <param name="lineNumber"></param>
		/// <returns></returns>
		public static string GetSourcePositionForConsoleRef([CallerFilePath] string doNotAssignCallerFilePath = "",
			[CallerLineNumberAttribute] int lineNumber = 0)
		{
			return $"{doNotAssignCallerFilePath}({lineNumber},1,1,1)";
		}
	}
}
