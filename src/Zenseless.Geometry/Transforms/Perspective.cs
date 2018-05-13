using System;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Implements a Perspective transformation
	/// </summary>
	/// <seealso cref="TransformationHierarchyNode" />
	public class Perspective : TransformationHierarchyNode
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Perspective"/> class.
		/// </summary>
		/// <param name="fieldOfViewY">The field-of-view in y-direction.</param>
		/// <param name="aspect">The aspect ratio.</param>
		/// <param name="nearClip">The near clip plane distance.</param>
		/// <param name="farClip">The far clip plane distance.</param>
		public Perspective(float fieldOfViewY = 90f, float nearClip = 0.1f, float farClip = 1f, float aspect = 1f) : base(null)
		{
			Aspect = aspect;
			FarClip = farClip;
			FieldOfViewY = fieldOfViewY;
			NearClip = nearClip;
		}

		/// <summary>
		/// Gets or sets the aspect ratio.
		/// </summary>
		/// <value>
		/// The aspect ratio.
		/// </value>
		public float Aspect
		{
			get => _aspect; set
			{
				_aspect = Math.Max(value, float.Epsilon);
				UpdateMatrix();
			}
		}

		/// <summary>
		/// Gets or sets the far clipping plane distance.
		/// </summary>
		/// <value>
		/// The far clipping plane distance.
		/// </value>
		public float FarClip
		{
			get => _farClip; set
			{
				_farClip = Math.Max(value, NearClip);
				UpdateMatrix();
			}
		}

		/// <summary>
		/// Gets or sets the field-of-view y.
		/// </summary>
		/// <value>
		/// The field-of-view y.
		/// </value>
		public float FieldOfViewY
		{
			get => _fieldOfViewY;
			set
			{
				_fieldOfViewY = MathHelper.Clamp(value, float.Epsilon, 179.9f);
				UpdateMatrix();
			}
		}

		/// <summary>
		/// Gets or sets the near clipping plane distance.
		/// </summary>
		/// <value>
		/// The near clipping plane distance.
		/// </value>
		public float NearClip
		{
			get => _nearClip; set
			{
				_nearClip = Math.Max(value, float.Epsilon);
				UpdateMatrix();
			}
		}

		private float _aspect = 1f;
		private float _farClip = 1f;
		private float _fieldOfViewY = 90f;
		private float _nearClip = 0.1f;

		private void UpdateMatrix()
		{
			var fov = MathHelper.DegreesToRadians(FieldOfViewY);
			matrix = Matrix4x4.CreatePerspectiveFieldOfView(fov, Aspect, NearClip, FarClip);
		}
	}
}
