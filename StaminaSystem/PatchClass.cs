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
    [BepInPlugin("StaminaSystem", "StaminaSystem", "1.1.0")]
    public class StaminaSystem : BasePlugin
    {
        public static ManualLogSource Logger;
        private Harmony harmony;

        public static ConfigEntry<float> staminaDrain;
        public static ConfigEntry<float> staminaRegain;
        public static ConfigEntry<float> staminaJumpDrain;
        public static ConfigEntry<float> minStamToRunJump;
        public static ConfigEntry<bool> staminaBar;
        public static ConfigEntry<float> staminaDrainMeleeFist;
        public static ConfigEntry<float> staminaDrainMeleeWeapon;
        public static ConfigEntry<float> staminaDrainBlock;
        public static ConfigEntry<float> staminaDrainThrowables;
        public static ConfigEntry<float> minStamToAttack;

        public override void Load()
        {
            Logger = Log;
            NewGameHandler eventHandler = new NewGameHandler();
            Logger.LogInfo("Loading Stamina System...");

            staminaDrainMeleeFist = Config.Bind("Stamina Melee Drain (Fists)", "Drain Amount", -40.0f, "How much stamina drained by melee attacks with fists.");
            staminaDrainMeleeWeapon = Config.Bind("Stamina Melee Drain (Weapons)", "Drain Amount", -55.0f, "How much stamina drained by melee attacks with a weapon.");
            staminaDrainThrowables = Config.Bind("Stamina Throwables Drain", "Drain Amount", -50.0f, "How much stamina drained by throwing certain items.");
            staminaDrainBlock = Config.Bind("Stamina Blocking Drain", "Drain Amount", -10.0f, "How much stamina drained by blocking.");
            staminaDrain = Config.Bind("Stamina Drain (Sprinting)", "Drain Speed", -20.0f, "How fast stamina drains while sprinting.");
            staminaRegain = Config.Bind("Stamina Regain", "Regain Speed", 20.0f, "How fast stamina regains.");
            staminaJumpDrain = Config.Bind("Stamina Jump Drain", "Jump Drain Amount", -15.0f, "How much stamina drained by jumping.");
            minStamToRunJump = Config.Bind("Allowed Run/Jump At", "Minimum", 35.0f, "Minimum amount of stamina before allowing player to run/jump again.");
            minStamToAttack = Config.Bind("Allowed Attack At", "Minimum", 1.0f, "Minimum amount of stamina before allowing player to attack.");
            staminaBar = Config.Bind("Stamina Bar", "Enabled", true, "Show's a visible stamina bar at the bottom of the screen.");

            try
            {
                harmony = new Harmony("StaminaSystem");
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
