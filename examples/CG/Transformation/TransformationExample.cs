namespace Example
{
	using System;
	using Zenseless.ExampleFramework;

	class Program
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var logic = new Model();
			var visual = new MyVisual();
			window.Update += logic.Update;
			window.Render += () => visual.Render(logic.Shapes);
			window.Resize += visual.Resize;
			window.Run();

		}
	}
}
