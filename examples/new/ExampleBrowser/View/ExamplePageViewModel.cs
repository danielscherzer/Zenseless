namespace ExampleBrowser.View
{
	using Caliburn.Micro;
	using System;

	class ExamplePageViewModel : Screen
	{
		private readonly Lazy<IExample> example;
		private bool _hasProperties = false;
		private bool _isRenderLoopActivated = true;

		public ExamplePageViewModel(Lazy<IExample, IExampleMetaData> example)
		{
			this.example = example;
		}

		public object Example => example.IsValueCreated ? example.Value : null;

		public bool IsRenderLoopActivated
		{
			get => _isRenderLoopActivated; set
			{
				_isRenderLoopActivated = value;
				NotifyOfPropertyChange();
			}
		}

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
			instance.Update();
			instance.Render();
		}

	}
}
