using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationSetup : MonoBehaviour
{
    public void BeginSimulation()
    {
        SceneManager.LoadScene("SimulationScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
