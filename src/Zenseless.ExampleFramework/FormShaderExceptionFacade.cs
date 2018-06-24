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
		/// <param name="name">Name or file name of the shader</param>
		public void ShowModal(ShaderException exception, string name)
		{
			form = new FormShaderException
			{
				Text = $"'{name}': {exception.GetType()}"
			};
			var compileException = exception as ShaderCompileException;
			if (compileException is null)
			{
				form.ShaderSourceCode = string.Empty;
			}
			else
			{
				form.Text += " for shader type " + compileException.ShaderType;
				form.ShaderSourceCode = compileException.ShaderSourceCode;
			}
			//load error list after source code is loaded for highlighting of error to work
			form.Errors.Clear();
			var log = new ShaderLog(exception.Message);
			foreach (var logLine in log.Lines)
			{
				form.Errors.Add(logLine);

			}
			foreach (var logLine in log.Lines)
			{
				Debug.Print(name + "(" + logLine.LineNumber + "): " + logLine.Message);
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
			if (newShaderSourceCode != compileException.ShaderSourceCode && File.Exists(name))
			{
				//save changed code to shader file
				File.WriteAllText(name, newShaderSourceCode);
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
