namespace Zenseless.ExampleFramework
{
	using OpenTK;
	using OpenTK.Input;
	using OpenTK.Platform;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.ComponentModel.Composition.Hosting;
	using System.Linq;
	using Zenseless.Patterns;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;
	using System.Reflection;
	using OpenTK.Graphics;
	using System.Diagnostics;
	using System.IO;

	/// <summary>
	/// Intended for use for small example programs in the <see cref="Zenseless"/> framework
	/// creates a OpenTK.GameWindow;
	/// reads command line arguments: 'capture' records each rendered frame into a png file; 'fullscreen'
	/// handles keys: ESCAPE: closes application; F11: toggles full-screen; <see cref="RemoveDefaultKeyHandler"/> F12: makes a screen-shot to the clipboard
	/// create a MEF composition container for IOC;
	/// creates experimental versions of high level GL abstraction and resource handling; use with care and subject to change;
	/// </summary>
	public sealed class ExampleWindow : Disposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExampleWindow"/> class.
		/// </summary>
		/// <param name="width">The window width.</param>
		/// <param name="height">The window height.</param>
		/// <param name="updateRenderRate">The update and render rate.</param>
		/// <param name="samples">Anit-aliasing sample count</param>
		/// <param name="debug">Activate OpenGL debug mode (probably slower)</param>
		public ExampleWindow(int width = 1024, int height = 1024, double updateRenderRate = 60, int samples = 1, bool debug = false)
		{
			Debug.Listeners.Add(new ConsoleDebugListener());
			var graphicsMode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, samples, ColorFormat.Empty, 2);
			gameWindow = new GameWindow(width,height, graphicsMode, Assembly.GetEntryAssembly().ManifestModule.Name
				, GameWindowFlags.Default, DisplayDevice.Default, 0, 0, debug ? GraphicsContextFlags.Debug : GraphicsContextFlags.Default)
			{
				X = 200, //DPI scaling screws everything up, so use some hacked values
				Y = 10,
				ClientSize = new OpenTK.Size(width, height), //do not set extents in the constructor, because windows 10 with enabled scale != 100% scales our given sizes in the constructor of GameWindow
			};

			Input = new Input(gameWindow);

			if(debug)
			{
				debugger = new DebuggerGL();
			}
			CreateIOCcontainer();

			RenderContext = new RenderContextGL();

			ProcessCommandLineArguments(ref updateRenderRate);

			gameWindow.TargetUpdateFrequency = updateRenderRate;
			gameWindow.TargetRenderFrequency = updateRenderRate;
			gameWindow.VSync = VSyncMode.On;
			//register callback for keyboard
			gameWindow.KeyDown += INativeWindowExtensions.DefaultExampleWindowKeyEvents;
			gameWindow.KeyDown += GameWindow_KeyDown;
			//register a callback for updating the game logic
			gameWindow.UpdateFrame += GameWindow_UpdateFrame;
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			//register callback for resizing of window
			gameWindow.Resize += GameWindow_Resize;

