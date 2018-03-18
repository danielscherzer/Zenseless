using DMS.Application;
using DMS.Base;
using System;
using System.IO;

namespace Example
{
	class Application
	{
		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			var visual = new MainVisual();
			app.GameWindow.ConnectEvents(visual.OrbitCamera);
			app.ResourceManager.ShaderChanged += visual.ShaderChanged;
			LoadResources(app.ResourceManager);
			app.Render += visual.Render;
			app.Run();
		}

		private static void LoadResources(ResourceManager resourceManager)
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			resourceManager.AddShader(MainVisual.ShaderName, dir + "vertex.vert", dir + "fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);
		}
	}
}