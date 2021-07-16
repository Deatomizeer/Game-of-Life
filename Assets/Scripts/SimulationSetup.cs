using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimulationSetup : MonoBehaviour
{
    // UI elements that can be modified by the user.
    public InputField widthField;
    public InputField heightField;
    public Slider gridSpreadSlider;
    public Slider frameDelaySlider;
    // A script needed to preserve the settings and create a proper grid in the new scene.
    public DataHolderScript dataHolder;

    public void Start()
    {
        // Get object references.
        widthField = GameObject.Find("WidthField").GetComponent<InputField>();
        heightField = GameObject.Find("HeightField").GetComponent<InputField>();
        gridSpreadSlider = GameObject.Find("GridSpreadSlider").GetComponent<Slider>();
        frameDelaySlider = GameObject.Find("FrameDelaySlider").GetComponent<Slider>();
        dataHolder = GameObject.Find("DataHolder").GetComponent<DataHolderScript>();
    }
    public void BeginSimulation()
    {
        // Parse the received data.
        int width = int.Parse(widthField.text);
        int height = int.Parse(heightField.text);
        float spread = gridSpreadSlider.value;
        float delay = frameDelaySlider.value;
        // Save settings chosen by the user.
        dataHolder.SaveData(width, height, spread, delay);
        // Load the simulation scene.
        SceneManager.LoadScene("SimulationScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
