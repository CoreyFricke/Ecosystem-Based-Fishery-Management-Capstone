using UnityEngine;

[CreateAssetMenu(fileName = "PlantStats", menuName = "ScriptableObjects/PlantStatsScriptableObject", order = 2)]
public class PlantStatsScriptableObject : ScriptableObject
{
    [Header("ID - Number")]
    [SerializeField] private int ID;
    public int id { get { return ID; } private set { ID = value; } }
    internal string category { get; private set; } = "Plant";

    [Header("Age - In Minutes")]
    [SerializeField] private float maxAge;
}
