using LevelData;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Zenseless.Patterns;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace LevelEditor
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private Level levelData = new Level(); //todo: move to save method

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string[] args = Environment.GetCommandLineArgs();
			if(args.Length > 2)
			{
				if("autosave" == args[1].ToLower())
				{
					SaveLevelData(args[2]);
					Close();
					return;
				}
			}
			var exe = this.GetType().Assembly.Location;
			SaveLevelData(System.IO.Path.GetDirectoryName(exe) + "/level.data");
		}

		private void SaveLevelData(string fileName)
		{
			//do not use auto-size for canvas -> set a fixed size
			levelData.Bounds.SizeX = (float)canvas.ActualWidth;
			levelData.Bounds.SizeY = (float)canvas.ActualHeight;
			TraverseLogicalTree(canvas, canvas, string.Empty);
			Serialization.ToBinFile(levelData, fileName);
		}

		private void TraverseLogicalTree(DependencyObject dependencyObject, Canvas canvas, string parentName) //todo: move to tools class
		{
			if (dependencyObject is null) return;
			var childern = LogicalTreeHelper.GetChildren(dependencyObject);
			foreach (var child in childern)
			{
				var type = child.GetType();
				if (typeof(Image) == type)
				{
					var image = child as Image;
					var sprite = CreateSprite(image, canvas, parentName);
					levelData.Sprites.Add(sprite);
				}
				else if (typeof(Ellipse) == type)
				{
					var collider = CreateCollider(child as Ellipse, canvas, parentName);
					levelData.Add(collider);
				}
				var logicalChild = child as FrameworkElement;
				TraverseLogicalTree(logicalChild, canvas, EditorTools.ResolveName(logicalChild.Name, parentName));
			}
		}

		private static ColliderCircle CreateCollider(Ellipse collider, Canvas canvas, string parentName)
		{
			var bounds = collider.ConvertBounds(canvas);
			var circle = CircleExtensions.CreateFromBox(bounds);
			return new ColliderCircle(EditorTools.ResolveName(collider.Name, parentName), circle);
		}

		private static Sprite CreateSprite(Image image, Canvas canvas, string parentName)
		{
			var bounds = image.ConvertBounds(canvas);
			var layer = Canvas.GetZIndex(image);
			var namedStream = new NamedStream(image.Source?.ToString(), image.Source.ToStream());
			var sprite = new Sprite(EditorTools.ResolveName(image.Name, parentName), bounds, layer, namedStream);
			//todo: use cached content loader for streams 
			return sprite;
		}
	}
}
