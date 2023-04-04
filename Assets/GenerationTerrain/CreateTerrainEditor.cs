using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateTerrain))]
public class CreateTerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CreateTerrain terrain = (CreateTerrain)target;

        if (GUILayout.Button("Generate Terrain"))
        {
            terrain.GenerateTerrain();
        }
    }
}
