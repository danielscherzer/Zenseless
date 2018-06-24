namespace Zenseless.Patterns
{
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using System.Text;

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
			[CallerLineNumber] int lineNumber = 0)
		{
			return $"{doNotAssignCallerFilePath}({lineNumber},1,1,1)";
		}

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <param name="strings">The strings.</param>
		/// <param name="delimiter">The delimiter.</param>
		/// <returns>
		/// A <see cref="string" /> that represents this instance.
		/// </returns>
		public static string Combine(this IEnumerable<string> strings, string delimiter = ";")
		{
			var result = new StringBuilder();
			foreach (var name in strings)
			{
				result.Append(name);
				result.Append(delimiter);
			}
			result.Remove(result.Length - delimiter.Length, delimiter.Length);
			return result.ToString();
		}
	}
}
