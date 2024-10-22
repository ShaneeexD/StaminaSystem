using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StaminaSystem
{
    [HarmonyPatch(typeof(SessionData))]
    [HarmonyPatch("PauseGame")]
    public class PauseManager
    {
        public static void Prefix(ref bool showPauseText, ref bool delayOverride, ref bool openDesktopMode)
        {
                StaminaBar.isGamePaused = true;
        }
    }

    [HarmonyPatch(typeof(SessionData))]
    [HarmonyPatch("ResumeGame")]
    public class ResumeGameManager
    {
        public static void Prefix()
        {
            StaminaBar.isGamePaused = false;
        }
    }
}
