namespace Zenseless.OpenGL
{
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using OpenTK.Graphics.OpenGL4;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	/// <summary>
	/// 
	/// </summary>
	public class MeshVisual
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
			shaderProgram = shader;
			this.drawable = drawable;
			if (textureBindings is null)
			{
				this.textureBindings = new List<TextureBinding>();
			}
			else
			{
				this.textureBindings = textureBindings;
			}
		}

		/// <summary>
		/// Sets the uniform.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void SetUniform(string name, float value)
		{
			uniformsFloat[name] = value;
		}

		/// <summary>
		/// Sets the uniform.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void SetUniform(string name, in Vector2 value)
		{
			uniformsVec2[name] = value;
		}

		/// <summary>
		/// Sets the uniform.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void SetUniform(string name, in Vector3 value)
		{
			uniformsVec3[name] = value;
		}
		/// <summary>
		/// Sets the uniform.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void SetUniform(string name, in Vector4 value)
		{
			uniformsVec4[name] = value;
		}

		/// <summary>
		/// Draws the specified uniform setter.
		/// </summary>
		/// <param name="uniformSetter">The uniform setter.</param>
		public void Draw(Action<IShaderProgram> uniformSetter = null)
		{
			shaderProgram.Activate();
			DrawTools.BindTextures(shaderProgram, textureBindings);
			NewMethod();
			uniformSetter?.Invoke(shaderProgram);
			drawable.Draw();
			DrawTools.UnbindTextures(textureBindings);
			shaderProgram.Deactivate();
		}

		private void NewMethod()
		{
			foreach (var uniform in uniformsFloat)
			{
				shaderProgram.Uniform(uniform.Key, uniform.Value);
			}
			foreach (var uniform in uniformsVec2)
			{
				shaderProgram.Uniform(uniform.Key, uniform.Value);
			}
			foreach (var uniform in uniformsVec3)
			{
				shaderProgram.Uniform(uniform.Key, uniform.Value);
			}
			foreach (var uniform in uniformsVec4)
			{
				shaderProgram.Uniform(uniform.Key, uniform.Value);
			}
		}

		private readonly IShaderProgram shaderProgram;
		private readonly IDrawable drawable;
		private readonly IEnumerable<TextureBinding> textureBindings;
		private readonly Dictionary<string, float> uniformsFloat = new Dictionary<string, float>();
		private readonly Dictionary<string, Vector2> uniformsVec2 = new Dictionary<string, Vector2>();
		private readonly Dictionary<string, Vector3> uniformsVec3 = new Dictionary<string, Vector3>();
		private readonly Dictionary<string, Vector4> uniformsVec4 = new Dictionary<string, Vector4>();
	}
}
