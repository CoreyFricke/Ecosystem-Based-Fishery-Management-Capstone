using UnityEngine;

public class FishLost : MonoBehaviour
{
    public int fishLost = 0;
    public GameObject sphere;

    public void CreateSphere(Vector3 rayHitLocation)
    {
        GameObject debugSphere = Instantiate(sphere, rayHitLocation, Quaternion.identity);
        debugSphere.transform.parent = GameObject.Find("FishSpawnHolder").transform;
        print(fishLost + " Fish Lost");
    }


}
