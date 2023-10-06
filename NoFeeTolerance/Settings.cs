using UnityModManagerNet;
using System;

namespace NoFeeTolerance
{
	public enum LoggingLevel
	{
		None = 0,
		Minimal = 1,
		Verbose = 2,
		Debug = 3,
	}

	public class Settings : UnityModManager.ModSettings, IDrawable
	{
		public readonly string? version = Main.mod?.Info.Version;

		[Draw("Enable logging")]
		public bool isLoggingEnabled =
#if DEBUG
			true;
#else
            false;
#endif
		override public void Save(UnityModManager.ModEntry entry)
		{
			Save<Settings>(this, entry);
		}

		public void OnChange() { }
	}
}
