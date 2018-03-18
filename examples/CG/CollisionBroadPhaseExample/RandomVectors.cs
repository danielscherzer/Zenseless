using System;
using System.Numerics;

namespace Example
{
	public class RandomVectors
	{
		public static Vector2 Velocity()
		{
			var rndData = new byte[2];
			rnd.NextBytes(rndData);
			var velocity = new Vector2(rndData[0] - 128, rndData[1] - 128);
			velocity *= 0.0005f;
			return velocity;
		}

		private static Random rnd = new Random(12);
	}
}
