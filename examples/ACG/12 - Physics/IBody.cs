namespace Example
{
	using System.Numerics;

	public interface IBody
	{
		Vector3 Location { get; }
		float Mass { get; }
	}
}
