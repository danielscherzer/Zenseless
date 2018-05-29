using Zenseless.Geometry;

namespace Example
{
	public interface ITile
	{
		IReadOnlyBox2D Bounds { get; }
		uint Type { get; }
		bool Walkable { get; }
	}
}