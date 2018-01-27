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
			renderState.Register(Update, BlendStates.Opaque);
			renderState.Register((c) => GL.ClearColor(c.Red, c.Green, c.Blue, c.Alpha), new ClearColorState(0f, 0f, 0f, 1f));
			renderState.Register((s) => Update(s.IsEnabled, EnableCap.DepthTest), new BoolState<IDepthState>(false));
			renderState.Register((s) => Update(s.IsEnabled, EnableCap.CullFace), new BoolState<IBackfaceCullingState>(false));
			renderState.Register((s) => Update(s.IsEnabled, EnableCap.ProgramPointSize), new BoolState<IShaderPointSizeState>(false));
			renderState.Register((s) => Update(s.IsEnabled, EnableCap.PointSprite), new BoolState<IPointSpriteState>(false));
			renderState.Register((s) => Update(s.IsEnabled, EnableCap.LineSmooth), new BoolState<ILineSmoothState>(false));
			return renderState;
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

		private static void Update(BlendState blendState)
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
				default: throw new ArgumentOutOfRangeException(blendOperator.ToString());
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
				default: throw new ArgumentOutOfRangeException(param.ToString());
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
				default: throw new ArgumentOutOfRangeException(param.ToString());
			}
		}

		private static void SetFunction(BlendParameter source, BlendParameter destination)
		{
			var parameterSource = ConvertSource(source);
			var parameterDestination = ConvertDestination(destination);
			GL.BlendFunc(parameterSource, parameterDestination);
		}
	}
}
