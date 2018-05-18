namespace ExampleBrowser.Services
{
	using System.ComponentModel.Composition;
	using Zenseless.HLGL;

	[Export(typeof(IContentLoader)), PartCreationPolicy(CreationPolicy.NonShared)]
	class ContentLoader : Zenseless.ExampleFramework.ContentLoader
	{
	}
}
