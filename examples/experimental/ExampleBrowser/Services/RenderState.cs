namespace ExampleBrowser.Services
{
	using System.ComponentModel.Composition;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	[Export(typeof(IRenderState)), PartCreationPolicy(CreationPolicy.NonShared)]
	class RenderState : IRenderState
	{
		private readonly Zenseless.HLGL.RenderState renderState;

		public RenderState()
		{
			renderState = RenderStateGL.Create();
		}

		public TYPE Get<TYPE>() where TYPE : struct, IState
		{
			return renderState.Get<TYPE>();
		}

		public void Set<TYPE>(in TYPE value) where TYPE : struct, IState
		{
			renderState.Set(value);
		}
	}
}
