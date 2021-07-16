using UnityEngine;

public class DataHolderScript : MonoBehaviour
{
    // Grid properties that need to be preserved after loading the simulation scene.
    public int gridWidth;
    public int gridHeight;
    public float gridSpread;
    public float frameDelay;
    public bool wrapAroundEdges;

    // Start is called before the first frame update
    public void Start()
    {
        // Make the object persist between scenes.
        DontDestroyOnLoad(gameObject);
    }

    // Save data to be restored later.
    public void SaveData(int width, int height, float spread, float delay, bool wrap)
    {
        gridWidth = width;
        gridHeight = height;
        gridSpread = spread;
        frameDelay = delay;
        wrapAroundEdges = wrap;
    }

}
