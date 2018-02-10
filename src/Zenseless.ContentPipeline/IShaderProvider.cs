using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Zenseless.ContentPipeline
{
	/// <summary>
	/// 
	/// </summary>
	public interface IShaderProvider
	{
		//public delegate void ShaderChangedHandler(string name, Shader shader);
		//public event ShaderChangedHandler ShaderChanged;

		/// <summary>
		/// Gets the shader.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		IShaderProgram GetShaderProgram(string name);
		//Texture GetTexture(string name);
	}
}