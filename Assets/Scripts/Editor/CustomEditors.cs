using TMPro;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

// Main class to host all classes related to Editors/EditorWindows


/// <summary>
/// Editor for title screen buttons
/// </summary>
[CustomEditor(typeof(TitleScreenButton))]
public class TitleScreenButtonEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        TitleScreenButton targetButton = (TitleScreenButton) target;

        targetButton.label = (TMP_Text)EditorGUILayout.ObjectField("Label:", targetButton.label, typeof(TMP_Text), true);
        targetButton.text = EditorGUILayout.TextField("Text:", targetButton.text);
        
        base.OnInspectorGUI();
    }
}

/// <summary>
/// Editor for title screen buttons
/// </summary>
[CustomEditor(typeof(PuzzleButton))]
public class PuzzleButtonEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        PuzzleButton targetButton = (PuzzleButton) target;

        if (EditorGUILayout.LinkButton("Remove puzzle"))
        {
            targetButton.puzzle = null;
        }
        
        targetButton.title = (TMP_Text) EditorGUILayout.ObjectField("Title", targetButton.title, typeof(TMP_Text), true);
        targetButton.image = (Image) EditorGUILayout.ObjectField("Image", targetButton.image, typeof(Image), true);
        targetButton.puzzle = (Puzzle) EditorGUILayout.ObjectField("Puzzle", targetButton.puzzle, typeof(Puzzle), true);
        targetButton.comingSoonSprite = (Sprite) EditorGUILayout.ObjectField("Coming Soon", targetButton.comingSoonSprite, typeof(Sprite), true);
        
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(BasePiece), true)]
public class BasePieceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var piece = (BasePiece) target;
        
        EditorGUILayout.LabelField($"Level {piece.level}");
        //base.OnInspectorGUI();
    }
}