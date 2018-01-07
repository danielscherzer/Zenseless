namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TYPE">The type of the ype.</typeparam>
	public struct TypedHandle<TYPE>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TypedHandle{TYPE}"/> struct.
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
		/// Gets a value indicating whether this instance is null.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is null; otherwise, <c>false</c>.
		/// </value>
		public bool IsNull => ID == NULL.ID;

		/// <summary>
		/// The null
		/// </summary>
		public static TypedHandle<TYPE> NULL;
	}
}
