namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class View
	{
		public View(IRenderContext context, IContentLoader contentLoader)
		{
			texTank = contentLoader.Load<ITexture2D>("tank");
			shaderColorTexture = contentLoader.Load<IShaderProgram>("ColorTexture.*");

			context.RenderState.Set(BlendStates.AlphaBlend);
			context.RenderState.Set(new LineSmoothing(true));
			context.RenderState.Set(new LineWidth(2f));

			GL.Color3(1f, 1f, 1f);
		}

		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			GL.Translate(-1.0f, -1.0f, 0.0f);
			GL.Scale(2.0f, 2.0f, 1.0f);
		}

		public void DrawCircle(IReadOnlyCircle circle)
		{
			DrawTools.DrawCircle(circle.CenterX, circle.CenterY, circle.Radius, 32, false);
		}

		public void DrawRect(in Vector2 cornerA, in Vector2 cornerB)
		{
			var a = (cornerA);
			var b = (cornerB);
			GL.Begin(PrimitiveType.LineLoop);
				GL.Vertex2(a.X, a.Y);
				GL.Vertex2(b.X, a.Y);
				GL.Vertex2(b.X, b.Y);
				GL.Vertex2(a.X, b.Y);
			GL.End();
		}

		public void DrawRect(IReadOnlyBox2D box)
		{
			var a = new Vector2(box.MinX, box.MinY);
			var b = new Vector2(box.MaxX, box.MaxY);
			GL.Begin(PrimitiveType.LineLoop);
			GL.Vertex2(a.X, a.Y);
			GL.Vertex2(b.X, a.Y);
			GL.Vertex2(b.X, b.Y);
			GL.Vertex2(a.X, b.Y);
			GL.End();
		}

		public void DrawUnit(IUnit unit)
		{
			var box = Box2DExtensions.CreateFromCircle(unit.Bounds);
			DrawSprite(box, unit.Orientation, texTank);
			if (unit.Selected)
			{
				DrawCircle(unit.Bounds);
			}
			if(unit.IsMoving)
			{
				GL.Color3(1f, 0f, 0f);
				DrawRect(box);
				GL.Color3(1f, 1f, 1f);
			}
		}

		public Vector2 TransformToModel(in Vector2 clip)
		{
			return viewport2Model.Transform(clip);
		}

		private void DrawSprite(IReadOnlyBox2D extends, float orientation, ITexture texture)
		{
			shaderColorTexture.Activate();
			var cX = extends.CenterX;
			var cY = extends.CenterY;
			texture.Activate();
			GL.PushMatrix();
			{
				GL.Translate(cX, cY, 0);
				GL.Scale(extends.SizeX, extends.SizeY, 1.0f);
				GL.Rotate(orientation, 0, 0, 1);
				GL.Translate(-0.5f, -0.5f, 0.0f);
				DrawTools.DrawTexturedRect(Box2D.BOX01, Box2D.BOX01);
			}
			GL.PopMatrix();
			texture.Deactivate();
			shaderColorTexture.Deactivate();
		}

		private readonly Transformation viewport2Model = Transformation.Combine(Transformation.Translation(1f, 1f), Transformation.Scale(.5f));
		private readonly ITexture2D texTank;
		private readonly IShaderProgram shaderColorTexture;
	}
}
