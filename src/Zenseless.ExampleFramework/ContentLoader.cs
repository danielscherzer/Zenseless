namespace Zenseless.ExampleFramework
{
	using GLSLhelper;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Reflection;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	/// <summary>
	/// A content loader for OpenGL resources
	/// </summary>
	/// <seealso cref="IBeforeRendering" />
	/// <seealso cref="IContentLoader" />
	public class ContentLoader : IBeforeRendering, IContentLoader
	{
		private readonly FileContentManager contentManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentLoader"/> class.
		/// </summary>
		public ContentLoader()
		{
			var assembly = Assembly.GetEntryAssembly();
			//check if entry assembly was built with SOLUTION attribute
			var solutionMode = !(assembly.GetCustomAttribute<SolutionAttribute>() is null);

			contentManager = ContentManagerGL.Create(assembly, solutionMode);

			var contentDir = assembly.GetCustomAttribute<ContentSearchDirectoryAttribute>()?.ContentSearchDirectory;
			contentManager.SetContentSearchDirectory(contentDir);
		}

		/// <summary>
		/// Gets a list of registered importer types.
		/// </summary>
		/// <value>
		/// The importer types.
		/// </value>
		public IEnumerable<Type> ImporterTypes => contentManager.ImporterTypes;

		/// <summary>
		/// Enumerates all content resource names.
		/// </summary>
		/// <value>
		/// All content resource names.
		/// </value>
		public IEnumerable<string> Names => contentManager.Names;

		/// <summary>
		/// Will be called once a frame before rendering.
		/// </summary>
		public void BeforeRendering()
		{
			try
			{
				contentManager.CheckForResourceChange();
			}
			catch (NamedShaderException e)
			{
				ShowException(e.InnerException, e.Name);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
			}
		}

		private void ShowException(ShaderException e, string resourceName)
		{
			var name = contentManager.GetFilePath(resourceName);
			name = string.IsNullOrEmpty(name) ? resourceName : name;

			var log = new ShaderLogParser(e.Message);
			foreach (var logLine in log.Lines)
			{
				var msg = name + "(" + logLine.LineNumber + "): " + logLine.Message;
				Debug.Print(msg);
			}

			//new FormShaderExceptionFacade().ShowModal(e, name);
		}

		/// <summary>
		/// Creates an instance of a given type from the resources with the specified names.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="names">A list of resource names.</param>
		/// <returns>
		/// An instance of the given type.
		/// </returns>
		public TYPE Load<TYPE>(IEnumerable<string> names) where TYPE : class
		{
			try
			{
				var instance = contentManager.Load<TYPE>(names);
				Debug.WriteLine($"Load of '{string.Join(";", names)}' into {typeof(TYPE).FullName}.");
				return instance;
			}
			catch (NamedShaderException e)
			{
				ShowException(e.InnerException, e.Name);
			}
			return null;
		}
	}
}