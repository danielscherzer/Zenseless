using OpenTK;

namespace Example
{
	public interface IBody
	{
		Vector3 Location { get; }
		float Mass { get; }
	}
}
