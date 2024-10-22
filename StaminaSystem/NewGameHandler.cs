using System;
using BepInEx.Logging;
using SOD.Common;
using SOD.Common.Helpers;
using UnityEngine;
using StaminaSystem;
public class NewGameHandler
{
    public static ManualLogSource Logger;

    public NewGameHandler()
    {
        Lib.SaveGame.OnAfterLoad += HandleGameLoaded;
        Lib.SaveGame.OnAfterNewGame += HandleNewGameStarted;
    }   

    private void HandleNewGameStarted(object sender, EventArgs e)
    {
        StaminaBar.isGameLoaded = true;
        Lib.GameMessage.Broadcast("Thank you for downloading LifeIsHard!", InterfaceController.GameMessageType.notification, InterfaceControls.Icon.lockpick, Color.green, 10.0f);
        Logger.LogInfo("A new game has started!");
    }
    private void HandleGameLoaded(object sender, EventArgs e)
    {
        StaminaBar.isGameLoaded = true;
        Lib.GameMessage.Broadcast("Thank you for downloading LifeIsHard!", InterfaceController.GameMessageType.notification, InterfaceControls.Icon.lockpick, Color.green, 10.0f);
        Logger.LogInfo("A new game has started!");
    }
}
