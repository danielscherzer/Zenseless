using Zenseless.Geometry;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public static class ShaderExtensions
	{
		/// <summary>
		/// Set transformation uniforms (mat4) on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="shaderProgram">The shader program.</param>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="transformation">The transformation</param>
		public static void Uniform(this IShaderProgram shaderProgram, string name, in ITransformation transformation)
		{
			shaderProgram.Uniform(name, transformation.Matrix, true);
		}
	}
}
