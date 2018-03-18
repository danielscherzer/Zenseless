namespace MiniGalaxyBirds
{
	public class Component<TYPE> : IComponent
	{
		public Component(TYPE value)
		{
			this.Value = value;
		}

		public TYPE Value { get; set; }
	}
}
