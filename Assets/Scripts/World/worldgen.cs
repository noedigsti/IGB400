using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class worldgen : MonoBehaviour
{
    public bool GenerateWorld = true;
    public GameObject corner;
    public GameObject side;
    public GameObject inner;
    public int gridX = 8; // Assume N x N generation
    public float offset = 1f;
    public Vector3 gridOrigin = Vector3.zero;
    World world;

    [Serializable]
    public class World
    {
        public List<Row> Rows;
        public GameObject[,] array;
        public World(int capacity) {
            TileCount = 0;
            //Debug.Log($"Generating world ROW with size of {capacity}");
            Rows = new List<Row>(capacity);
            for(int z = 0;z < capacity;z++) {
                //Debug.Log($"ROW ({z}) of {capacity}");
                //row[z] = new Column(capacity);
                Rows.Add(new Row(capacity));
            }
            array = new GameObject[capacity,capacity];
            //Debug.Log($"Size of ROW = {row.Count}");
        }
    }
    [Serializable]
    public class Row
    {
        public static int index = 0;
        public string _name = "Row";
        public List<Tile> Columns;
        public Row(int capacity) {
            _name += $" {index++}";
            //Debug.Log($"Generating world COLUMN {index} with size of {capacity}");
            Columns = new List<Tile>(capacity);
            for(int i = 0;i < capacity;i++) {
                Columns.Add(new Tile());
                //col[i] = Vector3.one;
            }
            //Debug.Log($"Size of COLUMN = {col.Count}");
        }
    }
    [Serializable]
    public class Tile
    {
        public static int index;
        public string _name = "Tile";
        public Vector3 position;
        public Tile() {
            _name += $" {index++}";
            position = Vector3.zero;
        }
    }
    public static int TileCount = 0;
    public static void AddToCount() {
        TileCount += 1;
        //Debug.Log(count);
    }
    // Start is called before the first frame update
    void Start()
    {
        if(GenerateWorld) {
            world = new World(gridX);
            for(int x = 0;x < gridX;x++) {
                for(int z = 0;z < gridX;z++) {
                    Vector3 pos = new Vector3(x * offset,0,z * offset) + gridOrigin;
                    world.Rows[x].Columns[z].position = pos;
                }
            }
            Spawn();
        }
        //world.array[0,5].transform.position += new Vector3(0,1,0);
    }
    void Spawn() {
        for(int x = 0;x < gridX;x++) {
            for(int z = 0;z < gridX;z++) {
                GameObject clone = Instantiate(inner,world.Rows[x].Columns[z].position,Quaternion.identity,transform);
                world.array[x,z] = clone;
                //clone.GetComponent<Mesh>().RecalculateBounds();
            }
        }
    }
}
