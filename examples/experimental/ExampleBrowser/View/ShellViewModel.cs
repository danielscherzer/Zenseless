namespace ExampleBrowser.View
{
	using Caliburn.Micro;
	using ExampleBrowser.Services;
	using System;
	using System.ComponentModel.Composition;

	[Export]
	class ShellViewModel : Conductor<IScreen>.Collection.OneActive
	{
		[ImportingConstructor]
		public ShellViewModel([ImportMany] Lazy<IExample, IExampleMetaData>[] examples, [Import] Time time) //lazy loading because of example constructors with OpenGL code
		{
			DisplayName = "Example Browser";

			foreach (var example in examples)
			{
				var exampleVM = new ExamplePageViewModel(example)
				{
					DisplayName = example.Metadata.Name
				};
				PropertyChanged += (s, a) =>
				{
					if (a.PropertyName == nameof(IsRunning))
					{
						exampleVM.IsRenderLoopActivated = IsRunning;
					}
				};
				Items.Add(exampleVM);
			}

			this.time = time;
		}

		public void Invalidate() { }

		public bool IsRunning
		{
			get => time.IsRunning;
			set
			{
				time.IsRunning = value;
				NotifyOfPropertyChange();
			}
		}

		private readonly Time time;
	}
}
