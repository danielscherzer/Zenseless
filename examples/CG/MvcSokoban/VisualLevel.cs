using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace MvcSokoban
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
			Add(ElementType.Box, "Crate_Brown", slice++);
			Add(ElementType.Goal, "EndPoint_Red", slice++);
			Add(ElementType.ManOnGoal, "EndPointCharacter", slice++);
			Add(ElementType.BoxOnGoal, "EndPointCrate_Brown", slice++);
			Add(ElementType.Wall, "Wall_Beige", slice++);

			var names = from item in tileTypes select item.Value.Item1;
			texArray = contentLoader.Load<ITexture2dArray>(names);
			shdTexColor = contentLoader.Load<IShaderProgram>("texColor.*");

			levelGeometry = new VAO(PrimitiveType.Quads);
			var quadPos = new Vector2[4] { Vector2.Zero, Vector2.UnitX, Vector2.One, Vector2.UnitY };
			var locPosition = shdTexColor.GetResourceLocation(ShaderResourceType.Attribute, "position");
			levelGeometry.SetAttribute(locPosition, quadPos, VertexAttribPointerType.Float, 2);

			locTexId = shdTexColor.GetResourceLocation(ShaderResourceType.Attribute, "texId");
			locCamera = shdTexColor.GetResourceLocation(ShaderResourceType.Uniform, "camera");
			locTint = shdTexColor.GetResourceLocation(ShaderResourceType.Uniform, "tint");
		}

		internal void ResizeWindow(int width, int height)
		{
			windowAspect = width / (float)height;
		}

		internal void DrawLevelState(ILevel levelState, Color tint)
		{
			UpdateLevelGeometry(levelState);
			shdTexColor.Activate();
			texArray.Activate();
			var fitBox = Box2DExtensions.CreateContainingBox(levelState.Width, levelState.Height, windowAspect);
			var camera = Matrix4x4.CreateOrthographicOffCenter(fitBox.MinX, fitBox.MaxX, fitBox.MinY, fitBox.MaxY, 0, 1);

			GL.Uniform4(locTint, tint);
			ShaderProgramGL.Uniform(locCamera, camera, true);

			levelGeometry.Draw(levelState.Width * levelState.Height);

			texArray.Deactivate();
			shdTexColor.Deactivate();
		}

		private void UpdateLevelGeometry(ILevel levelState)
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
					levelGeometry.SetAttribute(locInstanceTranslate, instanceTranslate.ToArray(), VertexAttribPointerType.Float, 2, true);
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
				levelGeometry.SetAttribute(locTexId, texId.ToArray(), VertexAttribPointerType.Float, 1, true);
			}
		}

		private ILevel lastLevelState;
		private Size lastLevelSize;
		private VAO levelGeometry;
		private IShaderProgram shdTexColor;
		private ITexture texArray;
		private Dictionary<ElementType, Tuple<string, int>> tileTypes = new Dictionary<ElementType, Tuple<string, int>>();
		private int locCamera;
		private int locTint;
		private int locTexId;
		private float windowAspect;
	}
}
