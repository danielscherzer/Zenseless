using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MvcSokoban
{
	[Serializable]
	public class LevelLogic
	{
		private Point playerPos;
		private List<Level> levelStates = new List<Level>();

		public LevelLogic(Level level)
		{
			levelStates.Add(level);
			SetPlayerPos();
		}

		public ILevel GetLevelState() { return levelStates.Last(); }
		public int Moves { get { return levelStates.Count - 1; } }

		public void Undo()
		{
			if (levelStates.Count > 1)
			{
				levelStates.RemoveAt(levelStates.Count - 1);
				SetPlayerPos();
			}
		}

		public void Update(Movement movement)
		{
			UpdateMovables(movement);
		}

		private void SetPlayerPos()
		{
			var playerPos = GetLevelState().FindPlayerPos();
			if (playerPos.HasValue)
			{
				this.playerPos = playerPos.Value;
			}
		}

		private void UpdateMovables(Movement movement)
		{
			var newPlayerPos = playerPos;
			newPlayerPos = newPlayerPos.Move(movement);
			var newPlayerPosTileType = GetLevelState().GetElement(newPlayerPos.X, newPlayerPos.Y);
			if (ElementType.Wall == newPlayerPosTileType) return;
			var newLevelState = levelStates.Last().Copy();
			if (newPlayerPosTileType.ContainsBox())
			{
				//player tries to move a box
				var newBoxPos = newPlayerPos.Move(movement);
				var newBoxPosTileType = GetLevelState().GetElement(newBoxPos.X, newBoxPos.Y);
				//is new box position invalid
				if (ElementType.Floor != newBoxPosTileType && ElementType.Goal != newBoxPosTileType) return;
				//move box
				newLevelState.MoveBox(newPlayerPos, newBoxPos);
			}
			var oldPlayerPos = playerPos;
			playerPos = newPlayerPos;
			newLevelState.MovePlayer(oldPlayerPos, playerPos);
			//add new state to undo
			levelStates.Add(newLevelState);
		}
	}
}
