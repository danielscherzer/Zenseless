using Zenseless.Patterns;
using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Zenseless.OpenGL
{
	using Handle = TypedHandle<IShaderProgram>;

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.Patterns.Disposable" />
	public class ShaderManagerGL : Disposable
	{
		/// <summary>
		/// Creates the program.
		/// </summary>
		/// <returns></returns>
		public Handle CreateProgram()
		{
			var handle = new Handle(GL.CreateProgram());
			handles.Add(handle);
			return handle;
		}

		/// <summary>
		/// Removes the program.
		/// </summary>
		/// <param name="handle">The handle.</param>
		/// <exception cref="ArgumentNullException">Empty shader Handle</exception>
		public void RemoveProgram(Handle handle)
		{
			if (handle.IsNull) throw new ArgumentNullException("Empty shader Handle");
			GL.DeleteProgram(handle.ID);
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			foreach(var handle in handles)
			{
				RemoveProgram(handle);
			}
		}

		/// <summary>
		/// The handles
		/// </summary>
		private List<Handle> handles = new List<Handle>();
	}
}
