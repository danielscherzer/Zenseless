using System.Numerics;
using Zenseless.Geometry;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IUniform" />
	public struct TransformUniform : IUniform
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TransformUniform"/> struct.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public TransformUniform(string name, ITransformation value)
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
		public ITransformation Value { get; }

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
