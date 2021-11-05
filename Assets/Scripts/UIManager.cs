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
    public void UpdateUIMenuPauseState(bool _state) {
        uiBehaviour.UpdateUIMenuState(_state);
    }
}
