using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class dialogueNode
    {
        public string ID;
        public string text;
        public string[] children;
        public Rect rect = new Rect(0,0,200,100);
    }

}
