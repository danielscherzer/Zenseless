using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Implementation of a content manager
	/// </summary>
	/// <seealso cref="IContentManager" />
	public class ContentManager : IContentManager
	{
		private readonly IEnumerable<string> resourceNames;
		private readonly Assembly resourceAssembly;
		private Dictionary<string, Func<NamedResourceStream, object>> converters = new Dictionary<string, Func<NamedResourceStream, object>>();
		private Dictionary<Type, Func<IEnumerable<NamedResourceStream>, object>> typeConverters = new Dictionary<Type, Func<IEnumerable<NamedResourceStream>, object>>();

		//public IEnumerable<string> ResourceNames => resourceNames;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentManager"/> class.
		/// </summary>
		/// <param name="resourceAssembly">The resource assembly.</param>
		public ContentManager(Assembly resourceAssembly)
		{
			this.resourceAssembly = resourceAssembly;
			resourceNames = resourceAssembly.GetManifestResourceNames();

			string TextConverter(NamedResourceStream res)
			{
				using (var reader = new StreamReader(res.Stream, true))
				{
					return reader.ReadToEnd();
				}
			}
			RegisterConverter(".txt", TextConverter);
		}

		/// <summary>
		/// Registers a resource converter. Responsible for converting one type of resource into an object instance
		/// </summary>
		/// <typeparam name="TYPE">The type of the instance that will be created by the converter.</typeparam>
		/// <param name="nameExtension">The resource name extension this converter is responsible for.</param>
		/// <param name="converter">The converter function.</param>
		/// <exception cref="ArgumentException"></exception>
		public void RegisterConverter<TYPE>(string nameExtension, Func<NamedResourceStream, TYPE> converter) where TYPE : class
		{
			if(converter is null) throw new ArgumentException($"The converter must not be null.");
			converters.Add(nameExtension, converter);
		}

		/// <summary>
		/// Registers the converter.
		/// </summary>
		/// <typeparam name="TYPE">The type of the ype.</typeparam>
		/// <param name="converter">The converter.</param>
		/// <exception cref="ArgumentException"></exception>
		public void RegisterConverter<TYPE>(Func<IEnumerable<NamedResourceStream>, TYPE> converter) where TYPE : class
		{
			if (converter is null) throw new ArgumentException($"The converter must not be null.");
			typeConverters.Add(typeof(TYPE), converter);
		}
		
		/// <summary>
		/// Creates an instance of a given type from the resource with the specified name.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="name">The name of the resource to load from.</param>
		/// <returns>
		/// An instance of the given type.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// </exception>
		public TYPE Load<TYPE>(string name) where TYPE : class
		{
			var fullName = GetFullName(name);
			if (fullName is null) throw new ArgumentException($"The embedded resource '{name}' was not found.");
			using (var stream = resourceAssembly.GetManifestResourceStream(fullName))
			{
				var extension = Path.GetExtension(fullName).ToLowerInvariant();
				if (converters.TryGetValue(extension, out var converter))
				{
					var output = converter.Invoke(new NamedResourceStream(fullName, stream)) as TYPE;
					if (output is null) throw new ArgumentException($"Converter for resource type '{extension}' does not convert to '{typeof(TYPE).FullName}'.");
					return output;
				}
				throw new ArgumentException($"No converter for resource type '{extension}' was found.");
			}
		}

		/// <summary>
		/// Creates an instance of a given type from the resources with the specified names.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="names">A list of resource names.</param>
		/// <returns>
		/// An instance of the given type.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// </exception>
		public TYPE Load<TYPE>(IEnumerable<string> names) where TYPE : class
		{
			var resources = new List<NamedResourceStream>();
			foreach (var name in names)
			{ 
				var fullName = GetFullName(name);
				if (fullName is null) throw new ArgumentException($"The embedded resource '{name}' was not found.");
				resources.Add(new NamedResourceStream(fullName, resourceAssembly.GetManifestResourceStream(fullName)));
			}
			var type = typeof(TYPE);
			if (typeConverters.TryGetValue(type, out var converter))
			{
				return converter.Invoke(resources) as TYPE;
			}
			throw new ArgumentException($"No converter for type '{type.FullName}' was found.");
		}

		private string GetFullName(string name)
		{
			return resourceNames.FirstOrDefault((resName) => resName.ToLowerInvariant().Contains(name.ToLowerInvariant()));
		}

		private bool ContainsWildCard(string name)
		{
			return name.Contains("*");
		}

		private static string WildCardToRegular(string value)
		{
			return "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
		}
	}
}
