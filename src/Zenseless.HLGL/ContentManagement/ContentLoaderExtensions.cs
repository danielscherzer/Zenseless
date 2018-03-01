using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Zenseless.Base;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Extension methods for content loader classes
	/// </summary>
	public static class ContentLoaderExtensions
	{

		/// <summary>
		/// Gets the full name for a given shortName.
		/// </summary>
		/// <param name="fullNames">The reference list of full names</param>
		/// <param name="shortName">The short name.</param>
		/// <returns>The full name.</returns>
		public static string GetFullName(this IEnumerable<string> fullNames, string shortName)
		{
			if (fullNames == null) throw new ArgumentNullException(nameof(fullNames));
			if (shortName == null) throw new ArgumentNullException(nameof(shortName));
			
			return fullNames.First((name) => name.ToLowerInvariant().Contains(shortName.ToLowerInvariant()));
		}

		/// <summary>
		/// Gets the full names for a list of given shortNames.
		/// </summary>
		/// <param name="fullNames">The reference list of full names</param>
		/// <param name="shortNames">The list of short names.</param>
		/// <returns>The list of full names.</returns>
		public static IEnumerable<string> GetFullNames(this IEnumerable<string> fullNames, IEnumerable<string> shortNames)
		{
			if (fullNames == null) throw new ArgumentNullException(nameof(fullNames));
			if (shortNames == null) throw new ArgumentNullException(nameof(shortNames));
			
			return from name in shortNames select fullNames.GetFullName(name);
		}

		/// <summary>
		/// Creates an instance of a given type from the resource with the specified name.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="contentLoader">A content loader instance.</param>
		/// <param name="name">The name of the resource to load from.</param>
		/// <returns>
		/// An instance of the given type.
		/// </returns>
		public static TYPE Load<TYPE>(this IContentLoader contentLoader, string name) where TYPE : class
		{
			var names = new List<string>();
			if (ContainsWildCard(name))
			{
				var regex = WildCardToRegular(name);
				foreach (var res in contentLoader.Names)
				{
					if (Regex.IsMatch(res, regex))
					{
						names.Add(res);
					}
				}
				if (0 == names.Count) throw new ArgumentException($"Wildcard expansion of '{name}' yielded no resources!");
			}
			else
			{
				if (null == contentLoader.Names.GetFullName(name)) throw new ArgumentException($"Content '{name}' was not found!");
				names.Add(name);
			}
			return contentLoader.Load<TYPE>(names);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentLoader"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static IShaderProgram LoadPixelShader(this IContentLoader contentLoader, string name)
		{
			if (contentLoader == null) throw new ArgumentNullException(nameof(contentLoader));
			
			var names = new string[] { "screenQuad.vert", name };
			return contentLoader.Load<IShaderProgram>(names);
		}

		/// <summary>
		/// Find files in the search directory that match the names, in resource name form. 
		/// This feature is disabled otherwise. The execution time of this command is dependent on the file count inside the given directory.
		/// </summary>
		/// <param name="names">The file names to find.</param>
		/// <param name="searchDirectory">The search directory. Files are found in this directory or subdirectories</param>
		public static IEnumerable<KeyValuePair<string, string>> FindFiles(this IEnumerable<string> names, string searchDirectory)
		{
			var files = Directory.EnumerateFiles(searchDirectory, "*.*", SearchOption.AllDirectories);
			var relFileNames = from file in files select new Tuple<string, string>(FormResourceNameFromFileName(searchDirectory, file), file);
			return from res in names
				   from file in relFileNames
					where res.ToLowerInvariant().Contains(file.Item1)
					select new KeyValuePair<string, string>(res, file.Item2);
		}

		private static bool ContainsWildCard(string name) => name.Contains("*");

		private static string FormResourceNameFromFileName(string referencePath, string inputPath) => PathTools.GetRelativePath(referencePath, inputPath).Substring(2).Replace(Path.DirectorySeparatorChar, '.').ToLowerInvariant();

		private static string WildCardToRegular(string value) => Regex.Escape(value).Replace("\\*", ".*");
	}
}
