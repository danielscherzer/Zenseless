namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.HLGL.IState" />
	public interface IStateBool : IState
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IStateBool"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		bool Enabled { get; set; }
	}
}