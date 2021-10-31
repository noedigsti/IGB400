using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public UIMenuBehaviour uiBehaviour;
    public void SetupUIManager() {
        uiBehaviour.SetupBehaviour();
    }
    public void UpdateUIMenuState(bool _state) {
        uiBehaviour.UpdateUIMenuState(_state);
    }
}
