using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Implementation of a content manager for embedded resources
	/// </summary>
	/// <seealso cref="IContentManager" />
	public class ContentManager : IContentManager
	{
		private readonly IEnumerable<string> resourceNames;
		private readonly Assembly resourceAssembly;
		private Dictionary<Type, Func<IEnumerable<NamedResourceStream>, object>> typeConverters = new Dictionary<Type, Func<IEnumerable<NamedResourceStream>, object>>();

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentManager"/> class.
		/// </summary>
		/// <param name="resourceAssembly">The resource assembly.</param>
		public ContentManager(Assembly resourceAssembly)
		{
			this.resourceAssembly = resourceAssembly;
			resourceNames = resourceAssembly.GetManifestResourceNames();

			string StringConverter(IEnumerable<NamedResourceStream> resources)
			{
				var sb = new StringBuilder();
				foreach (var res in resources)
				{
					using (var reader = new StreamReader(res.Stream, true))
					{
						sb.Append(reader.ReadToEnd());
					}
				}
				return sb.ToString();
			}
			RegisterConverter(StringConverter);

			byte[] BufferConverter(IEnumerable<NamedResourceStream> resources)
			{
				foreach (var res in resources)
				{
					using (BinaryReader br = new BinaryReader(res.Stream))
					{
						return br.ReadBytes((int)res.Stream.Length);
					}
				}
				throw new ArgumentException("No elements");
			}
			RegisterConverter(BufferConverter);
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
			var names = new List<string>();
			if (ContainsWildCard(name))
			{
				var regex = WildCardToRegular(name);
				foreach (var res in resourceNames)
				{
					if(Regex.IsMatch(res, regex))
					{
						names.Add(res);
					}
				}
			}
			else
			{
				names.Add(name);
			}
			return Load<TYPE>(names);
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
				var result = converter.Invoke(resources) as TYPE;
				foreach (var res in resources) res.Stream.Dispose();
				return result;
			}
			throw new ArgumentException($"No converter for type '{type.FullName}' was found.");
		}

		private string GetFullName(string name) => resourceNames.FirstOrDefault((resName) => resName.ToLowerInvariant().Contains(name.ToLowerInvariant()));

		private bool ContainsWildCard(string name) => name.Contains("*");

		private static string WildCardToRegular(string value) => Regex.Escape(value).Replace("\\*", ".*");
	}
}
