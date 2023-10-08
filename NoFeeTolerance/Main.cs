using DV.ServicePenalty;
using DV.ServicePenalty.UI;
using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;

namespace NoFeeTolerance
{
	[EnableReloading]
	public static class Main
	{
		public static bool enabled;
		public static UnityModManager.ModEntry? mod;
		public static Settings settings = new Settings();

		public static bool Load(UnityModManager.ModEntry modEntry)
		{
			var harmony = new Harmony(modEntry.Info.Id);
			harmony.PatchAll(Assembly.GetExecutingAssembly());
			DebugLog("Attempting Patch.");

			mod = modEntry;
			modEntry.OnToggle = OnToggle;
#if DEBUG || RELOAD
			modEntry.OnUnload = OnUnload;
#endif
			modEntry.OnGUI = OnGui;
			modEntry.OnSaveGUI = OnSaveGui;

			return true;
		}

		static void OnGui(UnityModManager.ModEntry modEntry)
		{
			settings.Draw(modEntry);
		}

		static void OnSaveGui(UnityModManager.ModEntry modEntry)
		{
			settings.Save(modEntry);
		}

		static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
		{
			if (value != enabled)
			{
				enabled = value;
			}
			return true;
		}

#if DEBUG || RELOAD
		static bool OnUnload(UnityModManager.ModEntry modEntry)
		{
			var harmony = new Harmony(modEntry.Info.Id);
			harmony.UnpatchAll(modEntry.Info.Id);
			DebugLog("Removing patch.");
			return true;
		}
#endif

		static void DebugLog(string message)
		{
			if (settings.isLoggingEnabled)
				mod?.Logger.Log(message);
		}

		[HarmonyPatch(typeof(CareerManagerDebtController), nameof(CareerManagerDebtController.FeeTolerance), MethodType.Getter)]
		static class FeeTolerancePatcher
		{
			static bool Prefix(ref float __result)
			{
				__result = float.PositiveInfinity;
				return false;
			}
		}
	}
}
