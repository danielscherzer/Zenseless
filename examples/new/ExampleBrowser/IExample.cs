using Zenseless.Base;

namespace ExampleBrowser
{
	public interface IExample
	{
		void Update(ITime time);
		void Render();
	}
}