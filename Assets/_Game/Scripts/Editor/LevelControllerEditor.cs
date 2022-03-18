using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelController))]
public class LevelControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelController myScript = (LevelController)target;

        if (GUILayout.Button("Save Level Controller")) { }
            //myScript.SaveLevelDatasToJSON();

        if (GUILayout.Button("Load Level Controller")) { }
            //myScript.SaveLevelDatasToJSON();
    }
}
