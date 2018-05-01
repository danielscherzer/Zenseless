using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace ExampleBrowser.View
{
	[Export]
	class ShellViewModel : Conductor<IScreen>.Collection.OneActive
	{
		[ImportingConstructor]
		public ShellViewModel([ImportMany] IExample[] examples)
		{
			DisplayName = "Example Browser";

			foreach (var example in examples)
			{
				ActivateItem(new ExamplePageViewModel(example));
			}
		}
	}
}
