
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public GameObject player;
    public GameObject spawnPoint;
    private void Start() {
        //Instantiate(Player,spawnPoint.transform.position,Quaternion.identity);
        GameManager.Instance.playerPrefab = player;
        GameManager.Instance.AssignCurrentController(player.GetComponent<PlayerController>());
    }
    public void RestartScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
