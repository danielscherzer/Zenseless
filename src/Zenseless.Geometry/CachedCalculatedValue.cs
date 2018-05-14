namespace Zenseless.Geometry
{
	using System;

	/// <summary>
	/// Class that implements the dirty flag pattern http://gameprogrammingpatterns.com/dirty-flag.html.
	/// A value is cached and only recalculated, if invalidated.
	/// </summary>
	/// <typeparam name="VALUE_TYPE">The type of the cached value.</typeparam>
	public class CachedCalculatedValue<VALUE_TYPE>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CachedCalculatedValue{VALUE_TYPE}"/> class.
		/// </summary>
		/// <param name="calculateValue">Functor for calculating the value.</param>
		/// <exception cref="ArgumentNullException">calculateValue</exception>
		public CachedCalculatedValue(Func<VALUE_TYPE> calculateValue)
		{
			this.calculateValue = calculateValue ?? throw new ArgumentNullException(nameof(calculateValue));
		}


		/// <summary>
		/// Invalidates the cached value.
		/// </summary>
		public void Invalidate() => IsCacheDirty = true;

		/// <summary>
		/// Gets the cached value. If value is not valid (isDirty == true) it will get recalculated.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public VALUE_TYPE Value
		{
			get
			{
				if(IsCacheDirty)
				{
					value = calculateValue();
					IsCacheDirty = false;
				}
				return value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the cache is dirty (needs to be recalculated).
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance cache is dirty; otherwise, <c>false</c>.
		/// </value>
		public bool IsCacheDirty { get; private set; } = true;

		private VALUE_TYPE value;
		private readonly Func<VALUE_TYPE> calculateValue;
	}
}
