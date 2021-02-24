﻿using System;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Contains static/extension methods for System.Math and System.Numerics for more mathematical operations, 
	/// often overloaded for Vector types.
	/// Operations include Clamp, Round, Lerp, Floor, Mod
	/// </summary>
	public static class MathHelper
	{
		/// <summary>
		/// The mathematical constant PI
		/// </summary>
		public static float PI = (float)Math.PI;

		/// <summary>
		/// 2 * PI
		/// </summary>
		public static float TWO_PI = (float)(Math.PI * 2.0);

		/// <summary>
		/// Returns for each component the smallest integer bigger than or equal to the specified floating-point number.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static Vector2 Ceiling(in Vector2 value)
		{
			return new Vector2(CeilingF(value.X), CeilingF(value.Y));
		}
		/// <summary>
		/// Returns the smallest integer bigger than or equal to the specified floating-point number.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static float CeilingF(float value) => (float)Math.Ceiling(value);

		/// <summary>
		/// Clamp the input value x in between min and max. 
		/// If x smaller min return min; 
		/// if x bigger max return max; 
		/// else return x unchanged
		/// </summary>
		/// <param name="x">input value that will be clamped</param>
		/// <param name="min">lower limit</param>
		/// <param name="max">upper limit</param>
		/// <returns>clamped version of x</returns>
		public static int Clamp(this int x, int min, int max)
		{
			return Math.Min(max, Math.Max(min, x));
		}
		/// <summary>
		/// Clamp the input value x in between min and max. 
		/// If x smaller min return min; 
		/// if x bigger max return max; 
		/// else return x unchanged
		/// </summary>
		/// <param name="x">input value that will be clamped</param>
		/// <param name="min">lower limit</param>
		/// <param name="max">upper limit</param>
		/// <returns>clamped version of x</returns>
		public static float Clamp(this float x, float min, float max)
		{
			return Math.Min(max, Math.Max(min, x));
		}

		/// <summary>
		/// Clamp the input value x in between min and max. 
		/// If x smaller min return min; 
		/// if x bigger max return max; 
		/// else return x unchanged
		/// </summary>
		/// <param name="x">input value that will be clamped</param>
		/// <param name="min">lower limit</param>
		/// <param name="max">upper limit</param>
		/// <returns>clamped version of x</returns>
		public static double Clamp(this double x, double min, double max)
		{
			return Math.Min(max, Math.Max(min, x));
		}

		/// <summary>
		/// Clamp each component of the input vector v in between min and max. 
		/// </summary>
		/// <param name="v">input vector that will be clamped component-wise</param>
		/// <param name="min">lower limit</param>
		/// <param name="max">upper limit</param>
		/// <returns>clamped version of v</returns>
		public static Vector2 Clamp(this in Vector2 v, float min, float max)
		{
			return new Vector2(Clamp(v.X, min, max), Clamp(v.Y, min, max));
		}

		/// <summary>
		/// Clamp each component of the input vector v in between min and max. 
		/// </summary>
		/// <param name="v">input vector that will be clamped component-wise</param>
		/// <param name="min">lower limit</param>
		/// <param name="max">upper limit</param>
		/// <returns>clamped version of v</returns>
		public static Vector2 Clamp(this in Vector2 v, in Vector2 min, in Vector2 max)
		{
			return new Vector2(Clamp(v.X, min.X, max.X), Clamp(v.Y, min.Y, max.Y));
		}

		/// <summary>
		/// Clamp each component of the input vector v in between min and max. 
		/// </summary>
		/// <param name="v">input vector that will be clamped component-wise</param>
		/// <param name="min">lower limit</param>
		/// <param name="max">upper limit</param>
		/// <returns>clamped version of v</returns>
		public static Vector3 Clamp(this in Vector3 v, float min, float max)
		{
			return new Vector3(Clamp(v.X, min, max), Clamp(v.Y, min, max), Clamp(v.Z, min, max));
		}

		/// <summary>
		/// Clamp each component of the input vector v in between min and max. 
		/// </summary>
		/// <param name="v">input vector that will be clamped component-wise</param>
		/// <param name="min">lower limit</param>
		/// <param name="max">upper limit</param>
		/// <returns>clamped version of v</returns>
		public static Vector4 Clamp(this in Vector4 v, float min, float max)
		{
			return new Vector4(Clamp(v.X, min, max), Clamp(v.Y, min, max), Clamp(v.Z, min, max), Clamp(v.W, min, max));
		}

		/// <summary>
		/// Creates a rotation matrix.
		/// </summary>
		/// <param name="degrees">The angle of rotation in degrees.</param>
		/// <param name="axis">The axis of rotation.</param>
		/// <returns>The <seealso cref="Matrix4x4"/></returns>
		/// <exception cref="ArgumentException"></exception>
		public static Matrix4x4 CreateRotation(float degrees, Axis axis = Axis.Z)
		{
			var radians = DegreesToRadians(degrees);
			switch (axis)
			{
				case Axis.X: return Matrix4x4.CreateRotationX(radians);
				case Axis.Y: return Matrix4x4.CreateRotationY(radians);
				case Axis.Z: return Matrix4x4.CreateRotationZ(radians);
				default: throw new ArgumentException($"Unknown axis:{axis}");
			}
		}

		/// <summary>
		/// Calculates the determinant of the two vectors.
		/// </summary>
		/// <param name="a">Vector a.</param>
		/// <param name="b">Vector b.</param>
		/// <returns>The determinant</returns>
		public static float Determinant(in Vector2 a, in Vector2 b)
		{
			return a.X * b.Y - a.Y * b.X;
		}

		/// <summary>
		/// Returns the number of mipmap levels required for mipmapped filtering of an image.
		/// </summary>
		/// <param name="width">The image width in pixels.</param>
		/// <param name="height">The image height in pixels.</param>
		/// <returns>Number of mipmap levels</returns>
		public static int MipMapLevels(int width, int height)
		{
			return (int)Math.Log(Math.Max(width, height), 2.0) + 1;
		}

		/// <summary>
		/// Clock-wise normal to input vector.
		/// </summary>
		/// <param name="v">The input vector.</param>
		/// <returns>A vector normal to the input vector</returns>
		public static Vector2 CwNormalTo(this in Vector2 v)
		{
			return new Vector2(v.Y, -v.X);
		}

		/// <summary>
		/// Counter-clock-wise normal to input vector.
		/// </summary>
		/// <param name="v">The input vector.</param>
		/// <returns>A vector normal to the input vector</returns>
		public static Vector2 CcwNormalTo(this in Vector2 v)
		{
			return new Vector2(-v.Y, v.X);
		}

		/// <summary>
		/// Convert input uint from range [0,255] into float in range [0,1]
		/// </summary>
		/// <param name="v">input in range [0,255]</param>
		/// <returns>range [0,1]</returns>
		public static float Normalize(uint v)
		{
			return v / 255f;
		}

		/// <summary>
		/// Finds a range of existing indexes inside a sorted array that encompass a given value
		/// </summary>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="sorted">a sorted array of values</param>
		/// <param name="value">a value</param>
		/// <returns></returns>
		public static (int lower, int upper) FindExistingRange<TValue>(this TValue[] sorted, TValue value)
		{
			var ipos = Array.BinarySearch(sorted, value);
			if (ipos >= 0)
			{
				// exact target found at position "ipos"
				return (ipos, ipos);
			}
			else
			{
				// Exact key not found: BinarySearch returns negative when the 
				// exact target is not found, which is the bitwise complement 
				// of the next index in the list larger than the target.
				ipos = ~ipos;
				if (0 == ipos)
				{
					return (0, 0);
				}
				if (ipos < sorted.Length)
				{
					return (ipos - 1, ipos);
				}
				else
				{
					return (sorted.Length - 1, sorted.Length - 1);
				}
			}
		}
		/*
			var times = Enumerable.Range(-4, 10).Select(v => (float)v).ToArray();
			for (var v = -4f; v < 15; v += 0.25f)
			{
				var (lower, upper) = times.FindExistingRange(v);
				Console.WriteLine($"{v}: lower={times[lower]} higher={times[upper]}");
			}
		 */

		/// <summary>
		/// Transform the input value into the range [0..1]
		/// </summary>
		/// <param name="inputValue">the input value</param>
		/// <param name="inputMin">the lower input range bound</param>
		/// <param name="inputMax">the upper input range bound</param>
		/// <returns></returns>
		public static float Normalize(this float inputValue, float inputMin, float inputMax)
		{
			var inputRange = inputMax - inputMin;
			return float.Epsilon >= inputRange ? 0f : (inputValue - inputMin) / inputRange;
		}

		/// <summary>
		/// Normalizes each input uint from range [0,255] into float in range [0,1]
		/// </summary>
		/// <param name="x">input in range [0,255]</param>
		/// <param name="y">input in range [0,255]</param>
		/// <param name="z">input in range [0,255]</param>
		/// <param name="w">input in range [0,255]</param>
		/// <returns>vector with each component in range [0,1]</returns>
		public static Vector4 Normalize(uint x, uint y, uint z, uint w)
		{
			return new Vector4(x, y, z, w) / 255f;
		}

		/// <summary>
		/// Converts degrees to radians
		/// </summary>
		/// <param name="angle">input angle in degrees</param>
		/// <returns>input angle converted to radians</returns>
		public static float DegreesToRadians(float angle)
		{
			return (angle * TWO_PI) / 360.0f;
		}

		/// <summary>
		/// Converts radians to degrees
		/// </summary>
		/// <param name="angle">input angle in radians</param>
		/// <returns>input angle converted to degrees</returns>
		public static float RadiansToDegrees(float angle)
		{
			return (angle * 360.0f) / TWO_PI;
		}

		/// <summary>
		/// Linear interpolation of two known values a and b according to weight
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="weight">Interpolation weight</param>
		/// <returns>Linearly interpolated value</returns>
		public static float Lerp(float a, float b, float weight)
		{
			return a * (1 - weight) + b * weight;
		}

		/// <summary>
		/// Linear interpolation of two known values a and b according to weight
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="weight">Interpolation weight</param>
		/// <returns>Linearly interpolated value</returns>
		public static double Lerp(double a, double b, double weight)
		{
			return a * (1 - weight) + b * weight;
		}

		/// <summary>
		/// Linear interpolation of two points values a and b according to weight
		/// </summary>
		/// <param name="a">First point</param>
		/// <param name="b">Second point</param>
		/// <param name="weight">Interpolation weight</param>
		/// <returns>Linearly interpolated point</returns>
		public static Vector3 Lerp(in Vector3 a, in Vector3 b, float weight)
		{
			return a * (1 - weight) + b * weight;
		}

		/// <summary>
		/// Returns the integer part of the specified floating-point number. 
		/// Works not for constructs like <code>1f - float.epsilon</code> because this is outside of floating point precision
		/// </summary>
		/// <param name="x">Input floating-point number</param>
		/// <returns>The integer part.</returns>
		public static int FastTruncate(this float x) => (int)x;

		/// <summary>
		/// Returns for each component the integer part of the specified floating-point number. 
		/// Works not for constructs like <code>1f - float.epsilon</code> because this is outside of floating point precision
		/// </summary>
		/// <param name="value">Input floating-point vector</param>
		/// <returns>The integer parts.</returns>
		public static Vector2 Truncate(in Vector2 value) => new Vector2(value.X.FastTruncate(), value.Y.FastTruncate());

		/// <summary>
		/// Returns the largest integer less than or equal to the specified floating-point number.
		/// </summary>
		/// <param name="x">Input floating-point number</param>
		/// <returns>The largest integer less than or equal to x.</returns>
		public static float FloorF(this float x) => (float)Math.Floor(x);

		/// <summary>
		/// For each component returns the largest integer less than or equal to the specified floating-point number.
		/// </summary>
		/// <param name="v">Input vector</param>
		/// <returns>For each component returns the largest integer less than or equal to the specified floating-point number.</returns>
		public static Vector3 Floor(this in Vector3 v) => new Vector3(FloorF(v.X), FloorF(v.Y), FloorF(v.Z));

		/// <summary>
		/// Returns the value of x modulo y. This is computed as x - y * floor(x/y). 
		/// </summary>
		/// <param name="x">Dividend</param>
		/// <param name="y">Divisor</param>
		/// <returns>Returns the value of x modulo y.</returns>
		public static Vector3 Mod(this in Vector3 x, float y)
		{
			var div = x / y;
			return x - y * Floor(div);
		}

		/// <summary>
		/// packs normalized floating-point values into an unsigned integer.  
		/// </summary>
		/// <param name="v">Input normalized floating-point vector. Will be clamped</param>
		/// <returns>The first component of the vector will be written to the least significant bits of the output; 
		/// the last component will be written to the most significant bits.</returns>
		public static uint PackUnorm4x8(this in Vector4 v)
		{
			var r = Round(Clamp(v, 0.0f, 1.0f) * 255.0f);
			var x = (uint)r.X;
			var y = (uint)r.Y;
			var z = (uint)r.Z;
			var w = (uint)r.W;
			return (w << 24) + (z << 16) + (y << 8) + x;
		}

		/// <summary>
		/// Unpacks normalized floating-point values from an unsigned integer.
		/// </summary>
		/// <param name="i">Specifies an unsigned integer containing packed floating-point values.</param>
		/// <returns>The first component of the returned vector will be extracted from the least significant bits of the input; 
		/// the last component will be extracted from the most significant bits. </returns>
		public static Vector4 UnpackUnorm4x8(uint i)
		{
			var x = (i & 0x000000ff);
			var y = (i & 0x0000ff00) >> 8;
			var z = (i & 0x00ff0000) >> 16;
			var w = (i & 0xff000000) >> 24;
			var v = new Vector4(x, y, z, w);
			return v / 255.0f;
		}

		/// <summary>
		/// Rounds a floating-point value to the nearest integral value.
		/// </summary>
		/// <param name="f">A floating-point number to be rounded.</param>
		/// <returns>The integer nearest a. If the fractional component of a is halfway between two 
		/// integers, one of which is even and the other odd, then the even number is returned.
		/// Note that this method returns a System.Float instead of an integral type.</returns>
		public static float Round(this float f) => (float)Math.Round(f);

		/// <summary>
		/// Rounds each component of a floating-point vector (using MathHelper.Round) to the nearest integral value.
		/// </summary>
		/// <param name="v">A floating-point vector to be rounded component-wise.</param>
		/// <returns>Component-wise rounded vector</returns>
		public static Vector3 Round(this in Vector3 v) => new Vector3(Round(v.X), Round(v.Y), Round(v.Z));

		/// <summary>
		/// Rounds each component of a floating-point vector (using MathHelper.Round) to the nearest integral value.
		/// </summary>
		/// <param name="v">A floating-point vector to be rounded component-wise.</param>
		/// <returns>Component-wise rounded vector</returns>
		public static Vector4 Round(this in Vector4 v) => new Vector4(Round(v.X), Round(v.Y), Round(v.Z), Round(v.W));

		/// <summary>
		/// Converts given Cartesian coordinates into a polar angle.
		/// Returns an angle [-PI, PI].
		/// </summary>
		/// <param name="cartesian">Cartesian input coordinates</param>
		/// <returns>An angle [-PI, PI].</returns>
		public static float PolarAngle(this in Vector2 cartesian)
		{
			return (float)Math.Atan2(cartesian.Y, cartesian.X);
		}


		/// <summary>
		/// Converts a Vector to a array of float
		/// </summary>
		/// <param name="q">The input vector.</param>
		/// <returns></returns>
		public static float[] ToArray(this in Quaternion q) => new float[] { q.X, q.Y, q.Z, q.W };

		/// <summary>
		/// Converts a Vector to a array of float
		/// </summary>
		/// <param name="vector">The input vector.</param>
		/// <returns></returns>
		public static float[] ToArray(this in Vector2 vector) => new float[] { vector.X, vector.Y };

		/// <summary>
		/// Converts a Vector to a array of float
		/// </summary>
		/// <param name="vector">The input vector.</param>
		/// <returns></returns>
		public static float[] ToArray(this in Vector3 vector) => new float[] { vector.X, vector.Y, vector.Z };

		/// <summary>
		/// Converts a Vector to a array of float
		/// </summary>
		/// <param name="vector">The input vector.</param>
		/// <returns></returns>
		public static float[] ToArray(this in Vector4 vector) => new float[] { vector.X, vector.Y, vector.Z, vector.W };

		/// <summary>
		/// Converts the given polar coordinates to Cartesian.
		/// </summary>
		/// <param name="polar">The polar coordinates. A vector with first component angle [-PI, PI] and second component radius.</param>
		/// <returns>A Cartesian coordinate vector.</returns>
		public static Vector2 ToCartesian(this in Vector2 polar)
		{
			float x = polar.Y * (float)Math.Cos(polar.X);
			float y = polar.Y * (float)Math.Sin(polar.X);
			return new Vector2(x, y);
		}

		/// <summary>
		/// Converts given Cartesian coordinates into polar coordinates.
		/// Returns a vector with first component angle [-PI, PI] and second component radius.
		/// </summary>
		/// <param name="cartesian">Cartesian input coordinates</param>
		/// <returns>A vector with first component angle [-PI, PI] and second component radius.</returns>
		public static Vector2 ToPolar(this in Vector2 cartesian)
		{
			float angle = cartesian.PolarAngle();
			float radius = cartesian.Length();
			return new Vector2(angle, radius);
		}

		/// <summary>
		/// Returns the <seealso cref="Vector2"/> that results when dropping the z component from <seealso cref="Vector3"/>.
		/// </summary>
		/// <param name="vector">The <seealso cref="Vector3"/></param>
		/// <returns><seealso cref="Vector2"/></returns>
		public static Vector2 XY(this in Vector3 vector) => new Vector2(vector.X, vector.Y);

		/// <summary>
		/// Returns the <seealso cref="Vector2"/> that results when dropping the z and w component from <seealso cref="Vector4"/>.
		/// </summary>
		/// <param name="vector">The <seealso cref="Vector3"/></param>
		/// <returns><seealso cref="Vector2"/></returns>
		public static Vector2 XY(this in Vector4 vector) => new Vector2(vector.X, vector.Y);

		/// <summary>
		/// Returns the <seealso cref="Vector3"/> that results when dropping the w component from <seealso cref="Vector4"/>.
		/// </summary>
		/// <param name="vector">The <seealso cref="Vector4"/>.</param>
		/// <returns><seealso cref="Vector3"/></returns>
		public static Vector3 XYZ(this in Vector4 vector) => new Vector3(vector.X, vector.Y, vector.Z);
	}
}
