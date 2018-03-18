using System.Numerics;

namespace Example
{
	public interface IParticle
	{
		float Age { get; }
		Vector2 Location { get; }
	}
}