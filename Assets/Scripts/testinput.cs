//using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using Assets.Scripts.Character;
//using UnityEngine.InputSystem.EnhancedTouch;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
//using UnityEngine.EventSystems;

public class testinput : MonoBehaviour
{
    //    //public CharacterEditorDisplay m_classDisplay;
    public InputActions m_Controls;
    //    public GameObject m_Object;
    //    //[SerializeField]
    //    //Character m_char;
    //    //[SerializeField]
    //    //Character m_char1;
    //    public GameObject GridObject;
    //    public Material material;
    private void Awake() {
        m_Controls = new InputActions();
        //m_controls.Player.LClick.ChangeBinding(0).WithInteraction("Hold(duration=0.5)");
    }
    //    //private void Update() {
    //    //    if(Touch.activeFingers.Count == 1) {
    //    //        Touch activeTouch = Touch.activeFingers[0].currentTouch;
    //    //        Debug.Log($"Phase: {activeTouch.phase} | Position: {activeTouch.startScreenPosition}");
    //    //    }
    //    //}
    private void Start() {
        //m_char = new Character();
        //m_char1 = new Character(8);
        //material = GridObject.GetComponent<MeshRenderer>().material;
        //Debug.Log(material.GetFloat("TileSize"));
    }
    //    private void OnEnable()
    //    {
    //        m_controls.Enable();
    //        m_controls.Player.Fire.Enable();
    //        m_controls.Player.Fire.started += LogMe;
    //        //EnhancedTouchSupport.Enable();
    //        //m_controls.Player.LClick.Enable();
    //        //m_controls.Player.LClick.performed += LogMe;

    //        //m_controls.Menu.Exit.Enable();
    //        //m_controls.Menu.Exit.performed += ExitApp;

    //        Debug.Log("Input System Enabled");
    //    }
    //    private void OnDisable()
    //    {
    //        //m_controls.UI.Click.performed -= LogMe;
    //        m_controls.UI.Click.Disable();
    //        //m_controls.Player.LClick.Disable();
    //        //m_controls.Player.LClick.performed -= LogMe;

    //        //m_controls.Menu.Exit.Disable();
    //        //m_controls.Menu.Exit.performed -= ExitApp;
    //        Debug.Log("Disabled");
    //    }
    //    private void LogMe(InputAction.CallbackContext context)
    //    {
    //        Debug.Log("Tapped.");
    //        CastClickRay();
    //    }

    //    void CastClickRay() {
    //        Vector3 mousePos = Input.mousePosition;
    //        var ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x,mousePos.y,Camera.main.nearClipPlane));
    //        if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject.CompareTag("UI Overlay") && !EventSystem.current.IsPointerOverGameObject()) {
    //            StartInteraction(hit.point);
    //        }
    //    }
    //    void StartInteraction(Vector3 point) {
    //        material.SetVector("ClickPoint",point);
    //        GameObject clone = Instantiate(m_Object,point,Quaternion.identity);
    //    }
    //    public void DebugLog() {
    //        Debug.Log("Clicked");
    //    }
    //    public void MenuClick() {
    //        Debug.Log("Button clicked!");
    //        //m_char.Perform(new Move(m_Objects[0], new Vector3(1,0,0)));
    //        //m_char1.Perform(new Move(m_Objects[1], new Vector3(-1,0,0)));
    //    }
    //    private void ExitApp(InputAction.CallbackContext context)
    //    {
    //        Debug.Log("Exiting application..");
    //        Application.Quit();
    //        System.Diagnostics.Process.GetCurrentProcess().Kill();
    //    }
}
