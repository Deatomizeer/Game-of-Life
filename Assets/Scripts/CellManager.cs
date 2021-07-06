using UnityEngine;

public class CellManager : MonoBehaviour
{
    // An array storing the materials for cell's states (alive/dead).
    public Material[] materials = new Material[2];
    // Whether the cell is alive; also doubles as index for the `materials` array.
    public int alive;
    // Cell's renderer reference to swap its material when switching states.
    Renderer renderer;
    // Manager script, used to validate clicking on the individual cubes.
    GameManagerScript managerScript;

    // Start is called before the first frame update
    void Start()
    {
        alive = 0;
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        renderer.sharedMaterial = materials[alive];
        managerScript = GameObject.Find("GameOfLifeManager").GetComponent<GameManagerScript>();
    }

    // Toggle the life of the selected cell. Works only when the game is paused.
    private void OnMouseDown()
    {
        if (!managerScript.running)
        {
            alive = (alive + 1) % 2;
            renderer.sharedMaterial = materials[alive];
        }
    }

    // Set the cell to be either alive or dead. `state` can be either 0 or 1.
    public void setState(int state)
    {
        Debug.Assert(state == 0 || state == 1);
        alive = state;
        renderer.sharedMaterial = materials[state];
    }
}
