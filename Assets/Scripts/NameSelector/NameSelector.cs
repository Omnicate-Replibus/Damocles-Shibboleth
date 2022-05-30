using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class NameSelector : MonoBehaviour
{
    // Wordle variables
    public List<Transform> wordBoxes = new List<Transform>();
    public GameObject TextAnimator;
    int currentWordBox = 0;
    string playerGuess = "";
    int visibleGuesses = 4;         // the max number of guesses visible on screen at any given time, including the current guess
    private string characterNames = "QWERTYUIOPASDFGHJKLZXCVBNM";
    string correctGuess = "MARTIN";
    bool solved = false;

    public void AddLetterToWordBox(string letter)
    {

        if (currentWordBox <= 5)
        {
            playerGuess += letter;
            wordBoxes[currentWordBox].GetChild(0).GetComponent<TextMeshProUGUI>().text = letter;
            currentWordBox++;
        }

    }
    
    void BackSpace()
    {
        if(currentWordBox > 0)
        {
            currentWordBox--;
            wordBoxes[currentWordBox].GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            playerGuess = playerGuess.Remove(playerGuess.Length - 1, 1);
        }
    }

    void Return()
    {
        if (playerGuess == correctGuess && solved == false)
        {
            solved = true;
            OpeningAnimation();
        }
        else if(solved == false)
        {
            playerGuess = playerGuess.PadRight(6);
            char[] playArr = playerGuess.ToCharArray();
            List<int> loci = new List<int>();
            List<int> nearly = new List<int>();

            for (int k = 0; k < correctGuess.Length; k++)
            { 
                if(correctGuess[k] == playerGuess[k])
                {
                    loci.Add(k);
                }
                else if(playArr.Contains(correctGuess[k]))
                {
                    int l = playerGuess.IndexOf(correctGuess[k]);
                    nearly.Add(l);
                }
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = visibleGuesses - 1; j > 0; j--)
                {
                    wordBoxes[i + 6 * j].GetChild(0).GetComponent<TextMeshProUGUI>().text = wordBoxes[i + 6 * (j - 1)].GetChild(0).GetComponent<TextMeshProUGUI>().text;
                    wordBoxes[i + 6 * j].GetChild(0).GetComponent<TextMeshProUGUI>().color = wordBoxes[i + 6 * (j - 1)].GetChild(0).GetComponent<TextMeshProUGUI>().color;
                }
                if(loci.Contains(i))
                {
                    wordBoxes[i+6].GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
                }
                else if (nearly.Contains(i))
                {
                    wordBoxes[i+6].GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.yellow;
                }
                wordBoxes[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                wordBoxes[i].GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                currentWordBox = 0;
                playerGuess = "";
            }
        }
    }

    void Update()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey) && solved == false)
            {
                string p = "" + vKey;
                if(p == "Backspace")
                {
                    BackSpace();
                }
                else if(p == "Return")
                {
                    Return();
                }
                else if(characterNames.Contains(p))
                {
                    AddLetterToWordBox(p);
                }
            }
        }
    }
    public async void OpeningAnimation()
    {
        await Task.Delay(2000);
        for (int i = 0; i < 6; i++)
        {
            wordBoxes[i].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            for (int j = visibleGuesses - 1; j > 0; j--)
            {
                wordBoxes[i + 6 * j].GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
            }
        }

        await Task.Delay(2000);

        for (int i = 0; i < 6; i++)
        {
            wordBoxes[i].GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
        }

        await Task.Delay(2000);
        TextAnimator.GetComponent<textAnimator>().StartText();
    }
   
}