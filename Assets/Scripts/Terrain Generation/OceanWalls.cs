using UnityEngine;

public class OceanWalls: MonoBehaviour
{
    [Header("OceanWalls")]
    private GameObject[] _oceanBounds = new GameObject[16];
    [SerializeField] Material oceanMat;
    [SerializeField] Material skyBoxMat;

    internal void SetOceanBounds(float xSize, float zSize, float _minTerrainHeight, float _maxTerrainHeight)
    {

        Vector3 landmassCenter = new Vector3(xSize / 2f, (_minTerrainHeight + _maxTerrainHeight) / 2f, zSize / 2f);

        if (!CheckIfCreated())
        {
            for (int i = 0; i < _oceanBounds.Length; i++)
            {
                _oceanBounds[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                _oceanBounds[i].transform.gameObject.GetComponent<MeshRenderer>().material = oceanMat;
                _oceanBounds[i].transform.parent = transform;
                _oceanBounds[i].transform.name = "Ocean Bound" + i.ToString();
            }
        }
        else
        {
            if (transform.childCount > 0)
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    if(transform.GetChild(i) != null)
                        _oceanBounds[i] = transform.GetChild(i).gameObject;
                }
            }
        }

        //OUTSIDE LAYER
        //X
        _oceanBounds[0].transform.position = landmassCenter + new Vector3(xSize / 2f, (_minTerrainHeight + _maxTerrainHeight) / 4f, 0);
        _oceanBounds[0].transform.rotation = Quaternion.Euler(0, -90f, -90f);
        _oceanBounds[0].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f, zSize, 1);
        _oceanBounds[0].GetComponent<MeshCollider>().enabled = false;
        _oceanBounds[0].layer = 3;

        //-X
        _oceanBounds[1].transform.position = landmassCenter + new Vector3(-xSize / 2f, (_minTerrainHeight + _maxTerrainHeight) / 4f, 0);
        _oceanBounds[1].transform.rotation = Quaternion.Euler(0, 90f, 90f);
        _oceanBounds[1].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f, zSize, 1);
        _oceanBounds[1].GetComponent<MeshCollider>().enabled = false;
        _oceanBounds[1].layer = 3;

        //Z
        _oceanBounds[2].transform.position = landmassCenter + new Vector3(0, (_minTerrainHeight + _maxTerrainHeight) / 4f, zSize / 2f);
        _oceanBounds[2].transform.rotation = Quaternion.Euler(0, 180f, 90f);
        _oceanBounds[2].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f, xSize, 1);
        _oceanBounds[2].GetComponent<MeshCollider>().enabled = false;
        _oceanBounds[2].layer = 3;

        //-Z
        _oceanBounds[3].transform.position = landmassCenter + new Vector3(0, (_minTerrainHeight + _maxTerrainHeight) / 4f, -zSize / 2f);
        _oceanBounds[3].transform.rotation = Quaternion.Euler(0, 0, -90f);
        _oceanBounds[3].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f, xSize, 1);
        _oceanBounds[3].GetComponent<MeshCollider>().enabled = false;
        _oceanBounds[3].layer = 3;

        //Y
        _oceanBounds[4].transform.position = landmassCenter;
        _oceanBounds[4].transform.rotation = Quaternion.Euler(90f, 0, 0);
        _oceanBounds[4].transform.localScale = new Vector3(xSize, zSize, 1);
        _oceanBounds[4].GetComponent<MeshCollider>().enabled = false;
        _oceanBounds[4].layer = 3;

        //INSIDE LAYER
        //X
        _oceanBounds[5].transform.position = landmassCenter + new Vector3(xSize / 2f, (_minTerrainHeight + _maxTerrainHeight) / 4f, 0);
        _oceanBounds[5].transform.rotation = Quaternion.Euler(0, 90f, -90f);
        _oceanBounds[5].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f, zSize, 1);
        _oceanBounds[5].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[5].layer = 3;

        //-X
        _oceanBounds[6].transform.position = landmassCenter + new Vector3(-xSize / 2f, (_minTerrainHeight + _maxTerrainHeight) / 4f, 0);
        _oceanBounds[6].transform.rotation = Quaternion.Euler(0, -90f, 90f);
        _oceanBounds[6].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f, zSize, 1);
        _oceanBounds[6].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[6].layer = 3;

        //Z
        _oceanBounds[7].transform.position = landmassCenter + new Vector3(0, (_minTerrainHeight + _maxTerrainHeight) / 4f, zSize / 2f);
        _oceanBounds[7].transform.rotation = Quaternion.Euler(0, 0, -90f);
        _oceanBounds[7].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f, xSize, 1);
        _oceanBounds[7].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[7].layer = 3;

        //-Z
        _oceanBounds[8].transform.position = landmassCenter + new Vector3(0, (_minTerrainHeight + _maxTerrainHeight) / 4f, -zSize / 2f);
        _oceanBounds[8].transform.rotation = Quaternion.Euler(0, 180f, 90f);
        _oceanBounds[8].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f, xSize, 1);
        _oceanBounds[8].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[8].layer = 3;

        //Y
        _oceanBounds[9].transform.position = landmassCenter;
        _oceanBounds[9].transform.rotation = Quaternion.Euler(-90f, 0, 0);
        _oceanBounds[9].transform.localScale = new Vector3(xSize, zSize, 1);
        _oceanBounds[9].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[9].layer = 3;

        //---------------------DEBUGGING----------------------------

        //DEBUG LAYER
        //X
        _oceanBounds[10].transform.position = landmassCenter - new Vector3(xSize / 1.5f, 0, 0);
        _oceanBounds[10].transform.rotation = Quaternion.Euler(0, -90f, -90f);
        _oceanBounds[10].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f * 2, zSize + (zSize * 0.335f), 1);
        _oceanBounds[10].transform.name = "FishBound_0";
        _oceanBounds[10].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[10].GetComponent<MeshRenderer>().material = skyBoxMat;
        _oceanBounds[10].tag = "FishBarrier";
        _oceanBounds[10].layer = 3;

        //-X
        _oceanBounds[11].transform.position = landmassCenter - new Vector3(-xSize / 1.5f, 0, 0);
        _oceanBounds[11].transform.rotation = Quaternion.Euler(0, 90f, 90f);
        _oceanBounds[11].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f * 2, zSize + (zSize * 0.335f), 1);
        _oceanBounds[11].transform.name = "FishBound_1";
        _oceanBounds[11].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[11].GetComponent<MeshRenderer>().material = skyBoxMat;
        _oceanBounds[11].tag = "FishBarrier";
        _oceanBounds[11].layer = 3;

        //Z
        _oceanBounds[12].transform.position = landmassCenter - new Vector3(0, 0, zSize / 1.5f);
        _oceanBounds[12].transform.rotation = Quaternion.Euler(0, 180f, 90f);
        _oceanBounds[12].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f * 2, xSize + (xSize * 0.335f), 1);
        _oceanBounds[12].transform.name = "FishBound_2";
        _oceanBounds[12].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[12].GetComponent<MeshRenderer>().material = skyBoxMat;
        _oceanBounds[12].tag = "FishBarrier";
        _oceanBounds[12].layer = 3;

        //-Z
        _oceanBounds[13].transform.position = landmassCenter - new Vector3(0, 0, -zSize / 1.5f);
        _oceanBounds[13].transform.rotation = Quaternion.Euler(0, 0, -90f);
        _oceanBounds[13].transform.localScale = new Vector3((_minTerrainHeight + _maxTerrainHeight) / 2f * 2, xSize + (xSize * 0.335f), 1);
        _oceanBounds[13].transform.name = "FishBound_3";
        _oceanBounds[13].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[13].GetComponent<MeshRenderer>().material = skyBoxMat;
        _oceanBounds[13].tag = "FishBarrier";
        _oceanBounds[13].layer = 3;

        //Y
        _oceanBounds[14].transform.position = landmassCenter - new Vector3(0, (_minTerrainHeight + _maxTerrainHeight) / 2f, 0);
        _oceanBounds[14].transform.rotation = Quaternion.Euler(-90f, 0, 0);
        _oceanBounds[14].transform.localScale = new Vector3(xSize + (xSize * 0.335f), zSize + (zSize * 0.335f), 1);
        _oceanBounds[14].transform.name = "FishBound_4";
        _oceanBounds[14].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[14].GetComponent<MeshRenderer>().material = skyBoxMat;
        _oceanBounds[14].tag = "FishBarrier";
        _oceanBounds[14].layer = 3;

        //-Y
        _oceanBounds[15].transform.position = landmassCenter + new Vector3(0, (_minTerrainHeight + _maxTerrainHeight) / 2f, 0);
        _oceanBounds[15].transform.rotation = Quaternion.Euler(90f, 0, 0);
        _oceanBounds[15].transform.localScale = new Vector3(xSize + (xSize * 0.335f), zSize + (zSize * 0.335f), 1);
        _oceanBounds[15].transform.name = "FishBound_5";
        _oceanBounds[15].GetComponent<MeshCollider>().enabled = true;
        _oceanBounds[15].GetComponent<MeshRenderer>().material = skyBoxMat;
        _oceanBounds[15].tag = "FishBarrier";
        _oceanBounds[15].layer = 3;

    }

    bool CheckIfCreated()
    {
        if(transform.childCount == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
