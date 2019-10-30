using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	internal class VisualLevel
	{
		public VisualLevel(IContentLoader contentLoader)
		{
			void Add(ElementType elementType, string resourceName, int sliceID)
			{
				tileTypes.Add(elementType, new Tuple<string, int>(resourceName, sliceID));
			}

			int slice = 0;
			Add(ElementType.Floor, "GroundGravel_Grass", slice++);
			Add(ElementType.Man, "character4", slice++);
			Add(ElementType.Box, ".Crate_Brown", slice++);
			Add(ElementType.Goal, "EndPoint_Red", slice++);
			Add(ElementType.ManOnGoal, "EndPointCharacter", slice++);
			Add(ElementType.BoxOnGoal, "EndPointCrate_Brown", slice++);
			Add(ElementType.Wall, "Wall_Beige", slice++);

			var names = from item in tileTypes select item.Value.Item1;
			texArray = contentLoader.Load<ITexture2dArray>(names);
			shdTexColor = contentLoader.Load<IShaderProgram>("texColor.*");

			vaoLevelGeometry = new VAO(PrimitiveType.Quads);
			var quadPos = new Vector2[4] { Vector2.Zero, Vector2.UnitX, Vector2.One, Vector2.UnitY };
			var locPosition = shdTexColor.GetResourceLocation(ShaderResourceType.Attribute, "position");
			vaoLevelGeometry.SetAttribute(locPosition, quadPos);
		}

		internal void ResizeWindow(int width, int height)
		{
			windowAspect = width / (float)height;
		}

		internal void DrawLevelState(ILevelGrid levelState, Color tint)
		{
			UpdateLevelGeometry(levelState);
			shdTexColor.Activate();
			texArray.Activate();
			var fitBox = Box2DExtensions.CreateContainingBox(levelState.Width, levelState.Height, windowAspect);
			var camera = Matrix4x4.CreateOrthographicOffCenter(fitBox.MinX, fitBox.MaxX, fitBox.MinY, fitBox.MaxY, 0, 1);

			shdTexColor.Uniform(nameof(tint), tint.ToVector4());
			shdTexColor.Uniform(nameof(camera), camera, true);

			vaoLevelGeometry.Draw(levelState.Width * levelState.Height);

			texArray.Deactivate();
			shdTexColor.Deactivate();
		}

		private void UpdateLevelGeometry(ILevelGrid levelState)
		{
			if (!ReferenceEquals(levelState, lastLevelState))
			{
				//move or different level size
				lastLevelState = levelState;
				var size = new Size(levelState.Width, levelState.Height);
				if (lastLevelSize != size)
				{
					//different level size -> create translates for tiles
					var instanceTranslate = new List<Vector2>();
					for (int x = 0; x < levelState.Width; ++x)
					{
						for (int y = 0; y < levelState.Height; ++y)
						{
							instanceTranslate.Add(new Vector2(x, y));
						}
					}
					var locInstanceTranslate = shdTexColor.GetResourceLocation(ShaderResourceType.Attribute, "instanceTranslate");
					vaoLevelGeometry.SetAttribute(locInstanceTranslate, instanceTranslate.ToArray(), true);
					lastLevelSize = size;
				}
				//update all tile types
				var texId = new List<float>();
				for (int x = 0; x < levelState.Width; ++x)
				{
					for (int y = 0; y < levelState.Height; ++y)
					{
						var type = levelState.GetElement(x, y);
						texId.Add(tileTypes[type].Item2);
					}
				}
				var locTexId = shdTexColor.GetResourceLocation(ShaderResourceType.Attribute, "texId");
				vaoLevelGeometry.SetAttribute(locTexId, texId.ToArray(), true);
				drawLevel = DrawableGL.CreateDrawCallGL(PrimitiveType.Quads, 4, levelState.Width * levelState.Height, vaoLevelGeometry);
			}
		}

		private ILevelGrid lastLevelState;
		private Size lastLevelSize;
		private VAO vaoLevelGeometry;
		private Action drawLevel;
		private IShaderProgram shdTexColor;
		private ITexture texArray;
		private Dictionary<ElementType, Tuple<string, int>> tileTypes = new Dictionary<ElementType, Tuple<string, int>>();
		private float windowAspect;
	}
}
