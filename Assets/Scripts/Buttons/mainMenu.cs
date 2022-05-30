using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class mainMenu : MonoBehaviour
{
    public bool isStart;
    public bool isQuit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Button()
    {
        if (isStart)
        {
            SceneManager.LoadScene("mainscene");
        }
        if (isQuit)
        {
            Application.Quit();
        }
    }
}
