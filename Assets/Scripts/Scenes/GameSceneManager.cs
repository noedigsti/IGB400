
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// TODO: combined this script with MainMenu.cs
/// </summary>
public class GameSceneManager : MonoBehaviour 
{
    /// <summary>
    /// UI Transition Animator
    /// </summary>
    public Animator animator;
    public Animator animator2; // When player completes the game

    public GameObject player;
    private void Start() {
        //Instantiate(Player,spawnPoint.transform.position,Quaternion.identity);
        if(player) {
            GameManager.Instance.playerPrefab = player;
            GameManager.Instance.AssignCurrentController(player.GetComponent<PlayerController>()); 
        }
    }
    IEnumerator LoadSequence(int _index,float _timer) {
        animator.Play("SceneTransition_Start");
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("SceneTransition_Start")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
        );
        StartCoroutine(LoadSceneWithDelay(_index,_timer));
    }
    IEnumerator LoadSceneWithDelay(int _index, float _timer) {
        yield return new WaitForSeconds(_timer);
        SceneManager.LoadScene(_index);
    }
    public void RestartScene() {
        StartCoroutine(LoadSequence(SceneManager.GetActiveScene().buildIndex, 0));
    }
    public void LoadMainMenuScene() {
        animator.gameObject.SetActive(false);
        animator = animator2;
        animator.gameObject.SetActive(true);
        StartCoroutine(LoadSequence(0, 2));
    }
}
