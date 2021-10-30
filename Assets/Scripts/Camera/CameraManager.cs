using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Component References")]
    public Camera GameCam;
    public Camera UICam;
    [Header("Component References")]
    CinemachineComponentBase componentBase;
    public GameObject[] OrbitCameras = new GameObject[4];
    public GameObject LookAtObject;
    public CameraController cameraController;

    bool IsGamePaused = false;
    float XAxisMaxSpeed;
    float YAxisMaxSpeed;
    private void Start() {
        if(LookAtObject != null) {
            GameObject wCenter = new GameObject();
            wCenter.transform.position = LookAtObject.transform.position;
            foreach(var Camera in OrbitCameras) {
                Camera.GetComponent<CinemachineFreeLook>().m_Follow = wCenter.transform;
                Camera.GetComponent<CinemachineFreeLook>().m_LookAt = wCenter.transform;
                XAxisMaxSpeed = Camera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed;
                YAxisMaxSpeed = Camera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed;
                Camera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0f;
                Camera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0f;
            }
        }
    }

    public void PauseMove() {
        IsGamePaused = !IsGamePaused;
        if(IsGamePaused) {
            foreach(var Camera in OrbitCameras) {
                Camera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0f;
            }
        } else {
            foreach(var Camera in OrbitCameras) {
                Camera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0f;
            }
        }
    }
    public void RotateCameraLeft() {
        StartCoroutine(ResetCameraAxis());
        cameraController.SwitchCamera(-1);
    }
    public void RotateCameraRight() {
        StartCoroutine(ResetCameraAxis());
        cameraController.SwitchCamera(1);
    }
    IEnumerator ResetCameraAxis() {
        yield return new WaitForFixedUpdate();
        cameraController.GetComponent<CinemachineStateDrivenCamera>().LiveChild.VirtualCameraGameObject.GetComponent<CinemachineFreeLook>().m_YAxis.Value = 0.5f;
    }
}
