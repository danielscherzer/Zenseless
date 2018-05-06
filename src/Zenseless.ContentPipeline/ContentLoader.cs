namespace Zenseless.ExampleFramework
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Zenseless.Base;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	internal class ContentLoader : IBeforeRendering, IContentLoader
	{
		private readonly FileContentManager contentManager;

		public ContentLoader()
		{
			var assembly = Assembly.GetEntryAssembly();
			//check if entry assembly was built with SOLUTION attribute
			var solutionMode = !(assembly.GetCustomAttribute<SolutionAttribute>() is null);

			contentManager = ContentManagerGL.Create(assembly, solutionMode);

			var contentDir = assembly.GetCustomAttribute<ContentSearchDirectoryAttribute>()?.ContentSearchDirectory;
			contentManager.SetContentSearchDirectory(contentDir);

		}

		public IEnumerable<Type> ImporterTypes => contentManager.ImporterTypes;

		public IEnumerable<string> Names => contentManager.Names;

		public void BeforeRendering()
		{
			try
			{
				contentManager.CheckForResourceChange();
			}
			catch (ShaderException e)
			{
				e.Data[ShaderLoader.ExceptionDataFileName] = contentManager.LastChangedFilePath; //TODO: make cleaner after removal of old stuff
				new FormShaderExceptionFacade().ShowModal(e);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		public TYPE Load<TYPE>(IEnumerable<string> names) where TYPE : class
		{
			return contentManager.Load<TYPE>(names);
		}
	}
}