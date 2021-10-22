using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    bool isPaused;
    public bool IsPaused => isPaused;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = true;
    }

    public bool ToggleMove(bool b) {
        return isPaused = b;
    }
}
