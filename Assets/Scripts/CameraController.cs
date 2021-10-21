using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Animator animator;
    int index = 0;
    public void SwitchCamera(int _index) {
        index -= _index;
        if (index >= 4) index = 0;
        if (index < 0) index = 3;
        switch(index) {
            case 0:
                animator.Play("SouthCamera");
                break;
            case 1:
                animator.Play("WestCamera");
                break;
            case 2:
                animator.Play("NorthCamera");
                break;
            case 3:
                animator.Play("EastCamera");
                break;
            default:
                index = 0;
                animator.Play("SouthCamera");
                break;
        }
    }
}
