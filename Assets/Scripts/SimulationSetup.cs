using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimulationSetup : MonoBehaviour
{
    // Grid size limit.
    public const int maxGridSize = 20;
    // UI elements that can be modified by the user.
    public InputField widthField;
    public InputField heightField;
    public Slider gridSpreadSlider;
    public Slider frameDelaySlider;
    public Toggle wrapAroundEdges;
    // Error text to communicate with the user.
    public Text errorText;
    // Text next to sliders that will be updated.
    public Text gridSpreadText;
    public Text frameDelayText;
    // Help panel and help text.
    public GameObject helpPanel;
    public Text helpText;
    public TextAsset helpFile;
    // Exit panel.
    public GameObject exitPanel;
    // A script needed to preserve the settings and create a proper grid in the new scene.
    public DataHolderScript dataHolder;
    // An enum for specific error messages.
    public enum ErrorCode
    {
        NoError,
        SizeNotPositive,
        SizeTooBig
    };

    public void Start()
    {
        // Get object references.
        widthField = GameObject.Find("WidthField").GetComponent<InputField>();
        heightField = GameObject.Find("HeightField").GetComponent<InputField>();
        gridSpreadSlider = GameObject.Find("GridSpreadSlider").GetComponent<Slider>();
        frameDelaySlider = GameObject.Find("FrameDelaySlider").GetComponent<Slider>();
        wrapAroundEdges = GameObject.Find("WrapToggle").GetComponent<Toggle>();

        errorText = GameObject.Find("ErrorText").GetComponent<Text>();
        // Immediately hide the default error text until an error has been made.
        errorText.gameObject.SetActive(false);

        gridSpreadText = GameObject.Find("GridSpreadValue").GetComponent<Text>();
        frameDelayText = GameObject.Find("FrameDelayValue").GetComponent<Text>();

        helpPanel = GameObject.Find("HelpPanel");
        helpText = GameObject.Find("HelpText").GetComponent<Text>();
        // Hide help panel initially and load text from file.
        helpPanel.SetActive(false);
        helpText.text = helpFile.text;

        exitPanel = GameObject.Find("ConfirmExitPanel");
        // Hide exit panel.
        exitPanel.SetActive(false);

        dataHolder = GameObject.Find("DataHolder").GetComponent<DataHolderScript>();

        // Immediately update slider text to remove the placeholder '8's.
        UpdateSliderTextSpread();
        UpdateSliderTextDelay();
    }
    public void Update()
    {
        if( Input.GetKeyDown("escape") )
        {
            // Act as if the "Exit" button was clicked.
            ToggleExitPrompt();
        }
    }
    public void BeginSimulation()
    {
        // Parse the received data.
        int width;
        bool widthOkay = int.TryParse(widthField.text, out width);
        int height;
        bool heightOkay = int.TryParse(heightField.text, out height);
        float spread = gridSpreadSlider.value;
        float delay = frameDelaySlider.value;
        bool wrap = wrapAroundEdges.isOn;

        // Validate the data.
        if( !(widthOkay && heightOkay) )
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "An error occured while parsing grid size.";
        }
        else
        {
            ErrorCode e = DataIsValid(width, height);
            if (e == ErrorCode.NoError)
            {
                // Save settings chosen by the user.
                dataHolder.SaveData(width, height, spread, delay, wrap);
                // Load the simulation scene.
                SceneManager.LoadScene("SimulationScene");
            }
            else
            {
                // Inform the user which data should be corrected.
                errorText.gameObject.SetActive(true);
                switch (e)
                {
                    case ErrorCode.SizeNotPositive:
                        errorText.text = "Both width and height must be positive integers.";
                        break;
                    case ErrorCode.SizeTooBig:
                        errorText.text = string.Format("Grid size cannot be bigger than {0}.", maxGridSize);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ToggleHelpDisplay()
    {
        helpPanel.SetActive(!helpPanel.activeSelf);
    }

    public void ToggleExitPrompt()
    {
        exitPanel.SetActive(!exitPanel.activeSelf);
    }

    // Make sure the grid size is positive.
    public ErrorCode DataIsValid(int width, int height)
    {
        if (width <= 0 && height <= 0)
        {
            return ErrorCode.SizeNotPositive;
        }
        if (width > maxGridSize || height > maxGridSize)
        {
            return ErrorCode.SizeTooBig;
        }
        return ErrorCode.NoError;
    }
    // Display the truncated value of a slider in text next to it.
    public void UpdateSliderTextSpread()
    {
        double val = gridSpreadSlider.value;
        gridSpreadText.text = string.Format("{0:#0.00}", val);
    }
    public void UpdateSliderTextDelay()
    {
        double val = frameDelaySlider.value;
        frameDelayText.text = string.Format("{0:#0.00}s", val);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
