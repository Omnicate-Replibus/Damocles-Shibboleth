using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class textwriter : MonoBehaviour
{
    public float letterPause = 0.2f;
    public TMP_Text theTMPgameobject;

    string message;
    TMP_Text textComp;

    // Use this for initialization
    public void TextScroll()
    {
        textComp = GameObject.Find("Renderer").GetComponent<TMP_Text>();
        message = "Undecidable";
        textComp.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            textComp.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
    }
}
