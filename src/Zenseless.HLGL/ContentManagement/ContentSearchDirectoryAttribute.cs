using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Defines an attribute that is used by Zenseless to set the content search directory
	/// </summary>
	/// <seealso cref="Attribute" />
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public class ContentSearchDirectoryAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContentSearchDirectoryAttribute"/> class.
		/// </summary>
		/// <param name="contentSearchDirectory">The content search directory.</param>
		public ContentSearchDirectoryAttribute([CallerFilePath] string contentSearchDirectory = "")
		{
			ContentSearchDirectory = Path.GetDirectoryName(contentSearchDirectory);
		}

		/// <summary>
		/// Gets the content search directory.
		/// </summary>
		/// <value>
		/// The content search directory.
		/// </value>
		public string ContentSearchDirectory { get; }
	}
}