using OpenTK.Graphics.OpenGL;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Numerics;
using Zenseless.Patterns;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace ExampleBrowser
{
	using Line = Tuple<Vector2, Vector2>;

	[ExampleDisplayName]
	[Export(typeof(IExample))]
	class AABBRotationExample : NotifyPropertyChanged, IExample
	{
		[ImportingConstructor]
		public AABBRotationExample([Import] IRenderState renderState, [Import] ITime time)
		{
			this.Time = time ?? throw new ArgumentNullException(nameof(time));
			if (renderState == null)
			{
				throw new ArgumentNullException(nameof(renderState));
			}
			renderState.Set(BlendStates.AlphaBlend);
			renderState.Set(new LineSmoothing(true));
			renderState.Set(new ClearColorState(1, 1, 1, 1));
			renderState.Set(new LineWidth(5f));
		}

		public void Render()
		{
			var newStick = RotateLine(stick, Angle);
			var stickAABB = Box2DExtensions.CreateFromPoints(new Vector2[] { newStick.Item1, newStick.Item2 });

			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Color3(Color.CornflowerBlue);
			DrawLine(newStick);

			GL.Color3(Color.YellowGreen);
			DrawAABB(stickAABB);

			GL.Color3(Color.Black);
			DrawAABB(Box2DExtensions.CreateFromCenterSize(0, 0, 0.02f, 0.02f));
		}

		[Description("The angle of rotation in degrees")]
		public float Angle
		{
			get => _angle;
			set => SetNotify(ref _angle, value);
		}

		private ITime Time { get; }

		public void Update()
		{
			Angle = 30f * Time.AbsoluteTime;
		}

		private const float size = 0.7f;
		private readonly Line stick = new Line(new Vector2(-size, -size), new Vector2(size, size));
		private float _angle = 0f;

		private static Line RotateLine(Line stick, float rotationAngleDegrees)
		{
			var mtxRotation = Matrix3x2.CreateRotation(MathHelper.DegreesToRadians(rotationAngleDegrees));
			var a = Vector2.Transform(stick.Item1, mtxRotation);
			var b = Vector2.Transform(stick.Item2, mtxRotation);
			return new Line(a, b);
		}

		private static void DrawLine(in Line stick)
		{
			GL.Begin(PrimitiveType.Lines);
			GL.Vertex2(stick.Item1.X, stick.Item1.Y);
			GL.Vertex2(stick.Item2.X, stick.Item2.Y);
			GL.End();
		}

		private static void DrawAABB(IReadOnlyBox2D rect)
		{
			GL.Begin(PrimitiveType.LineLoop);
			GL.Vertex2(rect.MinX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.Vertex2(rect.MinX, rect.MaxY);
			GL.End();
		}
	}
}