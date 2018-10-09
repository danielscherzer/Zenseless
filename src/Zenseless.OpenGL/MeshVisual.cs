namespace Zenseless.OpenGL
{
	using System.Collections.Generic;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	/// <summary>
	/// 
	/// </summary>
	public struct MeshVisual
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MeshVisual"/> class.
		/// </summary>
		/// <param name="mesh">The mesh.</param>
		/// <param name="shader">The shader.</param>
		/// <param name="textureBindings">The texture bindings.</param>
		public MeshVisual(DefaultMesh mesh, IShaderProgram shader, IEnumerable<TextureBinding> textureBindings = null)
		:this(VAOLoader.FromMesh(mesh, shader), shader, textureBindings)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MeshVisual"/> class.
		/// </summary>
		/// <param name="drawable"></param>
		/// <param name="shader">The shader.</param>
		/// <param name="textureBindings">The texture bindings.</param>
		public MeshVisual(IDrawable drawable, IShaderProgram shader, IEnumerable<TextureBinding> textureBindings = null)
		{
			ShaderProgram = shader;
			Drawable = drawable;
			TextureBindings = textureBindings;
			uniforms = new HashSet<IUniform>(new UniformComparer());
		}

		/// <summary>
		/// Gets the drawable.
		/// </summary>
		/// <value>
		/// The drawable.
		/// </value>
		public IDrawable Drawable { get; }

		/// <summary>
		/// Gets the shader program.
		/// </summary>
		/// <value>
		/// The shader program.
		/// </value>
		public IShaderProgram ShaderProgram { get; }

		/// <summary>
		/// Gets the texture bindings.
		/// </summary>
		/// <value>
		/// The texture bindings.
		/// </value>
		public IEnumerable<TextureBinding> TextureBindings { get; }

		/// <summary>
		/// Sets an uniform persistently. It will be updated each time the visual is drawn.
		/// </summary>
		public void SetUniform(IUniform uniform)
		{
			uniforms.Remove(uniform);
			uniforms.Add(uniform);
		}

		/// <summary>
		/// Draws this instance.
		/// </summary>
		public void Draw()
		{
			ShaderProgram.Activate();
			foreach(var uniform in uniforms)
			{
				uniform.Update(ShaderProgram);
			}
			if (TextureBindings is null)
			{
				Drawable.Draw();
			}
			else
			{
				DrawTools.BindTextures(ShaderProgram, TextureBindings);
				Drawable.Draw();
				DrawTools.UnbindTextures(TextureBindings);
			}
			ShaderProgram.Deactivate();
		}

		private class UniformComparer : IEqualityComparer<IUniform>
		{
			public bool Equals(IUniform x, IUniform y) => string.Equals(x.Name, y.Name);

			public int GetHashCode(IUniform obj) => obj.Name.GetHashCode();
		}
		private HashSet<IUniform> uniforms;
	}
}
