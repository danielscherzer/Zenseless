using System;
using OpenTK.Graphics.OpenGL4;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Implements blending for OpenGL
	/// </summary>
	public class BlendStateGL
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlendStateGL"/> class.
		/// </summary>
		public BlendStateGL(): this(BlendStates.Opaque)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BlendStateGL"/> class.
		/// </summary>
		/// <param name="blendState">State of the blend.</param>
		public BlendStateGL(BlendState blendState)
		{
			this.blendState = blendState;
			Update();
		}

		/// <summary>
		/// Gets or sets the current blend state.
		/// </summary>
		/// <value>
		/// The current blend state.
		/// </value>
		public BlendState BlendState
		{
			get => blendState;
			set
			{
				if (value == blendState) return;
				blendState = value;
				Update();
			}
		}

		private BlendState blendState;

		private void SetOperator(BlendOperator blendOperator)
		{
			switch(blendOperator)
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

		private BlendingFactorSrc ConvertSource(BlendParameter param)
		{
			switch(param)
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

		private BlendingFactorDest ConvertDestination(BlendParameter param)
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

		private void SetFunction(BlendParameter source, BlendParameter destination)
		{
			var parameterSource = ConvertSource(source);
			var parameterDestination = ConvertDestination(destination);
			GL.BlendFunc(parameterSource, parameterDestination);
		}

		private void Update()
		{
			SetOperator(blendState.BlendOperator);
			SetFunction(blendState.BlendParameterSource, blendState.BlendParameterDestination);
		}
	}
}
