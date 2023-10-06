using UnityModManagerNet;

namespace NoFeeTolerance
{
	public class Settings : UnityModManager.ModSettings, IDrawable
	{
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
