using System;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public interface IResourceProvider
	{
		/// <summary>
		/// Adds the specified name.
		/// </summary>
		/// <typeparam name="RESOURCE_TYPE">The type of the esource type.</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="resource">The resource.</param>
		void Add<RESOURCE_TYPE>(string name, IResource<RESOURCE_TYPE> resource) where RESOURCE_TYPE : IDisposable;
		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <typeparam name="RESOURCE_TYPE">The type of the esource type.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		IResource<RESOURCE_TYPE> Get<RESOURCE_TYPE>(string name) where RESOURCE_TYPE : IDisposable;
	}
}