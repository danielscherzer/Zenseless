using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Zenseless.Base
{
	/// <summary>
	/// Contains helper functions for file paths
	/// </summary>
	public static class PathTools
	{
		/// <summary>
		/// Returns the full path of the main module of the current process.
		/// </summary>
		/// <returns>Full path of the main module of the current process.</returns>
		public static string GetCurrentProcessPath()
		{
			return Process.GetCurrentProcess().MainModule.FileName;
		}

		/// <summary>
		/// Returns the directory of the main module of the current process.
		/// </summary>
		/// <returns>Directory of the main module of the current process.</returns>
		public static string GetCurrentProcessDir()
		{
			return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
		}

		/// <summary>
		/// Returns the output directory for the current process:
		/// a sub-directory of the directory the executable resides in 
		/// and with the name of the executable and an appended time code.
		/// </summary>
		/// <param name="timeCodeFormat">string format for DateTime</param>
		/// <returns>Output directory</returns>
		public static string GetCurrentProcessOutputDir(string timeCodeFormat = "yyyyMMdd HHmmss")
		{
			var path = GetCurrentProcessPath();
			var dir = Path.GetDirectoryName(path);
			var name = Path.GetFileNameWithoutExtension(path);
			if(!string.IsNullOrWhiteSpace(timeCodeFormat))
			{
				name += $" {DateTime.Now.ToString(timeCodeFormat)}";
			}
			return Path.Combine(dir, name);
		}

		/// <summary>
		/// Returns the absolute path for the specified path string by using <see cref="Path.GetFullPath"/>.
		/// If an exception is thrown by <see cref="Path.GetFullPath"/> the input parameter is returned.
		/// </summary>
		/// <param name="fileName">The file or directory for which to obtain absolute path information.</param>
		/// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
		public static string GetFullPath(string fileName)
		{
			try
			{
				return Path.GetFullPath(fileName);
			}
			catch
			{
				return fileName;
			}
		}

		/// <summary>
		/// Returns the relative path. If no relative path is valid, the absolute path is returned.
		/// </summary>
		/// <param name="referencePath">the path the result should be relative to</param>
		/// <param name="inputPath">the path to be converted into relative form</param>
		/// <returns></returns>
		public static string GetRelativePath(string referencePath, string inputPath)
		{
			if (string.IsNullOrEmpty(referencePath) || string.IsNullOrEmpty(inputPath)) return inputPath;
			try
			{
				int fromAttr = GetPathAttribute(referencePath);
				int toAttr = GetPathAttribute(inputPath);

				StringBuilder path = new StringBuilder(256);
				if (0 == SafeNativeMethods.PathRelativePathTo(path, referencePath, fromAttr, inputPath, toAttr))
				{
					return inputPath;
				}
				return path.ToString();
			}
			catch
			{
				return inputPath;
			}
		}

		/// <summary>
		/// Returns the full path of the source file that contains the caller. This is the file path at the time of compile.
		/// </summary>
		/// <param name="doNotAssignCallerFilePath">Dummy default parameter. Needed for internal attribute evaluation. Do not assign.</param>
		/// <returns></returns>
		public static string GetSourceFilePath([CallerFilePath] string doNotAssignCallerFilePath = "")
		{
			return doNotAssignCallerFilePath;
		}

		/// <summary>
		/// IncludeTrailingPathDelimiter ensures that a path name ends with a trailing path delimiter ('\" on Windows, '/' on Linux). 
		/// If S already ends with a trailing delimiter character, it is returned unchanged; otherwise path with appended delimiter character is returned. 
		/// </summary>
		/// <param name="path">Input path</param>
		/// <returns>Input path with trailing path delimiter</returns>
		public static string IncludeTrailingPathDelimiter(string path)
		{
			var d = Path.DirectorySeparatorChar;
			return (d != path.Last()) ? path + d : path;
		}

		private const int FILE_ATTRIBUTE_DIRECTORY = 0x10;
		private const int FILE_ATTRIBUTE_NORMAL = 0x80;

		private static int GetPathAttribute(string path)
		{
			DirectoryInfo di = new DirectoryInfo(path);
			if (di.Exists)
			{
				return FILE_ATTRIBUTE_DIRECTORY;
			}

			FileInfo fi = new FileInfo(path);
			if (fi.Exists)
			{
				return FILE_ATTRIBUTE_NORMAL;
			}

			throw new FileNotFoundException();
		}

		internal static class SafeNativeMethods
		{
			[DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
			internal static extern int PathRelativePathTo(StringBuilder pszPath,
				string pszFrom, int dwAttrFrom, string pszTo, int dwAttrTo);
		}
	}
}
