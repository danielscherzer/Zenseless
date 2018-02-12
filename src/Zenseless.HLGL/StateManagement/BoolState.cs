namespace Zenseless.HLGL
{
	using System;

	/// <summary>
	/// Encapsulates a boolean state inside an immutable structure
	/// </summary>
	public struct BoolState<NAME> : IEquatable<BoolState<NAME>>
	{
		/// <summary>
		/// The enabled
		/// </summary>
		public static BoolState<NAME> Enabled = new BoolState<NAME>(true);
		/// <summary>
		/// The disabled
		/// </summary>
		public static BoolState<NAME> Disabled = new BoolState<NAME>(false);

		private readonly bool isEnabled;

		/// <summary>
		/// Initializes a new instance of the <see cref="BoolState{NAME}"/> struct.
		/// </summary>
		/// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
		public BoolState(bool isEnabled)
		{
			this.isEnabled = isEnabled;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
		/// </value>
		public bool IsEnabled => isEnabled;

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator ==(BoolState<NAME> a, BoolState<NAME> b)
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
		public static bool operator !=(BoolState<NAME> a, BoolState<NAME> b)
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
		public bool Equals(BoolState<NAME> other)
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
			return IsEnabled.GetHashCode();
		}
	}
}

