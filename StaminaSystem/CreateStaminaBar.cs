using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using StaminaSystem;

namespace StaminaSystem
{
    public class CreateStaminaBar
    {
        private static GameObject staminaPanel;

        public static void CreateBar()
        {
            staminaPanel = new GameObject("StaminaBarBackground");
            Canvas canvas = UnityEngine.Object.FindObjectOfType<Canvas>();

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

            StaminaBar.staminaFillRect = fillImageObj.AddComponent<RectTransform>();
            StaminaBar.staminaFillRect.sizeDelta = new Vector2(500, 10);
            StaminaBar.staminaFillRect.anchorMin = Vector2.zero;
            StaminaBar.staminaFillRect.anchorMax = Vector2.one;
            StaminaBar.staminaFillRect.anchoredPosition = Vector2.zero;

            StaminaBar.staminaFill = fillImageObj.AddComponent<Image>();
            StaminaBar.staminaFill.color = StaminaBar.currentColor;

            Outline outline = fillImageObj.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 1f);
            outline.effectDistance = new Vector2(4, 4);
        }

        public static void DestroyBar()
        {
            if (staminaPanel != null)
            {
                UnityEngine.Object.Destroy(staminaPanel.gameObject);
                StaminaSystem.Logger.LogInfo("Deleted old stamina bar");
            }
        }

        public static void UpdateStamina(float amount)
        {
            StaminaBar.currentStamina += amount;
            StaminaBar.currentStamina = Mathf.Clamp(StaminaBar.currentStamina, 0, StaminaBar.maxStamina);

            if (StaminaBar.staminaBarCreated)
            {
                StaminaBar.staminaFill.fillAmount = StaminaBar.currentStamina / StaminaBar.maxStamina;

                float fillWidth = (StaminaBar.currentStamina / StaminaBar.maxStamina) * 500;

                StaminaBar.staminaFill.rectTransform.sizeDelta = new Vector2(fillWidth, 10);
            }
        }

        public static void ResetStamina()
        {
            StaminaBar.currentStamina = StaminaBar.maxStamina;
            StaminaBar.staminaFillRect.sizeDelta = new Vector2(500, 10);
        }
    }
}
