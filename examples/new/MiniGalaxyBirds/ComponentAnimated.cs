namespace MiniGalaxyBirds
{
	public class ComponentAnimated : ComponentPeriodicUpdate, IAnimation
	{
		public ComponentAnimated(float length) : base(length) { }

		public float Length	{ get { return this.Period; }	}

		public float Time { get { return this.PeriodRelativeTime; } }
	}
}
