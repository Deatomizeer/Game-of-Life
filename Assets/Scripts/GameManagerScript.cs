using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject cubePrefab;
    [Range(1, 10)]
    public int gridWidth;
    [Range(1, 10)]
    public int gridHeight;
    [Range(1, 4)]
    public int gridSpreadRatio;

    private struct GS // Distance between the cubes on the grid.
    {
        public float x;
        public float y;
    }
    private GS gridSpread;

    private GameObject[,] cubeGrid;
    // R reset
    // SPACE toggle run
    // Left Click toggle individual cells

    // Start is called before the first frame update
    void Start()
    {
        Vector3 cubePrefabSize = cubePrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        gridSpread.x = cubePrefabSize.x * gridSpreadRatio;
        gridSpread.y = cubePrefabSize.y * gridSpreadRatio;
        // Generate the grid of cubes.
        cubeGrid = new GameObject[gridHeight, gridWidth];
        InstantiateGrid(gridWidth, gridHeight);
    }

    private void InstantiateGrid(int width, int height)
    {
        Vector3 spawnPos = transform.position;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                cubeGrid[i,j] = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
                cubeGrid[i, j].transform.SetParent(this.transform);
                cubeGrid[i, j].name = "Cell (" + j + ", " + i + ")";
                //cubeGrid[i, j].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                spawnPos.x += gridSpread.x;
            }
            spawnPos.x = transform.position.x;
            spawnPos.y += gridSpread.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
