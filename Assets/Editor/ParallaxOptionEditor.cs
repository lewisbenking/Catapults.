using UnityEditor;
using UnityEngine;

/// <summary>A custom editor for parallax</summary>
/// <author>Marks Paskannijs</author>
[CustomEditor(typeof(ParallaxOption))]
public class ParallaxOptionEditor : Editor {

    private ParallaxOption options;

    private void Awake()
    {
        options = (ParallaxOption)target;
    }

    // We need to override the defaults, otherwise there's no point having a custom editor.
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Save Position"))
        {
            options.SavePosition();
            EditorUtility.SetDirty(options);
        }

        if (GUILayout.Button("Restore Position"))
            options.RestorePosition();
    }
}
