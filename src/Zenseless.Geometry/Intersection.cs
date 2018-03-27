namespace Zenseless.Geometry
{
	/// <summary>
	/// Class with extension methods for intersection tests of different objects
	/// </summary>
	public static class IntersectionExtensions
	{
		/// <summary>
		/// Test for intersection of the specified box and circle (excluding borders).
		/// </summary>
		/// <param name="box">The box.</param>
		/// <param name="circle">The circle.</param>
		/// <returns>True if the two objects overlap.</returns>
		public static bool Intersects(this IReadOnlyBox2D box, IReadOnlyCircle circle)
		{
			float AxisDeltaDelta(float min, float max, float center)
			{
				var diffMin = min - center;
				if (0 < diffMin) //left case
				{
					return diffMin * diffMin;
				}
				else
				{
					var diffMax = center - max;
					if (0 < diffMax) //right case
					{
						return diffMax * diffMax;
					}
				}
				return 0f;
			}

			float d = 0f;
			d += AxisDeltaDelta(box.MinX, box.MaxX, circle.CenterX);
			d += AxisDeltaDelta(box.MinY, box.MaxY, circle.CenterY);
			return d < circle.Radius * circle.Radius;
		}
	}
}
