using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    bool confirm = false;
    public Animator animator;
    public float delay = 3f;
    public void LoadScene() {
        if(!confirm) {
            confirm = true;
            StartCoroutine(LoadSequence());
        }
    }
    private void Start() {
        confirm = false;
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(LoadSequenceStart());
    }
    IEnumerator LoadSequenceStart() {
        yield return new WaitForSeconds(delay);
        animator.Play("SceneTransition");
        StartCoroutine(PlayAudioOnScene());
    }
    IEnumerator LoadSequence() {
    FindObjectOfType<AudioManager>().Play("MainMenuStart");
    animator.Play("SceneTransition_Start");
    yield return new WaitUntil(() =>
        animator.GetCurrentAnimatorStateInfo(0).IsName("SceneTransition_Start")
        && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
    );
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator PlayAudioOnScene() {
        yield return new WaitUntil(() =>
        animator.GetCurrentAnimatorStateInfo(0).IsName("SceneTransition")
        && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        FindObjectOfType<AudioManager>().Play("MainMenuStart");
    }
}
