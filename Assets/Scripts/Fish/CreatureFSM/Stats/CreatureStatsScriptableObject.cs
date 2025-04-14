using UnityEngine;

[CreateAssetMenu(fileName = "CreatureStats", menuName = "ScriptableObjects/CreatureStatsScriptableObject", order = 1)]
public class CreatureStatsScriptableObject : ScriptableObject
{
    [Header("ID - Number")]
    [SerializeField] private int ID;
    public int id { get { return ID; } private set { ID = value; } }
    internal string category { get; private set; } = "Fish";

    [Header("Movement")]
    [SerializeField] private float MaxSpeed = 0;
    [SerializeField] private float Acceleration = 0;
    [SerializeField] private float RotationalSpeed = 0;

    public float maxSpeed { get { return MaxSpeed; } private set { MaxSpeed = value; } }
    public float acceleration { get { return Acceleration; } private set { Acceleration = value; } }
    public float rotationalSpeed { get { return RotationalSpeed; } private set { RotationalSpeed = value; } }


    [Header("Hunger")]
    [SerializeField] private float MaxHunger;
    [SerializeField] private float HuntingDistance;
    [SerializeField] private int[] Prey;
    public float maxHunger { get { return MaxHunger; } private set { MaxHunger = value; } }
    public float huntingDistance { get { return HuntingDistance; } private set { HuntingDistance = value; } }
    public int[] prey { get { return Prey; } private set { Prey = value; } }


    [Header("Hunger")]
    [SerializeField] private float MaxRepoNeed;
    [SerializeField] private float SearchDidstance;
    public float maxRepoNeed { get { return MaxRepoNeed; } private set { MaxRepoNeed = value; } }
    public float searchDidstance { get { return SearchDidstance; } private set { SearchDidstance = value; } }

    [Header("Energy")]
    [SerializeField] private float MaxEnergy;
    public float maxEnergy { get { return MaxEnergy; } private set { MaxEnergy = value; } }

    [Header("Age - In Minutes")]
    [SerializeField] private float MaxAge;
    public float maxAge { get { return MaxAge; } private set { MaxAge = value; } }
}
