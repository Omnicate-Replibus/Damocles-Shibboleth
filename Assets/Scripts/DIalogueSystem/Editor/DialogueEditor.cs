using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        GUIStyle nodeStyle = null;
        bool dragging = false;
        dialogueNode draggingNode = null;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is Dialogue dialogue)
            {
                ShowEditorWindow();
                return true;
            }
            return false;

        }
        private void OnEnable() 
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            Dialogue newDialogue = Selection.activeObject as Dialogue; // if this isn't here then you need to                                                       
            if (newDialogue != null)                                   // click off the dialogue box and then
            {                                                          // back onto it in order to load the 
                selectedDialogue = newDialogue;                        // dialogue - it just saves a little tedium
            }
        }

        private void OnSelectionChange()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            Debug.Log(selectedDialogue);
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected");
            }
            else
            {
                ProcessEvents();
                foreach(dialogueNode node in selectedDialogue.GetAllNodes())
                {
                    OnGUINode(node);
                }
            }
        }

        private void ProcessEvents()
        {
            if(Event.current.type == EventType.MouseDown && !dragging)
            {
                dragging = true;
            }
            else if (Event.current.type == EventType.MouseDrag && dragging)
            {
                Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
                selectedDialogue.GetRootNode().rect.position = Event.current.mousePosition;
                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseUp && dragging)
            {
                dragging = false;
            }
        }

        private void OnGUINode(dialogueNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node:",EditorStyles.boldLabel);
            string newText = EditorGUILayout.TextField(node.text);
            string newID = EditorGUILayout.TextField(node.ID);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                node.text = newText;
                node.ID = newID;
            }
            GUILayout.EndArea();
        }
    }

}
