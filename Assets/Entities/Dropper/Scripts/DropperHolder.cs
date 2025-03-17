using Godot;
using System;
namespace drillex.Assets.Entities.Dropper
{	
	public partial class DropperHolder : GodotObject
	{
		public Vector2I SpawnPosition;
		public float Delay;
		public float TimeElapsed;
		public bool IsBlocked;
		
		public DropperHolder(Vector2I spawnPosition, float delay, bool isBlocked)
		{
			SpawnPosition = spawnPosition;
			Delay = delay;
			TimeElapsed = 0f;
			IsBlocked = isBlocked;
		}
		
		public DropperHolder()
		{
			SpawnPosition = Vector2I.Zero;
			Delay = 0f;
			TimeElapsed = 0f;
			IsBlocked = false;
		}
	}
}
