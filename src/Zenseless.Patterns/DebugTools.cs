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
		/// Returns a <see cref="string" /> that is the concatenation of the input strings.
		/// </summary>
		/// <param name="inputStrings">The input strings.</param>
		/// <param name="delimiter">The delimiter (can be <seealso cref="string.Empty"/> if no delimiter is wanted)</param>
		/// <returns>
		/// A concatenated <see cref="string" /> that represents this instance.
		/// </returns>
		public static string Combine(this IEnumerable<string> inputStrings, string delimiter)
		{
			var result = new StringBuilder();
			foreach (var name in inputStrings)
			{
				result.Append(name);
				result.Append(delimiter);
			}
			if (delimiter.Length > 0)
			{
				result.Remove(result.Length - delimiter.Length, delimiter.Length);
			}
			return result.ToString();
		}
	}
}
