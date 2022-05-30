using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class pauseMenu : MonoBehaviour
{
    public bool isOptions;
    public bool isMainMenu;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Button()
    {
        if (isOptions)
        {
            
        }
        if (isMainMenu)
        {
            SceneManager.LoadScene("startmenu");
        }
    }
}
