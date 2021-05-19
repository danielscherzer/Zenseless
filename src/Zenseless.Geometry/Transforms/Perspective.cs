namespace Zenseless.Geometry
{
	using System;
	using System.Numerics;
	using Zenseless.Patterns;

	/// <summary>
	/// Implements a Perspective transformation that allows incremental changes
	/// </summary>
	/// <seealso cref="ITransformation" />
	public class Perspective : NotifyPropertyChanged, INotifyingTransform
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Perspective"/> class.
		/// </summary>
		/// <param name="fieldOfViewY">The field-of-view in y-direction.</param>
		/// <param name="aspect">The aspect ratio.</param>
		/// <param name="nearClip">The near clip plane distance.</param>
		/// <param name="farClip">The far clip plane distance.</param>
		public Perspective(float fieldOfViewY = 90f, float nearClip = 0.1f, float farClip = 1f, float aspect = 1f)
		{
			cachedMatrix = new DirtyFlag<Matrix4x4>(CalculateProjectionMatrix);
			PropertyChanged += (s, a) => cachedMatrix.Invalidate();
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
			get => _aspect;
			set => Set(ref _aspect, Math.Max(value, float.Epsilon));
		}

		/// <summary>
		/// Gets or sets the far clipping plane distance.
		/// </summary>
		/// <value>
		/// The far clipping plane distance.
		/// </value>
		public float FarClip
		{
			get => _farClip;
			set => Set(ref _farClip, Math.Max(value, NearClip));
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
			set => Set(ref _fieldOfViewY, MathHelper.Clamp(value, float.Epsilon, 179.9f));
		}

		/// <summary>
		/// Gets the transformation matrix in row-major style.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		public Matrix4x4 Matrix => cachedMatrix.Value;

		/// <summary>
		/// Gets or sets the near clipping plane distance.
		/// </summary>
		/// <value>
		/// The near clipping plane distance.
		/// </value>
		public float NearClip
		{
			get => _nearClip;
			set => Set(ref _nearClip, Math.Max(value, float.Epsilon));
		}

		private float _aspect = 1f;
		private float _farClip = 1f;
		private float _fieldOfViewY = 90f;
		private float _nearClip = 0.1f;
		private DirtyFlag<Matrix4x4> cachedMatrix;

		private Matrix4x4 CalculateProjectionMatrix()
		{
			var fov = MathHelper.DegreesToRadians(FieldOfViewY);
			return Matrix4x4.CreatePerspectiveFieldOfView(fov, Aspect, NearClip, FarClip);
		}
	}
}
