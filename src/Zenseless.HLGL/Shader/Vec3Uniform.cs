using System.Numerics;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IUniform" />
	public struct Vec3Uniform : IUniform
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Vec3Uniform"/> struct.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public Vec3Uniform(string name, Vector3 value)
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
		public Vector3 Value { get; }

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
