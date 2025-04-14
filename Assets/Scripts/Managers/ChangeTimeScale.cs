using UnityEngine;

public class ChangeTimeScale : MonoBehaviour
{
    [Header("Current Time Scale")]
    [Range(0,5)]
    [SerializeField] float curTimeScale;

    private void Start()
    {
        curTimeScale = 1f;
    }
    void Update()
    {
        Time.timeScale = curTimeScale;
    }
}
