using Il2CppSystem.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerInfoProvider
{
    public Vector3 GetPlayerLocation()
    {
        Player player = Player.Instance;
        return player.currentNodeCoord;
    }
    public NewNode PlayerNode() 
    {
        Player player = Player.Instance;
        return player.currentNode;
    }
    public bool GetIsRunning() 
    {
        Player player = Player.Instance;
        return player.isRunning;
    }
    public void SetIsRunning(bool isRunning)
    {
        Player player = Player.Instance;
        player.isRunning = isRunning;
    }
    public bool GetHasJumped()
    {
        FirstPersonController control = Object.FindObjectOfType<FirstPersonController>();
        return control.m_Jump;
    }
    public bool GetIsJumping()
    {
        FirstPersonController control = Object.FindObjectOfType<FirstPersonController>();
        return control.m_Jumping;
    }
    public bool GetIsGrounded()
    {
        Player player = Player.Instance;
        return player.isGrounded;
    }

    public float GetMovementRunSpeed()
    {
        GameplayControls player = GameplayControls.Instance;
        return player.playerRunSpeed;
    }
    public void SetMovementRunSpeed(float setMovementRunSpeed)
    {
        GameplayControls player = GameplayControls.Instance;
        player.playerRunSpeed = setMovementRunSpeed;
    }
    public float GetMovementWalkSpeed()
    {
        GameplayControls player = GameplayControls.Instance;
        return player.playerWalkSpeed;
    }
    public void SetMovementWalkSpeed(float setMovementWalkSpeed)
    {
        GameplayControls player = GameplayControls.Instance;
        player.playerWalkSpeed = setMovementWalkSpeed;
    }
    public float GetCurrentHealth()
    {
        Player player = Player.Instance;
        return player.currentHealth;
    }
    public void SetCurrentHealth(float health)
    {
        Player player = Player.Instance;
        player.currentHealth = health;
    }
    public bool IsOnDuty()
    {
        Player player = Player.Instance;
        return player.isOnDuty;
    }
    public bool IsCurrentlyInAutoTravel()
    {
        Player player = Player.Instance;
        return player.autoTravelActive;
    }
    public float GetBlackEye()
    {
        Player player = Player.Instance;
        return player.blackEye;
    }
    public void SetBlackEye(float amount)
    {
        Player player = Player.Instance;
        player.blackEye = amount;
    }
    public float GetBlackedOut()
    {
        Player player = Player.Instance;
        return player.blackedOut;
    }
    public void SetBlackedOut(float amount)
    {
        Player player = Player.Instance;
        player.blackedOut = amount;
    }
    public Telephone GetAnsweringPhone()
    {
        Player player = Player.Instance;
        return player.answeringPhone;
    }
    public Il2CppSystem.Collections.Generic.List<NewAddress> ApartmentsOwned()
    {
        Player player = Player.Instance;
        return player.apartmentsOwned;
    }
}