using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zenseless.Geometry
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="IEnumerable{T}" />
	public class ControlPoints<T>  : IEnumerable<KeyValuePair<float, T>>
	{
		/// <summary>
		/// Adds the update.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="value">The value.</param>
		public void AddUpdate(float t, T value)
		{
			controlPoints[t] = value;
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			controlPoints.Clear();
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>
		/// The count.
		/// </value>
		public int Count { get { return controlPoints.Count; } }

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// An enumerator that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<KeyValuePair<float, T>> GetEnumerator()
		{
			return controlPoints.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return controlPoints.GetEnumerator();
		}

		/// <summary>
		/// Finds the infimum.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public KeyValuePair<float, T> FindInfimum(float t)
		{
			var firstItem = this.First();
			if (firstItem.Key > t) return firstItem;
			try
			{
				return this.Last((item) => item.Key <= t);
			}
			catch (InvalidOperationException)
			{
				return firstItem;
			}
		}

		/// <summary>
		/// Finds the pair.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="epsilon">The epsilon.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">No control points to interpolate!</exception>
		public Tuple<T, T, float> FindPair(float t, float epsilon = 0.001f)
		{
			if (0 == Count) throw new ArgumentException("No control points to interpolate!");
			var first = FindInfimum(t);
			var second = FindSupremum(t);
			float keyDelta = second.Key - first.Key;
			//if too little time in between return data value of infimum 
			float factor = (epsilon > Math.Abs(keyDelta)) ? 0 : (t - first.Key) / keyDelta;
			return new Tuple<T, T, float>(first.Value, second.Value, factor);
		}

		/// <summary>
		/// Finds the supremum.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public KeyValuePair<float, T> FindSupremum(float t)
		{
			var lastItem = this.Last();
			if (lastItem.Key < t) return lastItem;
			try
			{
				return this.First((item) => item.Key >= t);
			}
			catch (InvalidOperationException)
			{
				return lastItem;
			}
		}

		/// <summary>
		/// The control points
		/// </summary>
		private SortedDictionary<float, T> controlPoints = new SortedDictionary<float, T>();
	}
}
