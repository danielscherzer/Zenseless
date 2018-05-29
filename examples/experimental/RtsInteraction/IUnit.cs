using Zenseless.Geometry;

namespace Example
{
	public interface IUnit
	{
		IReadOnlyCircle Bounds { get; }
		bool IsMoving { get; }
		float Orientation { get; }
		bool Selected { get; }
	}
}