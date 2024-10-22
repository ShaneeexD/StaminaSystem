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
    }
    private void HandleGameLoaded(object sender, EventArgs e)
    {
        StaminaBar.isGameLoaded = true;
    }
}
