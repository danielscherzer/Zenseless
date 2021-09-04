using OpenTK.Graphics.OpenGL4;
using System;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	public static class RenderStateGL
	{
		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns></returns>
		public static RenderState Create()
		{
			var renderState = new RenderState();

			int[] vp = new int[4];
			GL.GetInteger(GetPName.Viewport, vp);
			renderState.Register((o, s) => GL.Viewport(s.X, s.Y, s.Width, s.Height), new Viewport(vp[0], vp[1], vp[2], vp[3]));
			renderState.Register(Update, BlendStates.Opaque);
			renderState.Register((old, s) => GL.ClearColor(s.Red, s.Green, s.Blue, s.Alpha), new ClearColorState(0f, 0f, 0f, 1f));
			renderState.Register((old, s) => Update(s.Enabled, EnableCap.DepthTest), new DepthTest(false));
			renderState.Register((old, s) => Update(s.Mode), new FaceCullingModeState(FaceCullingMode.NONE));
			renderState.Register((old, s) => Update(s.Enabled, EnableCap.ProgramPointSize), new ShaderPointSize(false));
			renderState.Register((old, s) => Update(s.Enabled, EnableCap.PointSprite), new PointSprite(false));
			renderState.Register((old, s) => Update(s.Enabled, EnableCap.LineSmooth), new LineSmoothing(false));
			renderState.Register((old, s) => GL.LineWidth(s.Value), new LineWidth(1f));
			renderState.Register((old, s) => Update(s.ShaderProgram), new ActiveShader(null));
			renderState.Register((old, s) => Update(old.RenderSurface, s.RenderSurface), new ActiveRenderSurface(null));
			return renderState;
		}

		private static void Update(IRenderSurface oldRenderSurface, IRenderSurface renderSurface)
		{
			oldRenderSurface?.Deactivate();
			//TODO: maybe view port and 0 buffer, but should be done by deactivate
			renderSurface?.Activate();
		}

		private static void Update(IShaderProgram shaderProgram)
		{
			if (shaderProgram is null)
			{
				GL.UseProgram(0);
			}
			else
			{
				shaderProgram.Activate();
			}
		}

		private static void Update(bool enabled, EnableCap cap)
		{
			if (enabled)
			{
				GL.Enable(cap);
			}
			else
			{
				GL.Disable(cap);
			}
		}

		private static void Update(FaceCullingMode mode)
		{
			switch (mode)
			{
				case FaceCullingMode.FRONT_SIDE:
					GL.Enable(EnableCap.CullFace);
					GL.CullFace(CullFaceMode.Front);
					break;
				case FaceCullingMode.BACK_SIDE:
					GL.Enable(EnableCap.CullFace);
					GL.CullFace(CullFaceMode.Back);
					break;
				default:
					GL.Disable(EnableCap.CullFace);
					break;

			};
		}

		private static void Update(BlendState oldBlendState, BlendState blendState)
		{
			SetOperator(blendState.BlendOperator);
			SetFunction(blendState.BlendParameterSource, blendState.BlendParameterDestination);
		}

		private static void SetOperator(BlendOperator blendOperator)
		{
			switch (blendOperator)
			{
				case BlendOperator.None: GL.Disable(EnableCap.Blend); return;
				case BlendOperator.Add: GL.BlendEquation(BlendEquationMode.FuncAdd); break;
				case BlendOperator.Max: GL.BlendEquation(BlendEquationMode.Max); break;
				case BlendOperator.Min: GL.BlendEquation(BlendEquationMode.Min); break;
				case BlendOperator.ReverseSubtract: GL.BlendEquation(BlendEquationMode.FuncReverseSubtract); break;
				case BlendOperator.Subtract: GL.BlendEquation(BlendEquationMode.FuncSubtract); break;
				default: throw new ArgumentException(blendOperator.ToString());
			}
			GL.Enable(EnableCap.Blend);
		}

		private static BlendingFactorSrc ConvertSource(BlendParameter param)
		{
			switch (param)
			{
				case BlendParameter.DestinationAlpha: return BlendingFactorSrc.DstAlpha;
				case BlendParameter.DestinationColor: return BlendingFactorSrc.DstColor;
				case BlendParameter.One: return BlendingFactorSrc.One;
				case BlendParameter.OneMinusDestinationAlpha: return BlendingFactorSrc.OneMinusDstAlpha;
				case BlendParameter.OneMinusDestinationColor: return BlendingFactorSrc.OneMinusDstColor;
				case BlendParameter.OneMinusSourceAlpha: return BlendingFactorSrc.OneMinusSrcAlpha;
				case BlendParameter.OneMinusSourceColor: return BlendingFactorSrc.OneMinusSrcColor;
				case BlendParameter.SourceAlpha: return BlendingFactorSrc.SrcAlpha;
				case BlendParameter.SourceAlphaSaturation: return BlendingFactorSrc.SrcAlphaSaturate;
				case BlendParameter.SourceColor: return BlendingFactorSrc.SrcColor;
				case BlendParameter.Zero: return BlendingFactorSrc.Zero;
				default: throw new ArgumentException(param.ToString());
			}
		}

		private static BlendingFactorDest ConvertDestination(BlendParameter param)
		{
			switch (param)
			{
				case BlendParameter.DestinationAlpha: return BlendingFactorDest.DstAlpha;
				case BlendParameter.DestinationColor: return BlendingFactorDest.DstColor;
				case BlendParameter.One: return BlendingFactorDest.One;
				case BlendParameter.OneMinusDestinationAlpha: return BlendingFactorDest.OneMinusDstAlpha;
				case BlendParameter.OneMinusDestinationColor: return BlendingFactorDest.OneMinusDstColor;
				case BlendParameter.OneMinusSourceAlpha: return BlendingFactorDest.OneMinusSrcAlpha;
				case BlendParameter.OneMinusSourceColor: return BlendingFactorDest.OneMinusSrcColor;
				case BlendParameter.SourceAlpha: return BlendingFactorDest.SrcAlpha;
				case BlendParameter.SourceAlphaSaturation: return BlendingFactorDest.SrcAlphaSaturate;
				case BlendParameter.SourceColor: return BlendingFactorDest.SrcColor;
				case BlendParameter.Zero: return BlendingFactorDest.Zero;
				default: throw new ArgumentException(param.ToString());
			}
		}

		private static void SetFunction(BlendParameter source, BlendParameter destination)
		{
			var parameterSource = ConvertSource(source);
			var parameterDestination = ConvertDestination(destination);
			GL.BlendFunc(0, parameterSource, parameterDestination);
		}
	}
}
