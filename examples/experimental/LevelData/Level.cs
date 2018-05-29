using System;
using System.Collections.Generic;
using Zenseless.Patterns;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace LevelData
{
	public interface INamed
	{
		string Name { get; }
	}

	[Serializable]
	public class ColliderCircle : INamed
	{
		public ColliderCircle(string name, Circle bounds)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Bounds = bounds ?? throw new ArgumentNullException(nameof(bounds));
		}

		public Circle Bounds { get; set; }
		public string Name { get; set; }
	}

	[Serializable]
	public class Sprite : Disposable, INamed
	{
		public Sprite(string name, IReadOnlyBox2D renderBounds, int layer, NamedStream namedStream)
		{
			Layer = layer;
			NamedStream = namedStream ?? throw new ArgumentNullException(nameof(namedStream));
			Name = name;
			RenderBounds = renderBounds ?? throw new ArgumentNullException(nameof(renderBounds));
		}

		public int Layer { get; set; }
		public NamedStream NamedStream { get; set; }
		public string Name { get; set; }
		public IReadOnlyBox2D RenderBounds { get; set; }

		protected override void DisposeResources()
		{
			NamedStream.Dispose();
		}
	}

	[Serializable]
	public class Level : Disposable
	{
		public Box2D Bounds = new Box2D(Box2D.BOX01);

		public void Add(ColliderCircle collider)
		{
			colliders.Add(collider);
		}

		protected override void DisposeResources()
		{
			foreach (var sprite in Sprites) sprite.Dispose();
		}

		public List<Sprite> Sprites = new List<Sprite>();
		public List<ColliderCircle> colliders = new List<ColliderCircle>();
	}
}
