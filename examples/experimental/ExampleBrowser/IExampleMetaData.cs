using System.ComponentModel;

namespace ExampleBrowser
{
	public interface IExampleMetaData
	{
		[DefaultValue("** Unknown name **")]
		string Name { get; }
	}
}