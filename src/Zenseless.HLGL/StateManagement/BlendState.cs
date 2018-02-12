namespace Zenseless.HLGL
{
	using System;

	/// <summary>
	/// Encapsulates the blend state inside an immutable structure
	/// </summary>
	public struct BlendState : IEquatable<BlendState>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlendState"/> structure.
		/// </summary>
		/// <param name="blendOperator">The blend function operator.</param>
		/// <param name="blendParameterSource">The source blend parameter.</param>
		/// <param name="blendParameterDestination">The destination blend parameter.</param>
		public BlendState(BlendOperator blendOperator, BlendParameter blendParameterSource, BlendParameter blendParameterDestination)
		{
			this.blendOperator = blendOperator;
			this.blendParameterSource = blendParameterSource;
			this.blendParameterDestination = blendParameterDestination;
		}

		/// <summary>
		/// Gets the blend function operator.
		/// </summary>
		/// <value>
		/// The blend function operator.
		/// </value>
		public BlendOperator BlendOperator { get => blendOperator; }
		/// <summary>
		/// Gets the blend parameter source.
		/// </summary>
		/// <value>
		/// The blend parameter source.
		/// </value>
		public BlendParameter BlendParameterSource { get => blendParameterSource; }
		/// <summary>
		/// Gets the blend parameter destination.
		/// </summary>
		/// <value>
		/// The blend parameter destination.
		/// </value>
		public BlendParameter BlendParameterDestination { get => blendParameterDestination; }

		/// <summary>
		/// The blend operator
		/// </summary>
		private readonly BlendOperator blendOperator;
		/// <summary>
		/// The blend parameter source
		/// </summary>
		private readonly BlendParameter blendParameterSource;
		/// <summary>
		/// The blend parameter destination
		/// </summary>
		private readonly BlendParameter blendParameterDestination;

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator==(BlendState a, BlendState b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator !=(BlendState a, BlendState b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
		/// </returns>
		public bool Equals(BlendState other)
		{
			//check for value type equality (memory compare)
			return base.Equals(other);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			//check for value type equality (memory compare)
			return base.Equals(obj);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return (int)BlendOperator ^ (int)BlendParameterSource ^ (int)BlendParameterDestination;
		}
	}
}
