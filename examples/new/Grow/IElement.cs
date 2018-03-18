using System.Numerics;

namespace Example
{
	public interface IElement
	{
		Vector2 Coord { get; }
		float Size { get; }
	}
}