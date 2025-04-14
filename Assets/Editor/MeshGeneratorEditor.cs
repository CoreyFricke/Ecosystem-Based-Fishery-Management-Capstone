using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshGenerator meshGenerator = (MeshGenerator)target;

        //Calls these functions on value changed
        if (DrawDefaultInspector())
        {
            if (meshGenerator.autoUpdate)
            {
                meshGenerator.CreateShape();
                meshGenerator.UpdateMesh();
            }
        }

        if (GUILayout.Button("Update Terrain Mesh"))
        {
            meshGenerator.CreateShape();
            meshGenerator.UpdateMesh();
        }
    }
}
