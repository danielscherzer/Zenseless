using System.Reflection;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public static class ResourceLoading
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="streamLoader"></param>
		/// <param name="resourceAssembly"></param>
		public static void AddMappings(this StreamLoader streamLoader, Assembly resourceAssembly)
		{
			var resourceNames = resourceAssembly.GetManifestResourceNames();

			foreach (var name in resourceNames)
			{
				streamLoader.AddStreamCreator(name, () => resourceAssembly.GetManifestResourceStream(name));
			}
		}
	}
}
