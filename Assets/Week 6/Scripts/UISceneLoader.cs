using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneLoader : MonoBehaviour
{
    public void SetRes16x9()
    {
        Screen.SetResolution(1600, 900, false);
        Debug.Log("Setting Res to 16:9");
    }
    public void SetResFullHD()
    {
        Screen.SetResolution(1920, 1080, false);
        Debug.Log("Setting Res to Full HD");
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(3);
    }
}
