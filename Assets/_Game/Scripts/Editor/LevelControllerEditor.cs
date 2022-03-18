using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelController))]
public class LevelControllerEditor : Editor
{
    string path = "C:/BubblePopper/";
    string fileName = "level-controller-data.json";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelController myScript = (LevelController)target;

        EditorGUILayout.LabelField($"Path to save and load from");
        EditorGUILayout.LabelField(path+fileName);
        //path = EditorGUILayout.TextField(path + fileName);

        if (GUILayout.Button("Save Level Controller"))
            myScript.Save(path+fileName,path);

        if (GUILayout.Button("Load Level Controller"))
            myScript.Load(path);
    }
}