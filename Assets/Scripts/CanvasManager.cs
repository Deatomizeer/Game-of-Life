using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    // Icon that shows whether the simulation is running.
    GameObject runningImage;
    public Sprite[] runningSprites = new Sprite[2];
    // Start is called before the first frame update
    void Start()
    {
        runningImage = GameObject.Find("RunningImage");
    }

    // Change the image after the simulation has been paused or resumed.
    public void SetRunningImage(int index)
    {
        runningImage.GetComponent<Image>().overrideSprite = runningSprites[index];
    }
}
