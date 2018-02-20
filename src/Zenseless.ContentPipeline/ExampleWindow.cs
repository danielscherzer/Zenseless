using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Zenseless.Base;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Zenseless.ContentPipeline
{
	/// <summary>
	/// Intended for use for small example programs in the <see cref="Zenseless"/> framework
	/// creates a OpenTK.GameWindow;
	/// reads command line arguments: 'capture' records each rendered frame into a png file; 'fullscreen'
	/// handles keys: ESCAPE: closes application; F11: toggles full-screen; <see cref="RemoveDefaultKeyHandler"/>
	/// create a MEF composition container for IOC;
	/// creates experimental versions of high level GL abstraction and resource handling; use with car and subject to change;
	/// </summary>
	public sealed class ExampleWindow
	{
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
		public IContentLoader ContentLoader => contentManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExampleWindow"/> class.
		/// </summary>
		/// <param name="width">The window width.</param>
		/// <param name="height">The window height.</param>
		/// <param name="updateRenderRate">The update and render rate.</param>
		public ExampleWindow(int width = 512, int height = 512, double updateRenderRate = 60)
		{
			gameWindow = new GameWindow
			{
				X = 200, //DPI scaling screws everything up, so use some hacked values
				Y = 100,
				ClientSize = new Size(width, height), //do not set extents in the constructor, because windows 10 with enabled scale != 100% scales our given sizes in the constructor of GameWindow
			};

			ProcessCommandLineArguments();

			RenderContext = new RenderContextGL();

			CreateIOCcontainer();

			gameWindow.TargetUpdateFrequency = updateRenderRate;
			gameWindow.TargetRenderFrequency = updateRenderRate;
			gameWindow.VSync = VSyncMode.On;
			//register callback for resizing of window
			gameWindow.Resize += GameWindow_Resize;
			//register callback for keyboard
			gameWindow.KeyDown += INativeWindowExtensions.DefaultExampleWindowKeyEvents;

			contentManager = ContentManagerGL.Create(Assembly.GetEntryAssembly());
		}

		/// <summary>
		/// Sets the content search directory. 
		/// This is needed if you want to do automatic runtime content reloading if the content source file changes. 
		/// This feature is disabled otherwise. The execution time of this command is dependent on how many files are found inside the given directory.
		/// </summary>
		/// <param name="contentSearchDirectory">The content search directory. Content is found in this directory or subdirectories</param>
		public void SetContentSearchDirectory(string contentSearchDirectory)
		{
			contentManager.SetContentSearchDirectory(contentSearchDirectory);
		}

		/// <summary>
		/// Removes the default key handler.
		/// </summary>
		public void RemoveDefaultKeyHandler() => gameWindow.KeyDown -= INativeWindowExtensions.DefaultExampleWindowKeyEvents;

		/// <summary>
		/// Runs the window loop, which in turn calls the registered event handlers
		/// </summary>
		public void Run()
		{
			//register a callback for updating the game logic
			gameWindow.UpdateFrame += (sender, e) => Update?.Invoke((float)gameWindow.TargetUpdatePeriod);
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += (sender, e) => GameWindowRender();
			//run the update loop, which calls our registered callbacks
			gameWindow.Run();
			screenShots?.ForEach((bmp) => bmp.RotateFlip()); //delayed rotate flip
			screenShots?.SaveToDefaultDir();
		}

		private readonly FileContentManager contentManager;
		private CompositionContainer _container;
		private GameWindow gameWindow;
		private List<Bitmap> screenShots = null;

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
				Console.WriteLine(e.ToString());
			}
		}

		private void GameWindowRender()
		{
			try
			{
				contentManager.CheckForResourceChange();
			}
			catch (ShaderException e)
			{
				e.Data[ShaderLoader.ExceptionDataFileName] = contentManager.LastChangedFilePath; //TODO: make cleaner after removal of old stuff
				new FormShaderExceptionFacade().ShowModal(e);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			//render
			Render?.Invoke();
			screenShots?.Add(FrameBuffer.ToBitmap(false));  //no rotate flip for speed
			DrawTools.WriteErrors();
			//buffer swap of double buffering (http://gameprogrammingpatterns.com/double-buffer.html)
			gameWindow.SwapBuffers();
		}

		/// <summary>
		/// Handles the Resize event of the GameWindow control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void GameWindow_Resize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			Resize?.Invoke(gameWindow.Width, gameWindow.Height);
		}

		private void ProcessCommandLineArguments()
		{
			var args = Environment.GetCommandLineArgs().Skip(1).Select(element => element.ToLowerInvariant());
			if (args.Contains("capture"))
			{
				screenShots = new List<Bitmap>();
			}
			if (args.Contains("fullscreen"))
			{
				gameWindow.WindowState = WindowState.Fullscreen;
			}
		}
	}
}
