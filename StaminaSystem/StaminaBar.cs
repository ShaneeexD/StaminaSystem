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
    public class StaminaBar : MonoBehaviour
    {
        private static bool staminaBarCreated;
        private static bool startedRunningOverStam;

        public static bool isGamePaused = false;
        public static bool isGameLoaded = false;
        public static bool isPlayerJumping;


        public static Image staminaFill;
        public static Color currentColor;
        public static float maxStamina = 100f;
        public static float currentStamina;
        private static RectTransform staminaFillRect;

        [HarmonyPrefix]
        public static void Prefix(Player __instance)
        {
            PlayerInfoProvider playerInfo = new PlayerInfoProvider();
            GameplayControls player = GameplayControls.Instance;
            FirstPersonController control = Object.FindObjectOfType<FirstPersonController>();

            if (player != null)
            {
                if (!staminaBarCreated)
                {
                    currentStamina = maxStamina;
                    CreateStaminaBar();
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
                    UpdateStamina(StaminaSystem.staminaDrain.Value * Time.deltaTime);
                }
                else if (!playerInfo.GetIsRunning() && !isGamePaused && isGameLoaded)
                {
                    UpdateStamina(StaminaSystem.staminaRegain.Value * Time.deltaTime);
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
                    UpdateStamina(StaminaSystem.staminaJumpDrain.Value);
                    control.m_Jumping = false;
                }

                if(__instance.playerKOInProgress)
                {
                    ResetStamina();
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



                static void CreateStaminaBar()
                {
                    GameObject staminaPanel = new GameObject("StaminaBarBackground");
                    Canvas canvas = FindObjectOfType<Canvas>();

                    if (canvas == null)
                    {
                        canvas = new GameObject("Canvas").AddComponent<Canvas>();
                        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.gameObject.AddComponent<CanvasScaler>();
                        canvas.gameObject.AddComponent<GraphicRaycaster>();
                    }

                    staminaPanel.transform.SetParent(canvas.transform);

                    RectTransform panelRect = staminaPanel.AddComponent<RectTransform>();
                    panelRect.sizeDelta = new Vector2(0, 0);
                    panelRect.anchorMin = new Vector2(0.5f, 0);
                    panelRect.anchorMax = new Vector2(0.5f, 0);
                    panelRect.anchoredPosition = new Vector2(0, 20);

                    Image panelImage = staminaPanel.AddComponent<Image>();
                    panelImage.color = Color.black;

                    GameObject fillImageObj = new GameObject("StaminaFill");
                    fillImageObj.transform.SetParent(staminaPanel.transform);

                    staminaFillRect = fillImageObj.AddComponent<RectTransform>();
                    staminaFillRect.sizeDelta = new Vector2(500, 10);
                    staminaFillRect.anchorMin = Vector2.zero;
                    staminaFillRect.anchorMax = Vector2.one;
                    staminaFillRect.anchoredPosition = Vector2.zero;

                    staminaFill = fillImageObj.AddComponent<Image>();
                    staminaFill.color = currentColor;

                    Outline outline = fillImageObj.AddComponent<Outline>();
                    outline.effectColor = new Color(0f, 0f, 0f, 1f); 
                    outline.effectDistance = new Vector2(4, 4); 

                }

                static void UpdateStamina(float amount)
                {
                    currentStamina += amount;
                    currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

                    if (staminaBarCreated)
                    {
                        staminaFill.fillAmount = currentStamina / maxStamina;

                        float fillWidth = (currentStamina / maxStamina) * 500;

                        staminaFill.rectTransform.sizeDelta = new Vector2(fillWidth, 10);
                    }
                }

                static void ResetStamina()
                {
                    currentStamina = maxStamina;
                    staminaFillRect.sizeDelta = new Vector2(500, 10);
                }
            }
        }
    }
}
