using UnityModManagerNet;

namespace NoFeeTolerance
{
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
		public override void Save(UnityModManager.ModEntry entry)
		{
			Save(this, entry);
		}

		public void OnChange() { }
	}
}
