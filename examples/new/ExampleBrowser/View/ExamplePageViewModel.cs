using Caliburn.Micro;
using System;

namespace ExampleBrowser.View
{
	class ExamplePageViewModel : Screen
	{
		private Lazy<IExample> example;

		public ExamplePageViewModel(Lazy<IExample> example)
		{
			this.example = example;
		}

		public void GlRender()
		{
			example.Value.Render();
		}

	}
}
