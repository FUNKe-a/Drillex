using Godot;
using System;
namespace drillex.Assets.Entities.Dropper
{	
	public partial class DropperHolder : GodotObject
	{
		public Vector2I SpawnPosition;
		public Vector2I MapPosition;
		public float Delay;
		public float TimeElapsed;
		public bool IsBlocked;
		
		public DropperHolder(Vector2I spawnPosition, Vector2I mapPosition, float delay, bool isBlocked)
		{
			SpawnPosition = spawnPosition;
			MapPosition = mapPosition;
			Delay = delay;
			TimeElapsed = 0f;
			IsBlocked = isBlocked;
		}
		public DropperHolder(Vector2I mapPosition)
		{
			SpawnPosition = Vector2I.Zero;
			MapPosition = mapPosition;
			Delay = 0f;
			TimeElapsed = 0f;
			IsBlocked = false;
		}
		public DropperHolder()
		{
			MapPosition = Vector2I.Zero;
			SpawnPosition = Vector2I.Zero;
			Delay = 0f;
			TimeElapsed = 0f;
			IsBlocked = false;
		}

		public override bool Equals(object obj)
		{
			if (obj is DropperHolder holder)
			{
				return MapPosition == holder.MapPosition;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return MapPosition.GetHashCode();
		}
	}
}
