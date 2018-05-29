using Zenseless.Geometry;
namespace MiniGalaxyBirds
{
	public interface IRenderer
	{
		IDrawable CreateDrawable(string type, IReadOnlyBox2D frame);
		IDrawable CreateDrawable(string type, IReadOnlyBox2D frame, IAnimation animation);
		void DeleteDrawable(IDrawable drawable);
	}
}
