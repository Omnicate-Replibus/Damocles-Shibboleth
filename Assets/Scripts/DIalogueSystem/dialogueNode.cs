using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] 
        string text;
        [SerializeField] 
        List<string> children = new List<string>();
        [SerializeField] 
        Rect rect = new Rect(0,0,200,100);

        public Rect GetRect()
        {
            return rect;
        }
        public string GetText()
        {
            return text;
        }
        public List<string> GetChildren()
        {
            return children;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
        }
        public void SetRect(Rect newRect)
        {
            Undo.RecordObject(this, "Set Size Of Node");
            rect = newRect;
        }

        public void SetText(string newText)
        {
            if(newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
            }
        }
        public void AddChild(string childName)
        {
            Undo.RecordObject(this, "Linked Nodes");
            children.Add(childName);
        }
        public void RemoveChild(string childName)
        {
            Undo.RecordObject(this, "Unlinked Nodes");
            children.Remove(childName);
        }
#endif
    }

}
