using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CreatureNav
{
    //TODO - Optomize ray generation (Major FPS hit from running each frame)
    //Use coroutines to delay updates?

    internal MeshCollider meshCollider { get; set; }
    internal Transform navTransform { get; set; }
    private Vector3[] maxVertices;

    internal bool noRaysActive { get; private set; } = true;
    internal int currentRay { get; private set; } = 0;
    internal RaycastHit raycastHit { get; private set; }

    LayerMask layerMaskToHit = LayerMask.GetMask("Terrain");

    //TestingNewNav
    private bool _FirstTimeGeneratingRays = true;
    private GameObject[] _navPoints = new GameObject[6];

    //DEBUGGING -------------------------------------------
    bool addedFishLost = false;
    void FishLostDebugging(RaycastHit hit)
    {
        if (!addedFishLost && hit.transform.tag == "FishBarrier" && hit.collider.gameObject.GetComponentInParent<FishLost>() != null)
        {
            FishLost fishBarrier = hit.collider.gameObject.GetComponentInParent<FishLost>();
            fishBarrier.fishLost++;
            fishBarrier.CreateSphere(hit.point);
            addedFishLost = true;

            navTransform.position = GameObject.Find("FishSpawnHolder").transform.position;
        }
        else if (addedFishLost && hit.transform.tag == "FishBarrier" && hit.collider.gameObject.GetComponentInParent<FishLost>() != null)
        {
            navTransform.position = GameObject.Find("FishSpawnHolder").transform.position;
        }
    }

    void RepositionVertices()
    {
        if (_FirstTimeGeneratingRays)
        {
            CreatureStateManager manager = navTransform.GetComponent<CreatureStateManager>();

            Quaternion rotation = navTransform.rotation;
            Vector3[] vertices = meshCollider.sharedMesh.vertices;

            maxVertices = IsolateAxisBounds(vertices);

            for (int i = 0; i < maxVertices.Length; i++)
            {
                float temp = maxVertices[i].x;
                maxVertices[i].x = maxVertices[i].z;
                maxVertices[i].z = -temp;
            }

            for (int n = 0; n < maxVertices.Length; n++)
            {

                maxVertices[n] = rotation * (maxVertices[n] - new Vector3(0, 0, 0)) + new Vector3(0, 0, 0);

            }

            //for(int i = 0; i < maxVertices.Length; i++)
            //{
            //    _navPoints[i] = manager.InstantiateNewCreature(manager.navTester, maxVertices[i] + navTransform.position, Quaternion.identity);
            //    _navPoints[i].transform.parent = manager.transform;
            //    _navPoints[i].GetComponent<MeshRenderer>().enabled = true;
            //}

            //_FirstTimeGeneratingRays = false;
        }
    }
    Vector3[] IsolateAxisBounds(Vector3[] meshVertices)
    {
        float whiskerLength = 0.3f;

        //Max
        Vector3 maxX = new Vector3(0, 0, 0);
        Vector3 maxY = new Vector3(0, 0, 0);
        Vector3 maxZ = new Vector3(0, 0, 0);

        //Min
        Vector3 minX = new Vector3(0, 0, 0);
        Vector3 minY = new Vector3(0, 0, 0);
        Vector3 minZ = new Vector3(0, 0, 0);

        for (int i = 0; i < meshVertices.Length; i++)
        {
            if (meshVertices[i].x > maxX.x)
                maxX = meshVertices[i];

            if (meshVertices[i].x < minX.x)
                minX = meshVertices[i];

            if (meshVertices[i].y > maxY.y)
                maxY = meshVertices[i];

            if (meshVertices[i].y < minY.y)
                minY = meshVertices[i];

            if (meshVertices[i].z > maxZ.z)
                maxZ = meshVertices[i];

            if (meshVertices[i].z < minZ.z)
                minZ = meshVertices[i];
        }

        Vector3[] maxVertices = new Vector3[] {
            maxX + new Vector3(whiskerLength, 0, 0),
            maxY + new Vector3(0, whiskerLength, 0),
            maxZ + new Vector3(0, 0, whiskerLength),
            minX + new Vector3(-whiskerLength, 0, 0),
            minY + new Vector3(0, -whiskerLength, 0),
            minZ + new Vector3(0, 0, -whiskerLength)
        };

        return maxVertices;
    }

    public void GenerateNaviagtionRays()
    {
        RepositionVertices();

        Ray[] rays = new Ray[] {
            //X -> Back: Currently not in use
            new Ray(navTransform.position, maxVertices[0]),
            //Y -> Down
            new Ray(navTransform.position, maxVertices[1]),
            //Z -> Right
            new Ray(navTransform.position, maxVertices[2]),
            //-X -> Front
            new Ray(navTransform.position, maxVertices[3]),
            //-Y -> Up
            new Ray(navTransform.position, maxVertices[4]),
            //-Z -> Left
            new Ray(navTransform.position, maxVertices[5])

            ////X -> Back: Currently not in use
            //new Ray(navTransform.position, _navPoints[0].transform.localPosition),
            ////Y -> Down
            //new Ray(navTransform.position, _navPoints[1].transform.localPosition),
            ////Z -> Right
            //new Ray(navTransform.position, _navPoints[2].transform.localPosition),
            ////-X -> Front
            //new Ray(navTransform.position, _navPoints[3].transform.localPosition),
            ////-Y -> Up
            //new Ray(navTransform.position, _navPoints[4].transform.localPosition),
            ////-Z -> Left
            //new Ray(navTransform.position, _navPoints[5].transform.localPosition)
        };

        if (noRaysActive)
        {
            for (int i = 1; i < rays.Length; i++)
            {
                if (Physics.Raycast(rays[i], out RaycastHit hit, Vector3.Distance(navTransform.position, navTransform.position + maxVertices[i]), layerMaskToHit))
                //if (Physics.Raycast(rays[i], out RaycastHit hit, Vector3.Distance(navTransform.position, _navPoints[i].transform.position), layerMaskToHit))
                {
                    FishLostDebugging(hit);
                    raycastHit = hit;
                    currentRay = i;
                    noRaysActive = false;
                    break;
                }
                else
                {
                    noRaysActive = true;
                }
            }
        }
        else
        {
            if (Physics.Raycast(rays[currentRay], out RaycastHit hit, Vector3.Distance(navTransform.position, navTransform.position + maxVertices[currentRay]), layerMaskToHit))
            //if (Physics.Raycast(rays[currentRay], out RaycastHit hit, Vector3.Distance(navTransform.position, _navPoints[currentRay].transform.position), layerMaskToHit))
            {
                raycastHit = hit;
                noRaysActive = false;
            }
            else
            {
                noRaysActive = true;
            }
        }
    }
    internal void RaycastGizmos()
    {
        if (maxVertices != null)
        {
            //Draw end of ray
            for (int i = 1; i < maxVertices.Length; i++)
            {
                Vector3 vertex = (maxVertices[i] + navTransform.position);
                //Vector3 vertex = (_navPoints[i].transform.position);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(vertex, 0.01f);
            }
            //Draw ray
            for (int i = 1; i < maxVertices.Length; i++)
            {
                Vector3 vertex = (maxVertices[i] + navTransform.position);
                //Vector3 vertex = (_navPoints[i].transform.position);

                Gizmos.color = Color.green;

                Gizmos.DrawLine(navTransform.position, vertex);
            }
        }
    }
}
