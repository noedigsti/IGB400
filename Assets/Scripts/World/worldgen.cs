using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class worldgen : MonoBehaviour
{
    public bool Generate = true;
    public bool randomWorld = false;
    [Range(0,100)]
    public int randomFillPercent;
    public int[,] map;
    public string seed;
    public int width;
    public int length;
    public int tileCount;
    void GenerateMap() {
        map = new int[width,length];
        RandomFillMap();

        for(int i = 0;i < 5;i++) {
            SmoothMap();
        }
    }
    void RandomFillMap() {
        if(randomWorld) {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for(int x = 0;x < width;x++) {
            for(int y = 0;y < length;y++) {
                if(x == 0 || x == width - 1 || y == 0 || y == length - 1) {
                    map[x,y] = 1;
                } else {
                    map[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(Generate) {
            GenerateMap();
        }
    }
    void SmoothMap() {
        for(int x = 0;x < width;x++) {
            for(int y = 0;y < length;y++) {
                int neighbourWallTiles = GetSurroundingWallCount(x,y);

                if(neighbourWallTiles > 4)
                    map[x,y] = 1;
                else if(neighbourWallTiles < 4)
                    map[x,y] = 0;

            }
        }
    }
    int GetSurroundingWallCount(int gridX,int gridY) {
        int wallCount = 0;
        for(int neighbourX = gridX - 1;neighbourX <= gridX + 1;neighbourX++) {
            for(int neighbourY = gridY - 1;neighbourY <= gridY + 1;neighbourY++) {
                if(neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < length) {
                    if(neighbourX != gridX || neighbourY != gridY) {
                        wallCount += map[neighbourX,neighbourY];
                    }
                } else {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    void OnDrawGizmos() {
        if(map != null) {
            for(int x = 0;x < width;x++) {
                for(int y = 0;y < length;y++) {
                    Gizmos.color = (map[x,y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f,0,-length / 2 + y + .5f);
                    Gizmos.DrawCube(pos,Vector3.one);
                }
            }
        }
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            GenerateMap();
        }
    }
}
