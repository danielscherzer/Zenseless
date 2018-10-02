using System;
using System.Collections.Generic;
using System.Text;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public class ShaderLogLine
	{
		/// <summary>
		/// The string type for the well known type warning
		/// </summary>
		public const string WellKnownTypeWarning = "WARNING";
		/// <summary>
		/// The string type for the well known type error
		/// </summary>
		public const string WellKnownTypeError = "ERROR";
		/// <summary>
		/// The string type for the well known type information
		/// </summary>
		public const string WellKnownTypeInfo = "INFO";
		/// <summary>
		/// The type
		/// </summary>
		public string Type = string.Empty;
		/// <summary>
		/// The file number
		/// </summary>
		public int FileNumber = -1;
		/// <summary>
		/// The line number
		/// </summary>
		public int LineNumber = -1;
		/// <summary>
		/// The message
		/// </summary>
		public string Message = string.Empty;
		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			if (string.Empty != Type)
			{
				sb.Append(Type + ": ");
			}
			if (-1 != LineNumber)
			{
				sb.Append("Line ");
				sb.Append(LineNumber.ToString());
				sb.Append(" : ");
			}
			sb.Append(Message);
			return sb.ToString();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class ShaderLog
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderLog"/> class.
		/// </summary>
		/// <param name="log">The log.</param>
		public ShaderLog(string log)
		{
			//parse error log
			char[] splitChars = new char[] { '\n' };
			var errorLines = new List<ShaderLogLine>();
			var otherLines = new List<ShaderLogLine>();
			foreach (var line in log.Split(splitChars, StringSplitOptions.RemoveEmptyEntries))
			{
				ShaderLogLine logLine;
				try
				{
					logLine = ParseLogLineNVIDIA(line);
				}
				catch
				{
					logLine = ParseLogLine(line);
				}
				if (logLine.Type.StartsWith(ShaderLogLine.WellKnownTypeError))
				{
					errorLines.Add(logLine);
				}
				else
				{
					otherLines.Add(logLine);
				}
			}
			//first error messages, then all others
			lines = errorLines;
			lines.AddRange(otherLines);
		}

		/// <summary>
		/// Gets the lines.
		/// </summary>
		/// <value>
		/// The lines.
		/// </value>
		public IEnumerable<ShaderLogLine> Lines { get { return lines; } }

		/// <summary>
		/// Parses the log line.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		private ShaderLogLine ParseLogLine(string line)
		{
			ShaderLogLine logLine = new ShaderLogLine();
			char[] splitChars = new char[] { ':' };
			var elements = line.Split(splitChars, 4);
			switch(elements.Length)
			{
				case 4:
					logLine.Type = ParseType(elements[0]);
					logLine.FileNumber = Parse(elements[1]);
					logLine.LineNumber = Parse(elements[2]);
					logLine.Message = elements[3];
					break;
				case 3:
					logLine.Type = ParseType(elements[0]);
					logLine.Message = elements[1] + ":" + elements[2];
					break;
				case 2:
					logLine.Type = ParseType(elements[0]);
					logLine.Message = elements[1];
					break;
				case 1:
					logLine.Message = elements[0];
					break;
				default:
					throw new ArgumentException(line);
			}
			logLine.Message = logLine.Message.Trim();
			return logLine;
		}

		/// <summary>
		/// Parses the log line nvidia.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		private ShaderLogLine ParseLogLineNVIDIA(string line)
		{
			ShaderLogLine logLine = new ShaderLogLine();
			char[] splitChars = new char[] { ':' };
			var elements = line.Split(splitChars, 3);
			switch (elements.Length)
			{
				case 3:
					logLine.FileNumber = ParseNVFileNumber(elements[0]);
					logLine.LineNumber = ParseNVLineNumber(elements[0]);
					logLine.Type = ParseNVType(elements[1]);
					logLine.Message = elements[1] + ":" + elements[2];
					break;
				default:
					throw new ArgumentException(line);
			}
			logLine.Message = logLine.Message.Trim();
			return logLine;
		}

		/// <summary>
		/// Parses the type of the nv.
		/// </summary>
		/// <param name="v">The v.</param>
		/// <returns></returns>
		private string ParseNVType(string v)
		{
			char[] splitChars = new char[] { ' ', '\t' };
			var elements = v.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
			return ParseType(elements[0]);
		}

		/// <summary>
		/// Parses the nv line number.
		/// </summary>
		/// <param name="v">The v.</param>
		/// <returns></returns>
		private int ParseNVLineNumber(string v)
		{
			char[] splitChars = new char[] { '(',')', ' ', '\t' };
			var elements = v.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
			return Parse(elements[1]);
		}

		/// <summary>
		/// Parses the nv file number.
		/// </summary>
		/// <param name="v">The v.</param>
		/// <returns></returns>
		private int ParseNVFileNumber(string v)
		{
			char[] splitChars = new char[] { '(', ')', ' ', '\t' };
			var elements = v.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
			return Parse(elements[0]);
		}

		/// <summary>
		/// Parses the type.
		/// </summary>
		/// <param name="typeString">The type string.</param>
		/// <returns></returns>
		private string ParseType(string typeString)
		{
			return typeString.ToUpperInvariant().Trim();
		}

		/// <summary>
		/// Parses the specified number.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <returns></returns>
		private int Parse(string number)
		{
			if (int.TryParse(number, out int output))
			{
				return output;
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// The lines
		/// </summary>
		private List<ShaderLogLine> lines = new List<ShaderLogLine>();
	}
}
