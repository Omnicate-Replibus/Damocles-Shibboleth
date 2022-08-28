using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        bool isPlayerSpeaking = false; //turn this into an enum if there are more than 2 people speaking
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
        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }
        public void SetRect(Rect newRect)
        {
            Undo.RecordObject(this, "Set Size Of Node");
            rect = newRect;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if(newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }
        public void AddChild(string childName)
        {
            Undo.RecordObject(this, "Linked Nodes");
            children.Add(childName);
            EditorUtility.SetDirty(this);
        }
        public void RemoveChild(string childName)
        {
            Undo.RecordObject(this, "Unlinked Nodes");
            children.Remove(childName);
            EditorUtility.SetDirty(this);
        }

        public void SetPlayerSpeaking(bool newSpeaker)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            isPlayerSpeaking = newSpeaker;
            EditorUtility.SetDirty(this);
        }
#endif
    }

}
