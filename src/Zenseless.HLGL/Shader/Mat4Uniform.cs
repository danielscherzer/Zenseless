using System.Numerics;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IUniform" />
	public struct Mat4Uniform : IUniform
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Mat4Uniform"/> struct.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public Mat4Uniform(string name, Matrix4x4 value)
		{
			Name = name;
			Value = value;
		}

		/// <summary>
		/// Gets the name of the uniform.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; }
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public Matrix4x4 Value { get; }

		/// <summary>
		/// Updates the specified program.
		/// </summary>
		/// <param name="program">The program.</param>
		public void Update(IShaderProgram program)
		{
			program.Uniform(Name, Value);
		}
	}
}
