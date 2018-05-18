namespace ExampleBrowser.View
{
	using Caliburn.Micro;
	using System;
	using Zenseless.Base;

	class ExamplePageViewModel : Screen
	{
		private readonly Lazy<IExample> example;
		private readonly GameTime time = new GameTime();
		private bool _hasProperties = false;

		public ExamplePageViewModel(Lazy<IExample, IExampleMetaData> example)
		{
			this.example = example;
		}

		public object Example => example.IsValueCreated ? example.Value : null;

		public bool HasProperties
		{
			get => _hasProperties;
			private set
			{
				_hasProperties = value;
				NotifyOfPropertyChange();
			}
		}

		protected override void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			var instance = example.Value;
			var type = instance.GetType();
			var props = type.GetProperties();
			HasProperties = (props.Length > 0);
			NotifyOfPropertyChange(nameof(Example));
		}

		public void GlRender()
		{
			var instance = example.Value;
			time.NewFrame();
			instance.Update(time); //TODO: relative time is "paused" when switching, but absolute time could cause problems
			instance.Render();
		}

	}
}
