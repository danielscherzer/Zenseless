using System;
using Zenseless.Patterns;

namespace Zenseless.ExampleFramework
{
	internal class ExampleServices //TODO: instead of ExampleWindow
	{
		public ExampleServices()
		{
			//serviceLocator.RegisterService();
		}

		private TypeRegistry serviceLocator = new TypeRegistry();
	}
}
