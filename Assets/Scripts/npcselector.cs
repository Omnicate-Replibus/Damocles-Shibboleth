using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcselector : MonoBehaviour
{
    public GameObject interactKey;
    private bool talking;
    // Start is called before the first frame update
    void Start()
    {
        var talking = false;
    }

    // Update is called once per frame
    void Update()
    {
        var target = GameObject.Find("Game Camera - RENDERS TO TEXTURE").GetComponent<raycast>().obj;
        var pause = GameObject.Find("UI").GetComponent<UIcontrol>().paused;

        if (target.CompareTag("NPC") && pause == false && talking == false)
        {
            interactKey.SetActive(true);
            target.GetComponent<Outline>().enabled = true;
  

            if (Input.GetKeyDown("e"))
            {
                talking = true;
                GameObject.Find("DialogueOrganizer").GetComponent<textwriter>().TextScroll();
            }
        }
        else
        {
            interactKey.SetActive(false);
        }
    }
}
