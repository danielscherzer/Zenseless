using Zenseless.ExampleFramework;

namespace ZenselessAppEmpty
{
	class Program
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow();
			var view = new View();
			window.Render += () => view.Draw();
			window.Run();
		}
	}
}
