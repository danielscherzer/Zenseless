using Zenseless.HLGL;
using Zenseless.OpenGL;
using System.IO;
using System.Text;
using Zenseless.Base;

namespace Zenseless.ContentPipeline
{
	/// <summary>
	/// 
	/// </summary>
	public class ShaderFileDebugger
	{
		//public delegate void ShaderLoadedHandler();
		//public event ShaderLoadedHandler ShaderLoaded;

		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderFileDebugger"/> class.
		/// </summary>
		/// <param name="vertexFile">The vertex file.</param>
		/// <param name="fragmentFile">The fragment file.</param>
		/// <param name="vertexShader">The vertex shader.</param>
		/// <param name="fragmentShader">The fragment shader.</param>
		public ShaderFileDebugger(string vertexFile, string fragmentFile,
			byte[] vertexShader = null, byte[] fragmentShader = null)
		{
			if (File.Exists(vertexFile) && File.Exists(fragmentFile))
			{
				shaderWatcherVertex = new FileWatcher(vertexFile);
				shaderWatcherVertex.Changed += (s, e) => form.Close();
				shaderWatcherFragment = new FileWatcher(fragmentFile);
				shaderWatcherFragment.Changed += (s, e) => form.Close();
			}
			else
			{
				var sVertex = vertexShader is null ? string.Empty : Encoding.UTF8.GetString(vertexShader);
				var sFragment = fragmentShader is null ? string.Empty : Encoding.UTF8.GetString(fragmentShader);
				shaderProgram = ShaderLoader.FromStrings(sVertex, sFragment);
				//ShaderLoaded?.Invoke(); //is null because we are in the constructor
			}
		}

		/// <summary>
		/// Checks for shader change.
		/// </summary>
		/// <returns></returns>
		public bool CheckForShaderChange()
		{
			//test if we even have file -> no files nothing to be done
			if (shaderWatcherVertex is null || shaderWatcherFragment is null) return false;
			//test if any file is dirty
			if (!shaderWatcherVertex.Dirty && !shaderWatcherFragment.Dirty) return false;
			try
			{
				shaderProgram = ShaderLoader.FromFiles(shaderWatcherVertex.FullPath, shaderWatcherFragment.FullPath);
				shaderWatcherVertex.Dirty = false;
				shaderWatcherFragment.Dirty = false;
				//ShaderLoaded?.Invoke();
				return true;
			}
			catch (IOException e)
			{
				var exception = new ShaderException(e.Message, string.Empty);
				ShowDebugDialog(exception);
			}
			catch (ShaderException e)
			{
				ShowDebugDialog(e);
			}
			return false;
		}

		/// <summary>
		/// Shows the debug dialog.
		/// </summary>
		/// <param name="exception">The exception.</param>
		private void ShowDebugDialog(ShaderException exception)
		{
			var newShaderCode = form.ShowModal(exception);
		}

		/// <summary>
		/// Gets the shader.
		/// </summary>
		/// <value>
		/// The shader.
		/// </value>
		public IShaderProgram ShaderProgram { get { return shaderProgram; } }

		/// <summary>
		/// The shader
		/// </summary>
		private IShaderProgram shaderProgram;
		/// <summary>
		/// The shader watcher vertex
		/// </summary>
		private FileWatcher shaderWatcherVertex = null;
		/// <summary>
		/// The shader watcher fragment
		/// </summary>
		private FileWatcher shaderWatcherFragment = null;
		/// <summary>
		/// The form
		/// </summary>
		private readonly FormShaderExceptionFacade form = new FormShaderExceptionFacade();
	}
}
