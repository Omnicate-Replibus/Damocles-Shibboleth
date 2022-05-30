using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontrol : MonoBehaviour
{
    GameObject[] pauseObjects;
    GameObject[] hudObjects;

    public bool paused;
    // Start is called before the first frame update
    void Start()
    {
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hudObjects = GameObject.FindGameObjectsWithTag("HUD");
        hidePaused();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused == false)
            {
                paused = true;
                Cursor.lockState = CursorLockMode.None;
                showPaused();
            }
            else if(paused == true)
            {
                paused = false;
                Cursor.lockState = CursorLockMode.Locked;
                hidePaused();
            }
        }
    }

    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
        foreach (GameObject h in hudObjects)
        {
            h.SetActive(false);
        }
    }

    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
        foreach (GameObject h in hudObjects)
        {
            h.SetActive(true);
        }
    }
}
