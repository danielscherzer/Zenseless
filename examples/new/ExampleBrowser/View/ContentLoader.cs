using System.ComponentModel.Composition;
using Zenseless.HLGL;

namespace ExampleBrowser.View
{
	[Export(typeof(IContentLoader)), PartCreationPolicy(CreationPolicy.NonShared)]
	class ContentLoader : Zenseless.ExampleFramework.ContentLoader
	{
	}
}
