namespace Zenseless.Geometry
{
	using System.ComponentModel;

	/// <summary>
	/// A transform that notifies on property changes
	/// </summary>
	/// <seealso cref="INotifyPropertyChanged" />
	/// <seealso cref="ITransformation" />
	public interface INotifyingTransform : INotifyPropertyChanged, ITransformation
	{
	}
}