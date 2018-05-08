using Caliburn.Micro;
using System;
using Zenseless.Base;

namespace ExampleBrowser.View
{
	class ExamplePageViewModel : Screen
	{
		private readonly Lazy<IExample> example;
		private readonly ITime time;

		public ExamplePageViewModel(Lazy<IExample, IExampleMetaData> example, ITime time)
		{
			this.example = example;
			this.time = time;
		}

		public void GlRender()
		{
			var instance = example.Value;
			instance.Update(time); //TODO: relative time is "paused" when switching, but absolute time could cause problems
			instance.Render();
		}

	}
}
