namespace Zenseless.OpenGL
{
	using System;
	using System.Collections.Generic;
	using System.Numerics;
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
		/// Sets an uniform.
		/// </summary>
		public void SetUniform(IUniform uniform)
		{
			uniforms.Remove(uniform);
			uniforms.Add(uniform);
		}

		/// <summary>
		/// Sets the uniform.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void SetUniform(string name, float value)
		{
			ShaderProgram.Uniform(name, value);
		}

		/// <summary>
		/// Sets the uniform.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void SetUniform(string name, in Vector2 value)
		{
			ShaderProgram.Uniform(name, value);
		}

		/// <summary>
		/// Sets the uniform.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void SetUniform(string name, in Vector3 value)
		{
			ShaderProgram.Uniform(name, value);
		}

		/// <summary>
		/// Sets the uniform.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="camera">The camera.</param>
		public void SetUniform(string name, ITransformation camera)
		{
			ShaderProgram.Uniform(name, camera);
		}

		/// <summary>
		/// Sets the uniform.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void SetUniform(string name, in Vector4 value)
		{
			ShaderProgram.Uniform(name, value);
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
