using Zenseless.Geometry;
using System.Numerics;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public interface IDrawConfiguration
	{
		/// <summary>
		/// Gets or sets a value indicating whether [backface culling].
		/// </summary>
		/// <value>
		///   <c>true</c> if [backface culling]; otherwise, <c>false</c>.
		/// </value>
		bool BackfaceCulling { get; set; }
		
		/// <summary>
		/// Gets or sets the instance count.
		/// </summary>
		/// <value>
		/// The instance count.
		/// </value>
		int InstanceCount { get; set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether [shader point size].
		/// </summary>
		/// <value>
		///   <c>true</c> if [shader point size]; otherwise, <c>false</c>.
		/// </value>
		bool ShaderPointSize { get; set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether [z buffer test].
		/// </summary>
		/// <value>
		///   <c>true</c> if [z buffer test]; otherwise, <c>false</c>.
		/// </value>
		bool ZBufferTest { get; set; }

		/// <summary>
		/// Draws the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Draw(IRenderContext context);
		
		/// <summary>
		/// Sets the input texture.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="image">The image.</param>
		void SetInputTexture(string name, IOldRenderSurface image);
		
		/// <summary>
		/// Updates the instance attribute.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="data">The data.</param>
		void UpdateInstanceAttribute(string name, float[] data);
		
		/// <summary>
		/// Updates the instance attribute.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="data">The data.</param>
		void UpdateInstanceAttribute(string name, int[] data);
		
		/// <summary>
		/// Updates the instance attribute.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="data">The data.</param>
		void UpdateInstanceAttribute(string name, Vector2[] data);
		
		/// <summary>
		/// Updates the instance attribute.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="data">The data.</param>
		void UpdateInstanceAttribute(string name, Vector3[] data);
		
		/// <summary>
		/// Updates the instance attribute.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="data">The data.</param>
		void UpdateInstanceAttribute(string name, Vector4[] data);

		/// <summary>
		/// Updates the shader buffer.
		/// </summary>
		/// <typeparam name="DATA_ELEMENT_TYPE">The type of the ata element type.</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="uniformArray">The uniform array.</param>
		void UpdateShaderBuffer<DATA_ELEMENT_TYPE>(string name, DATA_ELEMENT_TYPE[] uniformArray) where DATA_ELEMENT_TYPE : struct;
		
		/// <summary>
		/// Updates the uniforms.
		/// </summary>
		/// <typeparam name="DATA">The type of the ata.</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="uniforms">The uniforms.</param>
		void UpdateUniformBuffer<DATA>(string name, DATA uniforms) where DATA : struct;
	}
}