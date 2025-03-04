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
		
		public DropperHolder(Vector2I spawnPosition, Vector2I mapPosition, float delay)
		{
			SpawnPosition = spawnPosition;
			MapPosition = mapPosition;
			Delay = delay;
			TimeElapsed = 0f;
		}
		public DropperHolder(Vector2I mapPosition)
		{
			SpawnPosition = Vector2I.Zero;
			MapPosition = mapPosition;
			Delay = 0f;
			TimeElapsed = 0f;
		}
		public DropperHolder()
		{
			SpawnPosition = Vector2I.Zero;
			Delay = 0f;
			TimeElapsed = 0f;
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
