using Zenseless.HLGL;
using Zenseless.OpenGL;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Zenseless.ExampleFramework
{
	/// <summary>
	/// 
	/// </summary>
	public class FormShaderExceptionFacade
	{
		/// <summary>
		/// Shows the modal.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="shaderFileName">Name of the shader file.</param>
		/// <returns></returns>
		public void ShowModal(ShaderException exception, string shaderFileName)
		{
			form = new FormShaderException
			{
				Text = $"'{shaderFileName}': {exception.GetType()}"
			};
			var compileException = exception as ShaderCompileException;
			if (compileException is null)
			{
				form.ShaderSourceCode = string.Empty;
			}
			else
			{
				form.ShaderSourceCode = compileException.ShaderSourceCode;
			}
			//load error list after source code is loaded for highlighting of error to work
			form.Errors.Clear();
			var log = new ShaderLog(exception.Message);
			foreach (var logLine in log.Lines)
			{
				form.Errors.Add(logLine);

			}
			if (string.IsNullOrEmpty(shaderFileName))
			{
				foreach (var logLine in log.Lines)
				{
					Debug.Print(shaderFileName + "(" + logLine.LineNumber + "): " + logLine.Message);
				}
			}
			form.Select(0);
			form.TopMost = true;
			var oldSource = form.ShaderSourceCode;
			closeOnFileChange = true;
			var result = form.ShowDialog();
			closeOnFileChange = false;
			var newShaderSourceCode = DialogResult.OK == result ? form.ShaderSourceCode : oldSource;
			form = null;

			if (compileException is null) return;
			if (newShaderSourceCode != compileException.ShaderSourceCode && !string.IsNullOrEmpty(shaderFileName))
			{
				//save changed code to shader file
				File.WriteAllText(shaderFileName, newShaderSourceCode);
			}
		}

		/// <summary>
		/// Closes this instance.
		/// </summary>
		public void Close()
		{
			if (form is null) return;
			if (!closeOnFileChange) return;
			form.Invoke((MethodInvoker)delegate
			{
				form.Close();
			});
		}

		/// <summary>
		/// The form
		/// </summary>
		private FormShaderException form = null;
		/// <summary>
		/// The close on file change
		/// </summary>
		private bool closeOnFileChange = false;
	}
}
