using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Linq;

namespace Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [NonSerialized]
        GUIStyle nodeStyle = null;
        [NonSerialized]
        GUIStyle selectedNodeStyle = null;
        [NonSerialized]
        GUIStyle playerNodeStyle = null;
        [NonSerialized]
        GUIStyle selectedplayerNodeStyle = null;
        [NonSerialized]
        DialogueNode draggingNode = null;
        [NonSerialized]
        Vector2 draggingOffset;
        [NonSerialized]
        DialogueNode creatingNode = null;
        [NonSerialized]
        DialogueNode deletingNode = null;
        [NonSerialized]
        DialogueNode linkingParentNode = null;
        Vector2 scrollPosition;
        [NonSerialized]
        List<float> nodesX = new List<float>() { 1000f };
        [NonSerialized]
        List<float> nodesY = new List<float>() { 1000f };
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;
        [NonSerialized]
        string selectedDialogueNodeName = null;

        const float backgroundImageSize = 50f;

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

            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load("node0 on") as Texture2D;
            selectedNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);

            selectedplayerNodeStyle = new GUIStyle();
            selectedplayerNodeStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
            selectedplayerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            selectedplayerNodeStyle.border = new RectOffset(12, 12, 12, 12);

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
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected");
            }
            else
            {
                ProcessEvents();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                float xMax = nodesX.Max();
                float yMax = nodesY.Max();

                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, (xMax+50f)/backgroundImageSize, (yMax + 50f) / backgroundImageSize);

                Rect canvas = GUILayoutUtility.GetRect(xMax+50f,yMax+50f);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if(creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode != null)
                {
                    selectedDialogueNodeName = draggingNode.name;
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                    GUI.changed = true;
                }
                else
                {
                    selectedDialogueNodeName= null;
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                    GUI.changed = true;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }

        private void DrawNode(DialogueNode node)
        {
            nodesX.Add(node.GetRect().xMax);
            nodesY.Add(node.GetRect().yMax);
            GUIStyle style = new GUIStyle();
            if (node.name == selectedDialogueNodeName) //maybe make a variable for "style" then change it depending on who's talking?
            {
                if (node.IsPlayerSpeaking())
                {
                    style = selectedplayerNodeStyle;
                }
                else
                {
                    style = selectedNodeStyle;
                }
            }
            else
            {
                if (node.IsPlayerSpeaking())
                {
                    style = playerNodeStyle;
                }
                else
                {
                    style = nodeStyle;
                }
            }
            GUILayout.BeginArea(node.GetRect(), style);

            node.SetText(EditorGUILayout.TextField(node.GetText()));

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("-"))
            {
                deletingNode = node;
            }
            DrawLinkButtons(node);
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    Undo.RecordObject(selectedDialogue, "Linked Nodes");
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("Unlink"))
                {
                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax - 6, node.GetRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin + 6,childNode.GetRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.6f;
                if(node.name == selectedDialogueNodeName | selectedDialogueNodeName == null)
                {
                    Handles.DrawBezier(
                        startPosition, endPosition,
                        startPosition + controlPointOffset,
                        endPosition - controlPointOffset,
                        Color.white, null, 4f);
                }
                else
                {
                    Handles.DrawBezier(
                        startPosition, endPosition,
                        startPosition + controlPointOffset,
                        endPosition - controlPointOffset,
                        Color.gray, null, 4f);
                }
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode chosenNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if(node.GetRect().Contains(point) == true)
                {
                    chosenNode = node;
                }
            }
            return chosenNode;
        }

    }

}
