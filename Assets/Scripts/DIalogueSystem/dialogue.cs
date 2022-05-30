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

#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new dialogueNode());
            }
        }
        public IEnumerable<dialogueNode> GetAllNodes()
        {
            return nodes;
        }
    }
#endif
}
