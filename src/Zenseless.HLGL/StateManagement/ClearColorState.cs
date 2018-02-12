using System;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public struct ClearColorState : IEquatable<ClearColorState>
	{
		/// <summary>
		/// The red
		/// </summary>
		private readonly float red;
		/// <summary>
		/// The green
		/// </summary>
		private readonly float green;
		/// <summary>
		/// The blue
		/// </summary>
		private readonly float blue;
		/// <summary>
		/// The alpha
		/// </summary>
		private readonly float alpha;

		/// <summary>
		/// Initializes a new instance of the <see cref="ClearColorState"/> struct.
		/// </summary>
		/// <param name="red">The red.</param>
		/// <param name="green">The green.</param>
		/// <param name="blue">The blue.</param>
		/// <param name="alpha">The alpha.</param>
		public ClearColorState(float red, float green, float blue, float alpha)
		{
			this.red = red;
			this.green = green;
			this.blue = blue;
			this.alpha = alpha;
		}

		/// <summary>
		/// Gets the red.
		/// </summary>
		/// <value>
		/// The red.
		/// </value>
		public float Red { get => red; }
		/// <summary>
		/// Gets the green.
		/// </summary>
		/// <value>
		/// The green.
		/// </value>
		public float Green { get => green; }
		/// <summary>
		/// Gets the blue.
		/// </summary>
		/// <value>
		/// The blue.
		/// </value>
		public float Blue { get => blue; }
		/// <summary>
		/// Gets the alpha.
		/// </summary>
		/// <value>
		/// The alpha.
		/// </value>
		public float Alpha { get => alpha; }

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator==(ClearColorState a, ClearColorState b)
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
		public static bool operator !=(ClearColorState a, ClearColorState b)
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
		public bool Equals(ClearColorState other)
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
			var hashCode = 214250488;
			hashCode = hashCode * -1521134295 + red.GetHashCode();
			hashCode = hashCode * -1521134295 + green.GetHashCode();
			hashCode = hashCode * -1521134295 + blue.GetHashCode();
			hashCode = hashCode * -1521134295 + alpha.GetHashCode();
			return hashCode;
		}
	}
}
