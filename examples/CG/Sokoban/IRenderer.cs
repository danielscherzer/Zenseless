using System.Drawing;

namespace Example
{
	public interface IRenderer
	{
		void Clear();
		void DrawLevelState(ILevelGrid levelState, Color tint);
		void Print(string message, float size, TextAlignment alignment = TextAlignment.Left);
		void ResizeWindow(int width, int height);
	}
}