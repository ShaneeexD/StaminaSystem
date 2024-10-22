using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace StaminaSystem
{
    [BepInPlugin("StaminaSystem", "StaminaSystem", "1.0.0")]
    public class StaminaSystem : BasePlugin
    {
        public static ManualLogSource Logger;
        private Harmony harmony;

        public static ConfigEntry<float> staminaDrain;
        public static ConfigEntry<float> staminaRegain;
        public static ConfigEntry<float> staminaJumpDrain;
        public static ConfigEntry<float> minStamToRunJump;
        public static ConfigEntry<bool> staminaBar;

        public override void Load()
        {
            Logger = Log;
            NewGameHandler eventHandler = new NewGameHandler();
            Logger.LogInfo("Loading Stamina System...");

            staminaDrain = Config.Bind("Stamina Drain", "Drain Speed", -20.0f, "How fast stamina drains.");
            staminaRegain = Config.Bind("Stamina Regain", "Regain Speed", 20.0f, "How fast stamina regains.");
            staminaJumpDrain = Config.Bind("Stamina Jump Drain", "Jump Drain", -15.0f, "How much stamina drained by jumping.");
            minStamToRunJump = Config.Bind("Allowed Run/Jump At", "Minimum", 35.0f, "Minimum amount of stamina before allowing player to run/jump again.");
            staminaBar = Config.Bind("Stamina Bar", "Enabled", true, "Show's a visible stamina bar at the bottom of the screen.");

            try
            {
                harmony = new Harmony("LifeIsHard");
                harmony.PatchAll();
                Logger.LogInfo("All patches applied.");
            }

            catch (Exception ex)
            {
                Logger.LogError($"Error during Load: {ex}");
            }
        }
    }
}
