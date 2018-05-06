using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;

namespace ExampleBrowser.View
{
	[Export]
	class ShellViewModel : Conductor<IScreen>.Collection.OneActive
	{
		[ImportingConstructor]
		public ShellViewModel([ImportMany] Lazy<IExample, IExampleMetaData>[] examples) //lazy loading because of example constructors with OpenGL code
		{
			DisplayName = "Example Browser";
			foreach (var example in examples)
			{
				var exampleVM = new ExamplePageViewModel(example)
				{
					DisplayName = example.Metadata.Name
				};
				Items.Add(exampleVM);
			}
		}
	}
}
