using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System.Reflection;
using static Il2CppSystem.Linq.Expressions.Interpreter.NullableMethodCallInstruction;
using System.Threading.Tasks;

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

        private static PlayerInfoProvider _playerInfo;
        private static FirstPersonController _control;

        public static PlayerInfoProvider playerInfo
        {
            get
            {
                if (_playerInfo == null)
                    _playerInfo = new PlayerInfoProvider();
                return _playerInfo;
            }
        }

        public static FirstPersonController control
        {
            get
            {
                if (_control == null)
                    _control = UnityEngine.Object.FindObjectOfType<FirstPersonController>();
                return _control;
            }
        }

        [HarmonyPrefix]
        public static void Prefix(Player __instance)
        {
            if (!staminaBarCreated)
            {
                currentStamina = maxStamina;
                CreateStaminaBar.CreateBar();
                staminaBarCreated = true;
            }

            if (StaminaSystem.staminaBar.Value == false && staminaBarCreated)
            {
                staminaFill.color = CustomColors.Transparent;
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

                if (staminaBarCreated && StaminaSystem.staminaBar.Value)
                {
                    staminaFill.color = CustomColors.Red;
                }
            }
            else if (currentStamina < StaminaSystem.minStamToRunJump.Value && !startedRunningOverStam && !isGamePaused && isGameLoaded)
            {
                StatusController.Instance.disabledSprint = true;
                StatusController.Instance.disabledJump = true;
                control.m_Jump = false;

                if (staminaBarCreated && StaminaSystem.staminaBar.Value)
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

                    if (staminaBarCreated && StaminaSystem.staminaBar.Value)
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

    [HarmonyPatch(typeof(Player), "OnDestroy")]
    public static class StaminaBarCleanup
    {
        [HarmonyPostfix]
        public static void Postfix(Player __instance)
        {
            if (StaminaBar.staminaBarCreated)
            {
                CreateStaminaBar.DestroyBar();
                StaminaBar.staminaBarCreated = false;
            }
        }
    }


    [HarmonyPatch(typeof(FirstPersonItemController), nameof(FirstPersonItemController.MeleeAttack))]
    [HarmonyPriority(Priority.High)]
    public static class MeleeAttackPatch
    {
        public static void Prefix(FirstPersonItemController __instance)
        {
            if (StaminaBar.staminaBarCreated)
            {
                float staminaCost = StaminaSystem.staminaDrainMeleeWeapon.Value; // Default to weapon cost                

                // Check if the player is using fists
                if (BioScreenController.Instance.selectedSlot != null &&
                    BioScreenController.Instance.selectedSlot.isStatic == FirstPersonItemController.InventorySlot.StaticSlot.fists)
                {
                    staminaCost = StaminaSystem.staminaDrainMeleeFist.Value; // Use fists cost
                }

                // Update stamina
                CreateStaminaBar.UpdateStamina(staminaCost);
            }
        }
    }

    [HarmonyPatch(typeof(FirstPersonItemController), nameof(FirstPersonItemController.OnInteraction))]
    [HarmonyPriority(Priority.High)]
    public static class OnInteractionPatch
    {       
        public static bool hasBeenDelayed = false;

        public static void Prefix(FirstPersonItemController __instance, InteractablePreset.InteractionKey input)
        {
            if (StaminaBar.currentStamina <= StaminaSystem.minStamToAttack.Value && !hasBeenDelayed)
            {        
                float delayAmount = (StaminaSystem.minStamToRunJump.Value - StaminaBar.currentStamina + 0.7f) / 20;

                // Increase the attack delay when stamina is too low

                __instance.attackMainDelay += delayAmount;

                hasBeenDelayed = true;
                //StaminaSystem.Logger.LogInfo("Attack delay increased due to low stamina.");

                // Reset hasBeenDelayed after the delay
                Task.Run(async () =>
                {
                    await Task.Delay((int)(delayAmount * 1000)); // Convert seconds to milliseconds
                    hasBeenDelayed = false;
                });
            } 
        }
    }

    [HarmonyPatch(typeof(FirstPersonItemController), nameof(FirstPersonItemController.Block))]
    [HarmonyPriority(Priority.High)]
    public static class BlockPatch
    {
        public static void Prefix(FirstPersonItemController __instance)
        {
            if (StaminaBar.staminaBarCreated)
            { 
                CreateStaminaBar.UpdateStamina(StaminaSystem.staminaDrainBlock.Value);
            }
        }
    }

    [HarmonyPatch(typeof(FirstPersonItemController), nameof(FirstPersonItemController.ThrowGrenade))]
    [HarmonyPriority(Priority.High)]
    public static class ThrowGrenadePatch
    {
        public static void Prefix(FirstPersonItemController __instance)
        {
            if (StaminaBar.staminaBarCreated)
            { 
                CreateStaminaBar.UpdateStamina(StaminaSystem.staminaDrainThrowables.Value);
            }
        }
    }
}
