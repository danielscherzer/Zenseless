using System;
using System.Runtime.InteropServices;

namespace Zenseless.ExampleFramework
{
	/// <summary>
	/// Defines an attribute that is used by Zenseless to detect if SOLUTION is defined.
	/// Solution code is removed upon template generation
	/// </summary>
	/// <seealso cref="Attribute" />
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public class SolutionAttribute : Attribute
	{
	}
}