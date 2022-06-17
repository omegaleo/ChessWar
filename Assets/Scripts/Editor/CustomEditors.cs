using TMPro;
using UnityEditor;
using UnityEditor.UI;

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