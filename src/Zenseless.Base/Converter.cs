using System.Text;

namespace Zenseless.Base
{
	/// <summary>
	/// Contains data type converter methods
	/// </summary>
	public static class Converter
	{
		/// <summary>
		/// Converts a given byte array assuming UTF8 encoding into a string
		/// </summary>
		/// <param name="input">Byte array in UTF8 encoding</param>
		/// <returns>String of the byte array</returns>
		public static string BytesToString(byte[] input)
		{
			return Encoding.UTF8.GetString(input);
		}
	}
}
