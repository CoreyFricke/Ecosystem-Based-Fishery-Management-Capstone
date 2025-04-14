using UnityEngine;

public class PlantGenerator : MonoBehaviour
{
    //TODO: Dependancy on game manager's spawn zones (dynamically create children to avoid out of index errors)

    internal void GeneratePlants(Vector3[] vertices, float waterLine)
    {
        float density = 2.5f;

        int firstChildCount = transform.childCount;

        for(int i = 0; i < firstChildCount; i++)
        {
            while (transform.GetChild(i).childCount > 0)
            {
                DestroyImmediate(transform.GetChild(i).GetChild(0).gameObject);
            }
        }

        for (int i = 0, z = 0; z < GameManager.Instance.spawnZones.Length; z++)
        {
            for (int x = 0; x < vertices.Length; x++)
            {
                float curHeight = vertices[x].y;
                float zoneMinHeightl = waterLine + GameManager.Instance.spawnZones[z].minHeight;
                float zoneMaxHeightl = waterLine + GameManager.Instance.spawnZones[z].maxHeight;
                GameObject[] plantsToSpawn = GameManager.Instance.spawnZones[z].plantsToSpawn;

                if (curHeight > zoneMinHeightl && curHeight < zoneMaxHeightl && Random.Range(0f, 100f) < density)
                {
                    Vector3 pos = vertices[x];
                    GameObject objToSpawn = plantsToSpawn[i];
                    GameObject generatedTree = Instantiate(objToSpawn, pos - new Vector3(0, 0.25f, 0), Quaternion.identity);
                    generatedTree.transform.parent = transform.GetChild(z);
                    if (generatedTree.GetComponent<PlantStateManager>() != null)
                    {
                        generatedTree.GetComponent<PlantStateManager>().RotateToNormal();
                    }


                    if (i < plantsToSpawn.Length - 1)
                    {
                        i++;
                    }
                    else
                    {
                        i = 0;
                    }
                }
            }
        }
    }
}
