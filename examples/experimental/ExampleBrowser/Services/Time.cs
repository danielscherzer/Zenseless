namespace ExampleBrowser.Services
{
	using System.ComponentModel.Composition;
	using Zenseless.Patterns;

	[Export(typeof(ITime))]
	[Export(typeof(Time))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class Time : GameTime
	{
	}
}
