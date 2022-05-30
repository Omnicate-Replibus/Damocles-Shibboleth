using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        List<dialogueNode> nodes = new List<dialogueNode>();

        private void Awake()
        {
            
        }
    }
}
