using TMPro;
using UnityEditor;
using UnityEditor.UI;

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