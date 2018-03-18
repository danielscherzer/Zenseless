using Zenseless.Geometry;

namespace MiniGalaxyBirds
{
	public interface IDrawable
	{
		IReadOnlyBox2D Rect { get; }
		void Draw();
	}
}
