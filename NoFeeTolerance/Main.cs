using DV.ServicePenalty;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityModManagerNet;

namespace NoFeeTolerance
{
	public static class Main
	{
		public static bool enabled;
		public static UnityModManager.ModEntry mod;
		public static Settings settings;

		private static Harmony harmony;
		private static bool Load(UnityModManager.ModEntry modEntry)
		{
			Harmony? harmony = null;

			try
			{
				harmony = new Harmony(modEntry.Info.Id);
				harmony.PatchAll(Assembly.GetExecutingAssembly());

				// Other plugin startup logic
			}
			catch (Exception ex)
			{
				modEntry.Logger.LogException($"Failed to load {modEntry.Info.DisplayName}:", ex);
				harmony?.UnpatchAll(modEntry.Info.Id);
				return false;
			}

			return true;
		}

		static void DebugLog(string message)
		{
			if (settings.isLoggingEnabled)
			{
				mod.Logger.Log(message);
			}
		}

		private static float GetTotalDebtExitingLocos(DisplayableDebt debt)
		{
			if (debt is ExistingLocoDebt)
			{
				DebugLog($"Loco {debt.ID} still exists, ignoring its fees.");
				return 0;
			}
			return debt.GetTotalPrice();
		}

		[HarmonyPatch(typeof(CareerManagerDebtController), "IsPlayerAllowedToTakeJob")]

		static class IsPlayerAllowedToTakeJobPatch
		{
			static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				return instructions.MethodReplacer(
					typeof(DisplayableDebt).GetMethod("GetTotalPrice"),
					typeof(Main).GetMethod("GetTotalDebtForJobPurposes"));
			}
		}
	}
}
