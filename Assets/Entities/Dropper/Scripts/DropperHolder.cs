using drillex.Common.Scripts;
using Godot;
using System;

namespace drillex.Assets.Entities.Dropper
{	
	public partial class DropperHolder : RefCounted, IUpgradable
	{
		private ulong _upgradePrice;

		public ulong UpgradePrice
		{
			get => _upgradePrice; 
			private set => _upgradePrice = value;
		}
		
		public Vector2I SpawnPosition;
		public float Delay;
		public float TimeElapsed;
		public bool IsBlocked;
		public bool CanMine;
		public int Level;

		public DropperHolder(Vector2I spawnPosition, float delay, bool isBlocked)
		{
			SpawnPosition = spawnPosition;
			Delay = delay;
			TimeElapsed = 0f;
			IsBlocked = isBlocked;
			Level = 1;
			UpgradePrice = 200;
		}
		
		public DropperHolder()
		{
			SpawnPosition = Vector2I.Zero;
			Delay = 0f;
			TimeElapsed = 0f;
			IsBlocked = false;
		}

		public string GetPriceText()
		{
			if (UpgradePrice == 0)
				return ("level MAX");
			return String.Format("Price: {0}\u20bf", UpgradePrice);
		}

		public bool Upgrade()
		{
			if (Level < 3)
			{
				Level++;
				switch (Level)
				{
					case 2:
						Delay = 2.0f;
						UpgradePrice = 1000;
						break;
					case 3:
						Delay = 1.0f;
						UpgradePrice = 0;
						break;
				}

				return true;
			}
			return false;
		}

		public string UpgradeText()
		{
			return String.Format("Type: {0}\nSpeed: {1}\n Level: {2}", "Dropper", (100/Delay).ToString(), Level.ToString());
		}
	}
}
