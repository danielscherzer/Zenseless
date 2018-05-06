namespace ExampleBrowser
{
	using System;
	using System.ComponentModel.Composition;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text.RegularExpressions;

	[MetadataAttribute]
	public class ExampleDisplayNameAttribute : Attribute
	{
		public ExampleDisplayNameAttribute([CallerFilePath] string name = "")
		{
			Name = ParseCamelCase(Path.GetFileNameWithoutExtension(name));
		}

		public string Name { get; set; }

		public static string ParseCamelCase(string name)
		{
			return Regex.Replace(name, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 "); //camel case to words
		}
	}
}
