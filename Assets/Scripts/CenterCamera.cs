using UnityEngine;

public class CenterCamera : MonoBehaviour
{
    // As long as the camera doesn't clip cell objects on the Z axis, it should be fine.
    const float zPos = -10f;
    // Start is called before the first frame update
    public void CenterOnGrid(float gsx, float gsy, int gridWidth, int gridHeight)
    {
        // Place the camera in the middle of the grid.
        float xPos = .5f * gsx * (gridWidth - 1);
        float yPos = .5f * gsy * (gridHeight - 1);
        Vector3 cameraPos = new Vector3(xPos, yPos, zPos);
        transform.position = cameraPos;
        // Set the FOV to encompass the whole area.
        GetComponent<Camera>().orthographicSize = (gridWidth > gridHeight ? gridWidth : gridHeight);
    }

}
