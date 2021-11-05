using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject playerPrefab;
    bool isPaused;
    public bool IsPaused => isPaused;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        SetupUI();
    }
    void SetupUI() {
        UIManager.Instance.SetupUIManager();
    }
    public PlayerController GetPlayerController() {
        return playerController;
    }
    public void AssignCurrentController(PlayerController _controller) {
        playerController = _controller;
    }

    public bool TogglePause(bool _b) {
        isPaused = _b;
        ToggleUIPause(_b);
        SwitchControlScheme();
        return isPaused;
    }
    void SwitchControlScheme() {
        switch(isPaused) {
            case true:
                playerController.EnablePauseMenuControls();
                break;
            case false:
                playerController.EnableGameplayControls();
                break;
        }
    }
    void ToggleUIPause(bool _b) {
        UIManager.Instance.UpdateUIMenuPauseState(_b);
    }
}
