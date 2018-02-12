using System;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="RESOURCE_TYPE">The type of the esource type.</typeparam>
	public interface IResource<RESOURCE_TYPE> where RESOURCE_TYPE : IDisposable
	{
		/// <summary>
		/// Gets a value indicating whether this instance is value created.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is value created; otherwise, <c>false</c>.
		/// </value>
		bool IsValueCreated { get; }
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		RESOURCE_TYPE Value { get; }

		/// <summary>
		/// Occurs when [change].
		/// </summary>
		event EventHandler<RESOURCE_TYPE> Change;
	}
}
