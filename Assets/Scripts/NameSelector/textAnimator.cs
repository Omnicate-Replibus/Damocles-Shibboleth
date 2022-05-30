using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;

public class textAnimator : MonoBehaviour {

    public GameObject textBox1; // "Your name is "
    public GameObject textBox2; // "Dear Martin"
    public GameObject textBox3; // "Monday 9th"
    public GameObject textBox4; // "8:30am"

    public void Start()
    {
        textBox1.GetComponent<TextMeshProUGUI>().text = "Your name is";
        textBox1.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.BottomLeft;
        textBox2.GetComponent<TextMeshProUGUI>().text = "";
        GetComponent<ScreenFader>().enabled = false;
    }

    public async void StartText()
    {
        Dictionary<string, int> stringDict = new Dictionary<string, int> { { "Your name is Martin.", 4 },
            { "Two weeks ago, you received a strange envelope.", 4 }, { "Inside it was a map and a handwritten note.", 4 } };

        for (int i = 0; i < stringDict.Count(); i++)
        {
            textBox1.GetComponent<TextMeshProUGUI>().text = stringDict.ElementAt(i).Key;
            await Task.Delay(stringDict.ElementAt(i).Value * 1000);
            textBox1.GetComponent<TextMeshProUGUI>().text = "";
            await Task.Delay(2000);
        }

        await Task.Delay(2000);

        Dictionary<string, int> stringDict2 = new Dictionary<string, int> { { "Meet at 10am on Monday 9th at the location marked.", 4 },
            { "\n\n\nBring nobody and nothing with you except clothes and money.", 4 }, { "\n\n\nIt will all be over in five days.", 3 }, 
            { "\n\n\n-D", 6 } };

        for (int i = 0; i < stringDict2.Count(); i++)
        {
            textBox2.GetComponent<TextMeshProUGUI>().text += stringDict2.ElementAt(i).Key;
            await Task.Delay(stringDict2.ElementAt(i).Value * 1000);
        }

        textBox2.GetComponent<TextMeshProUGUI>().text = "";
        await Task.Delay(2000);

        textBox3.GetComponent<TextMeshProUGUI>().text = "Monday 9th";
        await Task.Delay(2000);
        textBox4.GetComponent<TextMeshProUGUI>().text += "8:30am";
        await Task.Delay(2000);

        SceneManager.LoadScene("mainscene");

        //GetComponent<ScreenFader>().enabled = true;
        //StartCoroutine(GetComponent<ScreenFader>().FadeAndLoadScene(ScreenFader.FadeDirection.Out, "mainscene"));
    }

}
