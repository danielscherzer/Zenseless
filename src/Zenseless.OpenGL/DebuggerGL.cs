namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Diagnostics;
	using System.Runtime.InteropServices;

	/// <summary>
	/// A debugger for OpenGL needs an OpenGL context created with the debug flag
	/// </summary>
	public class DebuggerGL
	{
		private readonly DebugProc debugCallback;

		/// <summary>
		/// Initializes a new instance of the <see cref="DebuggerGL"/> class.
		/// </summary>
		public DebuggerGL()
		{
			Debug.WriteLine($"{GL.GetString(StringName.Renderer)} running OpenGL " +
				$"Version {GL.GetInteger(GetPName.MajorVersion)}.{GL.GetInteger(GetPName.MinorVersion)} with ");
			Debug.WriteLine($"Shading Language Version {GL.GetString(StringName.ShadingLanguageVersion)}");
			GL.Enable(EnableCap.DebugOutput);
			GL.Enable(EnableCap.DebugOutputSynchronous);
			//Console.WriteLine(GL.GetString(StringName.Extensions));
			debugCallback = DebugCallback; //need to keep an instance, otherwise delegate is garbage collected
			GL.DebugMessageCallback(debugCallback, IntPtr.Zero);
			GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare, DebugSeverityControl.DontCare, 0, new int[] { }, true);
		}

		private void DebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
		{
			var errorMessage = Marshal.PtrToStringAnsi(message, length);
			var meta = $"OpenGL {type} from {source} with id={id} of {severity} with message";
			Debug.WriteLine(meta);
			Debug.Indent();
			Debug.WriteLine(errorMessage);
			Debug.Unindent();
			//if(DebugSeverity.DebugSeverityNotification != severity) throw new GLException($"{meta} {errorMessage}");
		}
	}
}
