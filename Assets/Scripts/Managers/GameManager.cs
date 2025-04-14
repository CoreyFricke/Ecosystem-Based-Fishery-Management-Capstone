using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TODO: Either remove the zone declerations from here or make this initilize without having to run simulation
    public static GameManager Instance { get; set; }

    [Header("Spawning Zones")]
    [SerializeField] PlantSpawnZone[] SpawnZones;
    public PlantSpawnZone[] spawnZones { get { return SpawnZones; } private set { SpawnZones = value; } }

    [Header("Creature Population Limits")]
    [SerializeField] CreatureLimit[] CreatureLimits;
    public CreatureLimit[] creatureLimits { get { return CreatureLimits; } private set { CreatureLimits = value; } }

    [Header("Population Line Graph")]
    [SerializeField] GameObject WindowGraph;
    public GameObject windowGraph { get { return WindowGraph; } private set { WindowGraph = value; } }
    [Header("Population Colors")]
    [SerializeField] Color[] PopColors;
    public Color[] popColors { get { return PopColors; } private set { PopColors = value; } }

    private Window_Graph _windowGraph;
    private float _updateGraphTime = 120f;
    private List<List<int>> _valueList = new List<List<int>>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            _windowGraph = windowGraph.GetComponent<Window_Graph>();

            for (int i = 0; i < creatureLimits.Length; i++)
            {
                _valueList.Add(new List<int>());
            }
        }

        SetStartingPopulations();
    }

    private void FixedUpdate()
    {
        _updateGraphTime -= Time.deltaTime;
        if (_updateGraphTime <= 0f)
        {

            for(int i = 0; i < creatureLimits.Length; i++)
            {
                _valueList[i].Add(creatureLimits[i].currentPopulation);
            }

            _windowGraph.ShowGraph(_valueList, popColors);
            _updateGraphTime = 120f;
        }
    }

    [System.Serializable]
    public struct PlantSpawnZone
    {
        public string name;
        [Tooltip("Distance above or below water line")]
        public float maxHeight;
        [Tooltip("Distance above or below water line")]
        public float minHeight;
        public GameObject[] plantsToSpawn;
    }

    [System.Serializable]
    public struct CreatureLimit
    {
        public int creatureIDToLimit;
        public int populationLimit;
        public int currentPopulation;
    }

    private void SetStartingPopulations()
    {
        creatureLimits[0].currentPopulation = 30;
        creatureLimits[1].currentPopulation = 30;
        creatureLimits[2].currentPopulation = 30;
        creatureLimits[3].currentPopulation = 30;
    }
}
