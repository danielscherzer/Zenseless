using Zenseless.Geometry;

namespace Example
{
	public interface ITile
	{
		IReadOnlyBox2D Bounds { get; }
		IReadOnlyBox2D TexCoords { get; }
	}
}