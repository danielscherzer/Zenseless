namespace Example
{
	public interface IParticle
	{
		float CreationTime { get; }
		float LifeTime { get; set; }
		void Update(float deltaTime);
	}
}