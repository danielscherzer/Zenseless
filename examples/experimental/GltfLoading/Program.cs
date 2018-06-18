using Zenseless.ExampleFramework;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow(debug:true);
			var view = new View(window.ContentLoader);
			window.Render += () => view.Draw();
			window.Run();

		}
	}
}
