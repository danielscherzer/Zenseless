using Zenseless.Base;
using Zenseless.Geometry;

namespace MiniGalaxyBirds
{
	public class ComponentClipper : IComponent, ITimedUpdate
	{
		public ComponentClipper(IReadOnlyBox2D clipFrame, IReadOnlyBox2D frame, Clip clip)
		{
			ClipFrame = clipFrame;
			Frame = frame;
			OnClip = clip;
		}

		public void Update(float absoluteTime)
		{
			if (!(OnClip is null) && !ClipFrame.Intersects(Frame))
			{
				OnClip();
			}
		}

		public IReadOnlyBox2D ClipFrame { get; private set; }
		public IReadOnlyBox2D Frame { get; private set; }

		public delegate void Clip();
		public event Clip OnClip;
	}
}
