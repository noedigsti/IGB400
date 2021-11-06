
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    /// <summary>
    /// UI Transition Animator
    /// </summary>
    public Animator animator;
    public GameObject player;
    private void Start() {
        //Instantiate(Player,spawnPoint.transform.position,Quaternion.identity);
        if(player) {
            GameManager.Instance.playerPrefab = player;
            GameManager.Instance.AssignCurrentController(player.GetComponent<PlayerController>()); 
        }
    }
    IEnumerator LoadSequence(int _index) {
        animator.Play("SceneTransition_Start");
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("SceneTransition_Start")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
        );
        SceneManager.LoadScene(_index);
    }
    public void RestartScene() {
        StartCoroutine(LoadSequence(SceneManager.GetActiveScene().buildIndex));
    }
    public void LoadMainMenuScene() {
        StartCoroutine(LoadSequence(0));
    }
}
