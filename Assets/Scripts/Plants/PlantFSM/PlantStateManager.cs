using UnityEngine;

[RequireComponent(typeof(PreyIdentifier))]
public class PlantStateManager : MonoBehaviour
{
    PlantBaseState currentState;

    internal PlantGrowingState plantGrowingState = new PlantGrowingState();
    internal PlantGrownState plantGrownState = new PlantGrownState();

    [SerializeField]
    LayerMask ignoreLayers;

    [SerializeField]
    internal GameObject plantToSpread;

    [SerializeField]
    internal GameObject seed;

    [SerializeField]
    internal GameObject curveHolder;

    //Identity (for being hunted)
    [Header("Prey Identifier")]
    [SerializeField] private PreyIdentifier PreyIdentifier;
    public PreyIdentifier preyIdentifier { get { return PreyIdentifier; } private set { PreyIdentifier = value; } }

    [Header("Stats")]
    [SerializeField] private PlantStatsScriptableObject PlantStats;
    public PlantStatsScriptableObject plantStats { get { return PlantStats; } private set { PlantStats = value; } }

    internal bool isClaimed { get; set; } = false;
    internal bool isEaten { get; set; } = true;

    //Debugging
    [Header("Debugging Tools / Variables")]
    [SerializeField] internal bool isReproducing;
    [SerializeField] string debug_CurrentState;

    void Start()
    {
        preyIdentifier._category = plantStats.category;
        preyIdentifier._id = plantStats.id;

        currentState = plantGrownState;
        currentState.EnterState(this);
    }

    void Update()
    {
        debug_CurrentState = currentState.ToString();
        currentState.UpdateState(this);
    }

    internal void SwitchState(PlantBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    internal GameObject InstantiateItem(GameObject newObject, Vector3 pos, Quaternion rot)
    {
        return Instantiate(newObject, pos, rot);
    }

    internal void DestroyGameObject(GameObject newObject)
    {
        Destroy(newObject);
    }

    internal void PrintToConsole(string text)
    {
        print(text);
    }

    internal void RotateToNormal()
    {
        if (Physics.Raycast(transform.position + new Vector3(0,1,0), Vector3.down, out RaycastHit hit, 5f, ~ignoreLayers))
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }
}