			var contentLoader = new ContentLoader();
			beforeRenderingCallbacks.Add(contentLoader);
			ContentLoader = contentLoader;
		}

		/// <summary>
		/// Occurs when in the render loop a new render of the window should be handled. Usually once per frame
		/// </summary>
		public event Action Render;

		/// <summary>
		/// Gets the game window.
		/// </summary>
		/// <value>
		/// The game window.
		/// </value>
		public IGameWindow GameWindow { get { return gameWindow; } }

		/// <summary>
		/// Experimental! Gets the render context.
		/// </summary>
		/// <value>
		/// The render context.
		/// </value>
		public IRenderContext RenderContext { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public delegate void ResizeHandler(int width, int height);

		/// <summary>
		/// Occurs when the window is resized.
		/// </summary>
		public event ResizeHandler Resize;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="updatePeriod">The update period.</param>
		public delegate void UpdateHandler(float updatePeriod);

		/// <summary>
		/// Occurs when in the update loop a new update should be handled. Usually once per frame
		/// </summary>
		public event UpdateHandler Update;

		/// <summary>
		/// Gets the content manager.
		/// </summary>
		/// <value>
		/// The content manager.
		/// </value>
		public IContentLoader ContentLoader { get; }

		/// <summary>
		/// Removes the default key handler.
		/// </summary>
		public void RemoveDefaultKeyHandler()
		{
			gameWindow.KeyDown -= GameWindow_KeyDown;
			gameWindow.KeyDown -= INativeWindowExtensions.DefaultExampleWindowKeyEvents;
		}

		/// <summary>
		/// Runs the window loop, which in turn calls the registered event handlers
		/// </summary>
		public void Run()
		{
			//run the update loop, which calls our registered callbacks
			gameWindow.Run();
		}

		private CompositionContainer _container;
		private GameWindow gameWindow;

		/// <summary>
		/// Retrieve keyboard and mouse input state.
		/// </summary>
		/// <value>
		/// The input state.
		/// </value>
		public IInput Input { get; }

		private readonly List<IAfterRendering> afterRenderingCallbacks = new List<IAfterRendering>();
		private readonly List<IBeforeRendering> beforeRenderingCallbacks = new List<IBeforeRendering>();
		private readonly DebuggerGL debugger;

		private void CreateIOCcontainer()
		{
			var catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new AssemblyCatalog(typeof(ExampleWindow).Assembly));
			_container = new CompositionContainer(catalog);
			try
			{
				_container.SatisfyImportsOnce(this);
			}
			catch (CompositionException e)
			{
				Debug.WriteLine($"Example Window error: {e}");
			}
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.F12:
					var bitmap = FrameBuffer.ToBitmap();
					bitmap.SaveToClipboard();
					var assemblyPath = Assembly.GetEntryAssembly().Location;
					var screenShotName = Path.ChangeExtension(assemblyPath, " screenshot.png");
					bitmap.Save(screenShotName);
					break;
			}
		}

		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			beforeRenderingCallbacks.ForEach((i) => i.BeforeRendering());
			Render?.Invoke();
			afterRenderingCallbacks.ForEach((i) => i.AfterRendering());
			//buffer swap (and sync) of double buffering (http://gameprogrammingpatterns.com/double-buffer.html)
			gameWindow.SwapBuffers();
		}

		/// <summary>
		/// Handles the Resize event of the GameWindow control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void GameWindow_Resize(object sender, EventArgs e)
		{
			if (0 == gameWindow.Width || 0 == gameWindow.Height) return;
			RenderContext.RenderState.Set(new Viewport(0, 0, gameWindow.Width, gameWindow.Height));
			Resize?.Invoke(gameWindow.Width, gameWindow.Height);
		}

		private void GameWindow_UpdateFrame(object _, FrameEventArgs e)
		{
			Update?.Invoke((float)gameWindow.TargetUpdatePeriod);
		}
		
		private void ProcessCommandLineArguments(ref double updateRenderRate)
		{
			var args = Environment.GetCommandLineArgs().Skip(1).Select(element => element.ToLowerInvariant());
			if (args.Contains("capture"))
			{
				afterRenderingCallbacks.Add(new FrameGrabber(PathTools.GetCurrentProcessOutputDir()));
				updateRenderRate = 30;
			}
			if (args.Contains("fullscreen"))
			{
				gameWindow.WindowState = WindowState.Fullscreen;
			}
		}

		/// <summary>
		/// Disposes the resources.
		/// </summary>
		protected override void DisposeResources()
		{
			foreach(var callback in afterRenderingCallbacks)
			{
				(callback as IDisposable)?.Dispose();
			}
			foreach (var callback in beforeRenderingCallbacks)
			{
				(callback as IDisposable)?.Dispose();
			}
			//RenderContext.Dispose();
			_container.Dispose();
			gameWindow.Dispose();
			//contentManager
		}
	}
}
