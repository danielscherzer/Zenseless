using System;
using System.Collections.Generic;
using Zenseless.Base;
using Zenseless.Geometry;

namespace SpaceInvadersMvc
{
	public class Model
	{
		public event EventHandler OnShoot;
		public event EventHandler OnEnemyDestroy;
		public event EventHandler OnLost;

		private GameTime time;
		private Box2D player = new Box2D(0.0f, -1.0f, 0.0789f, 0.15f);
		private List<Box2D> enemies = new List<Box2D>();
		private List<Box2D> bullets = new List<Box2D>();
		private PeriodicUpdate shootCoolDown = new PeriodicUpdate(0.1f);
		private float enemySpeed = 0.05f;

		public Model()
		{
			time = new GameTime();
			shootCoolDown.PeriodElapsed += (s, t) => shootCoolDown.Enabled = false;
			CreateEnemies();
		}

		public void Update(float axisUpDown, bool shoot)
		{
			if (Lost) return;
			shootCoolDown.Update(time.AbsoluteTime);
			//remove outside bullet - lazy remove (once per frame one bullet is removed)
			foreach (var bullet in bullets)
			{
				if (bullet.MinY > 1.0f)
				{
					bullets.Remove(bullet);
					break;
				}
			}
			HandleCollisions();

			UpdatePlayer(axisUpDown, shoot);
			MoveEnemies();
			MoveBullets();

			if (0 == enemies.Count && 0 == bullets.Count)
			{
				//game is won -> start new, but faster
				CreateEnemies();
				enemySpeed += 0.05f;
			}
		}

		public IEnumerable<IReadOnlyBox2D> Enemies { get { return enemies; } }

		public IEnumerable<IReadOnlyBox2D> Bullets { get { return bullets; } }

		public IReadOnlyBox2D Player { get { return player; } }

		private bool Lost { get; set; }

		private void CreateEnemies()
		{
			//create enemies
			for (float y = 0.1f; y < 1.0f; y += 0.2f)
			{

				for (float x = -0.85f; x < 0.9f; x += 0.2f)
				{
					enemies.Add(new Box2D(x, y, 0.06f, 0.1f));
				}
			}
		}
		
		private void UpdatePlayer(float axisUpDown, bool shoot)
		{
			player.MinX += time.DeltaTime * axisUpDown;
			//limit player position [left, right]
			player.MinX = Math.Min(1.0f - player.SizeX, Math.Max(-1.0f, player.MinX));

			if (shoot && !shootCoolDown.Enabled)
			{
				OnShoot?.Invoke(this, null);
				bullets.Add(new Box2D(player.MinX, player.MinY, 0.02f, 0.04f));
				bullets.Add(new Box2D(player.MaxX, player.MinY, 0.02f, 0.04f));
				shootCoolDown.Enabled = true;
			}
		}

		private void HandleCollisions()
		{
			//intersections
			foreach (var enemy in enemies)
			{
				if (enemy.MinY < - 0.8f)
				{
					//game lost
					Lost = true;
					if (!(OnLost is null)) OnLost(this, null);
				}
				foreach (var bullet in bullets)
				{
					if (bullet.Intersects(enemy))
					{
						//delete bullet and enemy
						OnEnemyDestroy?.Invoke(this, null);
						bullets.Remove(bullet);
						enemies.Remove(enemy);
						return;
					}
				}
			}
		}

		private void MoveEnemies()
		{
			foreach (var enemy in enemies)
			{
				enemy.MinY -= enemySpeed * time.DeltaTime;
			}
		}

		private void MoveBullets()
		{
			foreach (var bullet in bullets)
			{
				bullet.MinY += time.DeltaTime;
			}
		}
	}
}
