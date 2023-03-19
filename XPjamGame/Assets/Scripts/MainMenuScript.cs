using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void OnStartPressed()
    {
        SceneManager.LoadScene(1);
    }
}
