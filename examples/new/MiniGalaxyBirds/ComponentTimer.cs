using Zenseless.Patterns;

namespace MiniGalaxyBirds
{
	public class ComponentPeriodicUpdate : PeriodicUpdate, IComponent
	{
		public ComponentPeriodicUpdate(float interval) : base(interval)
		{
			Enabled = true;
		}
	}
}
