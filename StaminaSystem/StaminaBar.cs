using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System.Reflection;
using static Il2CppSystem.Linq.Expressions.Interpreter.NullableMethodCallInstruction;

namespace StaminaSystem
{
    [HarmonyPatch(typeof(Player), "Update")]
    public class StaminaBar
    {
        public static bool staminaBarCreated;
        private static bool startedRunningOverStam;

        public static bool isGamePaused = false;
        public static bool isGameLoaded = false;
        public static bool isPlayerJumping;


        public static Image staminaFill;
        public static Color currentColor;
        public static float maxStamina = 100f;
        public static float currentStamina;
        public static RectTransform staminaFillRect;

        private static PlayerInfoProvider playerInfo;
        private static FirstPersonController control;

        [HarmonyPrefix]
        public static void Prefix(Player __instance)
        {
                if (!staminaBarCreated)
                {
                    playerInfo = new PlayerInfoProvider();
                    control = Object.FindObjectOfType<FirstPersonController>();
                    currentStamina = maxStamina;
                    CreateStaminaBar.CreateBar();
                    staminaBarCreated = true;
                }

                if (StaminaSystem.staminaBar.Value == false)
                {
                    if (staminaBarCreated)
                    {
                        staminaFill.color = CustomColors.Transparent;
                    }
                }

                if (playerInfo.GetIsRunning() && !isGamePaused)
                {
                    if (currentStamina > StaminaSystem.minStamToRunJump.Value)
                    {
                        startedRunningOverStam = true;
                    }
                    CreateStaminaBar.UpdateStamina(StaminaSystem.staminaDrain.Value * Time.deltaTime);
                }
                else if (!playerInfo.GetIsRunning() && !isGamePaused && isGameLoaded)
                {
                    CreateStaminaBar.UpdateStamina(StaminaSystem.staminaRegain.Value * Time.deltaTime);
                    startedRunningOverStam = false;
                }

                if (currentStamina <= 0 && !isGamePaused && isGameLoaded)
                {
                    StatusController.Instance.disabledSprint = true;
                    StatusController.Instance.disabledJump = true;
                    control.m_Jump = false;

                    if (staminaBarCreated)
                    {
                        staminaFill.color = CustomColors.Red;
                    }
                }
                else if (currentStamina < StaminaSystem.minStamToRunJump.Value && !startedRunningOverStam && !isGamePaused && isGameLoaded)
                {
                    StatusController.Instance.disabledSprint = true;
                    StatusController.Instance.disabledJump = true;
                    control.m_Jump = false;

                    if (staminaBarCreated)
                    {
                        staminaFill.color = CustomColors.Red;
                    }
                }
                else
                {
                    if (!isGamePaused && isGameLoaded)
                    {
                        StatusController.Instance.disabledSprint = false;
                        StatusController.Instance.disabledJump = false;

                        if (staminaBarCreated)
                        {
                            staminaFill.color = CustomColors.Green;
                        }
                    }
                }

                if (isGamePaused && isGameLoaded && currentStamina != maxStamina)
                {
                    if (staminaBarCreated)
                    {
                        staminaFill.color = CustomColors.Transparent;
                    }
                }

                if(playerInfo.GetIsJumping())
                {
                    CreateStaminaBar.UpdateStamina(StaminaSystem.staminaJumpDrain.Value);
                    control.m_Jumping = false;
                }

                if(__instance.playerKOInProgress)
                {
                    CreateStaminaBar.ResetStamina();
                    staminaFill.color = CustomColors.Transparent;
                }

                if (currentStamina == maxStamina)
                {
                    staminaFill.CrossFadeAlpha(0f, 0.2f, true);
                }
                else if (currentStamina != maxStamina)
                {
                    staminaFill.CrossFadeAlpha(1f, 1.0f, true);
                }            
            }
        }
}

