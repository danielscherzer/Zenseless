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
		/// <param name="contentLoader">The content loader.</param>
		/// <param name="shortName">The short name.</param>
		/// <returns>The full name.</returns>
		public static string GetFullName(this IContentLoader contentLoader, string shortName) => contentLoader.Names.FirstOrDefault((name) => name.ToLowerInvariant().Contains(shortName.ToLowerInvariant()));

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
			}
			else
			{
				names.Add(name);
			}
			return contentLoader.Load<TYPE>(names);
		}

		/// <summary>
		/// Sets the content search directory. 
		/// This is needed if you want to do automatic runtime content reloading if the content source file changes. 
		/// This feature is disabled otherwise. The execution time of this command is dependent on how many files are found inside the given directory.
		/// </summary>
		/// <param name="contentLoader"></param>
		/// <param name="contentSearchDirectory">The content search directory. Content is found in this directory or subdirectories</param>
		public static IEnumerable<KeyValuePair<string, string>> ResolveContentFiles(this IContentLoader contentLoader, string contentSearchDirectory)
		{
			var files = Directory.EnumerateFiles(contentSearchDirectory, "*.*", SearchOption.AllDirectories);
			var relFileNames = from file in files select new Tuple<string, string>(FormResourceNameFromFileName(contentSearchDirectory, file), file);
			return from res in contentLoader.Names
							   from file in relFileNames
							   where res.ToLowerInvariant().Contains(file.Item1)
							   select new KeyValuePair<string, string>(res, file.Item2);
		}

		private static bool ContainsWildCard(string name) => name.Contains("*");

		private static string FormResourceNameFromFileName(string referencePath, string inputPath) => PathTools.GetRelativePath(referencePath, inputPath).Substring(2).Replace(Path.DirectorySeparatorChar, '.').ToLowerInvariant();

		private static string WildCardToRegular(string value) => Regex.Escape(value).Replace("\\*", ".*");

		//private void CheckFileExists(string fullName)
		//{
		//	fullName = fullName.Substring(fullName.IndexOf('.'));
		//	fullName = fullName.Replace('.', Path.DirectorySeparatorChar);
		//	var fileExists = false;
		//	for(var index = fullName.LastIndexOf(Path.DirectorySeparatorChar); -1 != index; index = fullName.LastIndexOf(Path.DirectorySeparatorChar))
		//	{
		//		if (File.Exists(contentSearchDirectory + fullName))
		//		{
		//			fileExists = true;
		//			break;
		//		}
		//		fullName = fullName.Remove(index, 1).Insert(index, ".");
		//	};
		//	if(fileExists)
		//	{

		//	}
		//}
	}
	}
