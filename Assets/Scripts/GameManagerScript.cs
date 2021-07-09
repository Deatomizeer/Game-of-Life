using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Cell object.
    public GameObject cubePrefab;
    // Canvas to switch between the running/halted images.
    public CanvasManager canvasManager;
    [Range(1, 20)]
    public int gridWidth;
    [Range(1, 20)]
    public int gridHeight;
    [Range(1, 4)] // Distance coefficient of the `gridSpread` struct.
    public int gridSpreadRatio;
    [Range(.3f, 1)] // Time per each generation in seconds.
    public float frameDelay;
    // Time elapsed since last grid update.
    private float timer;

    private struct GS // Distance between the cubes on the grid.
    {
        public float x;
        public float y;
    }
    private GS gridSpread;
    public bool running;
    // Determines how neighbors are counted at the edges.
    public bool wrapAroundEdges;
    // Used to instantiate the objects.
    private GameObject[,] cubeGrid;
    // Used to set individual cells' states.
    private CellManager[,] cellScriptGrid;
    // R reset
    // SPACE toggle run
    // Left Click toggle individual cells

    // Start is called before the first frame update
    void Start()
    {
        Vector3 cubePrefabSize = cubePrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        gridSpread.x = cubePrefabSize.x * gridSpreadRatio;
        gridSpread.y = cubePrefabSize.y * gridSpreadRatio;
        running = false;
        timer = 0f;
        // Generate the grid of cubes.
        cubeGrid = new GameObject[gridHeight, gridWidth];
        InstantiateGrid(gridWidth, gridHeight);
        CreateCellScriptGrid();
        // Adjust the camera.
        Camera.main.GetComponent<CenterCamera>().CenterOnGrid(gridSpread.x, gridSpread.y, gridWidth, gridHeight);
        // Get the canvas reference.
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
    }

    private void InstantiateGrid(int width, int height)
    {
        Vector3 spawnPos = transform.position;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                cubeGrid[i, j] = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
                cubeGrid[i, j].transform.SetParent(this.transform);
                cubeGrid[i, j].name = "Cell (" + j + ", " + i + ")";
                spawnPos.x += gridSpread.x;
            }
            spawnPos.x = transform.position.x;
            spawnPos.y += gridSpread.y;
        }
    }

    // Populate the `cellScriptGrid` array for ease of access to the cells' states.
    private void CreateCellScriptGrid()
    {
        cellScriptGrid = new CellManager[gridHeight, gridWidth];
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                cellScriptGrid[i, j] = cubeGrid[i, j].GetComponent<CellManager>();
            }
        }
    }

    // Update is called once per frame.
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            running = !running;
            timer = 0f;
            canvasManager.SetRunningImage(running ? 1 : 0);
        }
        else if (Input.GetKeyDown("r") && !running)
        {
            ResetGrid();
        }
        if (running)
        {
            timer += Time.deltaTime;
            if (timer > frameDelay)
            {
                int[,] nextGenInts = CalculateNextGeneration();
                UpdateGrid(nextGenInts);

                timer -= frameDelay;
            }
        }
    }

    // Return a 2D boolean array that determines the next generation layout.
    private int[,] CalculateNextGeneration()
    {
        int[,] nextGen = new int[gridHeight, gridWidth];
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                int count;
                if (wrapAroundEdges)
                {
                    count = CountAliveNeighborsWrapping(i, j);
                } else
                {
                    count = CountAliveNeighbors(i, j);
                }
                int currentState = cellScriptGrid[i, j].alive;
                nextGen[i, j] = DetermineCellState(count, currentState);
            }
        }
        return nextGen;
    }
    // Count the living neighbors of a given cell.
    private int CountAliveNeighbors(int row, int col)
    {
        int count = 0;
        if(row > 0)
        {
            // Top middle.
            count += cellScriptGrid[row - 1, col].alive;
            if(col > 0)
            {
                // Top left.
                count += cellScriptGrid[row - 1,col - 1].alive;
            }
            if(col < gridWidth-1)
            {
                // Top right.
                count += cellScriptGrid[row - 1, col + 1].alive;
            }
        }
        if (col > 0)
        {
            // Middle left.
            count += cellScriptGrid[row, col - 1].alive;
        }
        if (col < gridWidth-1)
        {
            // Middle right.
            count += cellScriptGrid[row, col + 1].alive;
        }
        if(row < gridHeight-1)
        {
            // Bottom middle.
            count += cellScriptGrid[row + 1, col].alive;
            if (col > 0)
            {
                // Bottom left.
                count += cellScriptGrid[row + 1, col - 1].alive;
            }
            if (col < gridWidth-1)
            {
                // Bottom right.
                count += cellScriptGrid[row + 1, col + 1].alive;
            }
        }
        return count;
    }
    // Count the living neighbors, but wrap the grid around if on the edge.
    private int CountAliveNeighborsWrapping(int row, int col)
    {
        // Pre-emptively subtract self from the count.
        int count = -cellScriptGrid[row,col].alive;
        // Count each adjacent cell, including self.
        for( int i=-1; i <= 1; i++ )
        {
            for( int j=-1; j<= 1; j++)
            {
                count += cellScriptGrid[
                    (row + i + gridHeight) % gridHeight,
                    (col + j + gridWidth) % gridWidth
                ].alive;
            }
        }
        return count;
    }
    // Determine whether the given cell should live or die in the next generation.
    private int DetermineCellState(int aliveNeighbors, int currentState)
    {
        switch (aliveNeighbors)
        {
            case 3:
                return 1;
            case 2:
                return currentState;
            default:
                return 0;
        }
    }
    // Proceed into the next generation.
    private void UpdateGrid(int[,] nextGenInts)
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                int state = nextGenInts[i, j];
                cellScriptGrid[i, j].SetState(state);
            }
        }
    }
    // Kill every cell on the grid.
    public void ResetGrid()
    {
        for( int i=0; i < gridHeight; i++ )
        {
            for(int j = 0; j < gridWidth; j++)
            {
                cellScriptGrid[i, j].SetState(0);
            }
        }
    }
}
