namespace ExampleBrowser
{
	using Zenseless.Patterns;

	public class Example : NotifyPropertyChanged, IExample
	{
		public virtual void Render()
		{
		}

		public virtual void Update()
		{
		}

		public virtual void Resize(int width, int height)
		{

		}
	}
}