
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoScene : MonoBehaviour
{
    public GameObject Player;
    public worldgen world;
    private void Start() {
        //Instantiate(Player,world.playerSpawnPoint,Quaternion.identity);
    }
    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
