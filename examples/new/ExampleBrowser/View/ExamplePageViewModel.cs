using Caliburn.Micro;
using System.Text.RegularExpressions;

namespace ExampleBrowser.View
{
	class ExamplePageViewModel : Screen
	{
		private IExample example;

		public ExamplePageViewModel(IExample example)
		{
			this.example = example;
			var name = example.GetType().Name;
			DisplayName = Regex.Replace(name, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 "); //camel case to words
		}

		public void GlRender()
		{
			example.Render();
			if(IsActive) (GetView() as ExamplePageView)?.gl.Invalidate(); //TODO: breaks mvvm principle and isRenderLoopActive does not work here?!
		}
	}
}
