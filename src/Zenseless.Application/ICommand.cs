namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.HLGL.IState" />
	public interface ICommand : IState
	{
		/// <summary>
		/// Invokes this instance.
		/// </summary>
		void Invoke();
	}
}
