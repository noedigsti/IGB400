using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class EventSystemManager : Singleton<EventSystemManager>
{
    public EventSystem eventSystem;
    public InputSystemUIInputModule inputSystemUIInputModule;

    public void UpdateActionAssetToFocusedPlayer() {
        PlayerController focusedPlayerController = GameManager.Instance.GetPlayerController();
        inputSystemUIInputModule.actionsAsset = focusedPlayerController.GetInputActionAsset();
    }

}
