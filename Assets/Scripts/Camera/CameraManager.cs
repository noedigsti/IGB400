using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    [Header("Component References")]
    public Camera GameCam;
    public Camera UICam;
    [Header("Pause References")]
    public GameObject UIPauseCam;
    public GameObject UIPause;
    [Header("_")]
    public GameObject LookAtObject;
    public CameraController cameraController;
    private void Start() {
        LookAtObject = GameManager.Instance.playerPrefab;
        ResetActiveCameraTarget();
    }
    public void ResetActiveCameraTarget() {
        cameraController.GetComponent<CinemachineStateDrivenCamera>().LiveChild.LookAt = LookAtObject.transform;
        cameraController.GetComponent<CinemachineStateDrivenCamera>().LiveChild.Follow = LookAtObject.transform;
    }
    public void PauseGame(bool _b) {
        UIPause.SetActive(_b);
        UIPauseCam.SetActive(_b);
    }
    public void RotateCameraLeft() {
        cameraController.SwitchCamera(-1);
    }
    public void RotateCameraRight() {
        cameraController.SwitchCamera(1);
    }
}
