namespace Zenseless.Patterns
{
	/// <summary>
	/// A structure for strongly typed handles
	/// </summary>
	/// <typeparam name="TYPE">The type of the handle.</typeparam>
	public struct TypedHandle<TYPE>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TypedHandle{TYPE}"/> structure.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public TypedHandle(int id)
		{
			ID = id;
		}

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public int ID { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance is the null handle.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is the null handle; otherwise, <c>false</c>.
		/// </value>
		public bool IsNullHandle => ID == NullHandle.ID;

		/// <summary>
		/// The null handle.
		/// </summary>
		public static TypedHandle<TYPE> NullHandle = new TypedHandle<TYPE>(-1);
	}
}
