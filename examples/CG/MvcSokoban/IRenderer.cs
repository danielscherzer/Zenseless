using System.Drawing;

namespace MvcSokoban
{
	public interface IRenderer
	{
		void Clear();
		void DrawLevelState(ILevel levelState, Color tint);
		void Print(string message, float size, TextAlignment alignment = TextAlignment.Left);
		void ResizeWindow(int width, int height);
	}
}