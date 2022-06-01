using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach(string childID in parentNode.GetChildren())
            {
                if(nodeLookup.ContainsKey(childID))
                    yield return nodeLookup[childID];
            }
        }

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode childNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(childNode, "Created Node");
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                Undo.RecordObject(this, "Created Dialogue Node");
            }
            AddNode(childNode);
        }

        private static DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode childNode = CreateInstance<DialogueNode>();
            childNode.name = Guid.NewGuid().ToString();
            if (parent != null)
            {
                childNode.SetRect(new Rect(parent.GetRect().xMax + 50f, parent.GetRect().y, parent.GetRect().width, parent.GetRect().height));
                parent.AddChild(childNode.name);
            }

            return childNode;
        }

        private void AddNode(DialogueNode childNode)
        {
            nodes.Add(childNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }
#endif

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogueNode childNode = MakeNode(null);
                AddNode(childNode);
            }
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
        }
#endif
    }

}
