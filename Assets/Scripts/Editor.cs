#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BombCasingGen))]
public class CQModuleEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Build Casing"))
        {
            ((BombCasingGen)target).BuildCasings();
        }
        if (GUILayout.Button("Build All Casings"))
        {
            ((BombCasingGen)target).build_all_casings();
        }
        
    }
}
#endif