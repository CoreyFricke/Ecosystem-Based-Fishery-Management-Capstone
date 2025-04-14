using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(PreyIdentifier))]
public class CreatureStateManager : MonoBehaviour
{
    internal CreatureBaseState currentState { get; private set; }

    //States
    internal CreatureIdleState idleState { get; private set; } = new CreatureIdleState();
    internal CreatureNewWander newWanderState { get; private set; } = new CreatureNewWander();
    internal CreatureHuntingState huntingState { get; private set; } = new CreatureHuntingState();
    internal CreatureEatingState eatingState { get; private set; } = new CreatureEatingState();
    internal CreatureLookingForMateState lookingForMateState { get; private set; } = new CreatureLookingForMateState();
    internal CreatureMatingState matingState { get; private set; } = new CreatureMatingState();

    //Identity (for being hunted)
    [Header("Prey Identifier")]
    [SerializeField] private PreyIdentifier PreyIdentifier;
    public PreyIdentifier preyIdentifier { get { return PreyIdentifier; } private set { PreyIdentifier = value; } }

    //Navigation
    internal CreatureNav creatureNav { get; private set; }
    internal SphereCollider sphereCollider { get; private set; }

    //Stats
    [Header("Stats")]
    [SerializeField] private CreatureStatsScriptableObject CreatureStats;
    public CreatureStatsScriptableObject creatureStats { get { return CreatureStats; } private set { CreatureStats = value; } }
    internal Energy energy { get; private set; } = new Energy();
    internal Movement movement { get; private set; } = new Movement();
    internal Hunger hunger { get; private set; } = new Hunger();
    internal Reproduction reproduction { get; private set; } = new Reproduction();
    internal Age age { get; private set; } = new Age();

    //Mating
    internal bool isMating = false;


    //Debugging
    [Header("Debugging Tools / Variables")]
    [SerializeField] bool isNavActive = true;
    [SerializeField] bool doesHungerKill = true;
    [SerializeField] bool doesAgeKill = true;
    [SerializeField] string debug_CurrentState;
    [SerializeField] float debug_CurEnergy;
    [SerializeField] float debug_CurSpeed;
    [SerializeField] float debug_CurHunger;
    [SerializeField] float debug_CurRepoNeed;
    [SerializeField] float debug_CurAge;

    //NavTesting
    [SerializeField] internal GameObject navTester;

    void Start()
    {
        InitializeState(idleState);
        CreateNavData();
        SetStats();

        //Hunger
        StartCoroutine(hunger.TickHungerDown());
        InitilizeHuntingSphereCol();

        //Reproduciton
        StartCoroutine(reproduction.TickRepoNeedDown());

        //Age
        StartCoroutine(age.TickAgeDown());
    }

    void Update()
    {
        currentState.UpdateState(this);
        RunDebuggingCode();
        HandleEnergy();

        if (doesHungerKill && hunger.curHunger <= 0)
        {
            for (int i = 0; i < GameManager.Instance.creatureLimits.Length; i++)
            {
                if (GameManager.Instance.creatureLimits[i].creatureIDToLimit == gameObject.GetComponent<PreyIdentifier>()._id)
                {
                    GameManager.Instance.creatureLimits[i].currentPopulation--;
                    if (GameManager.Instance.creatureLimits[i].currentPopulation < 0)
                    {
                        GameManager.Instance.creatureLimits[i].currentPopulation = 0;
                    }
                    break;
                }
            }
            Destroy(gameObject);
        }

        if (doesAgeKill && age.curAge <= 0)
        {
            for (int i = 0; i < GameManager.Instance.creatureLimits.Length; i++)
            {
                if (GameManager.Instance.creatureLimits[i].creatureIDToLimit == gameObject.GetComponent<PreyIdentifier>()._id)
                {
                    GameManager.Instance.creatureLimits[i].currentPopulation--;
                    if(GameManager.Instance.creatureLimits[i].currentPopulation < 0)
                    {
                        GameManager.Instance.creatureLimits[i].currentPopulation = 0;
                    }
                    break;
                }
            }
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    internal void SwitchState(CreatureBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.SetStatReferences(this);
        state.EnterState(this);
    }

    //Print
    internal void PrintToConsole(string text)
    {
        print(text);
    }

    void OnDrawGizmosSelected()
    {
        if(currentState != null)
        {
            currentState.DrawGizmo(this);
        }
    }

    internal void RunCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    internal void OnTriggerEnter(Collider other)
    {
        if(currentState != null)
        {
            currentState.OnTriggerEnter(this, other);
        }
    }

    internal void RestartHungerTick()
    {
        StartCoroutine(hunger.TickHungerDown());
    }

    internal void RestartRepoTick()
    {
        StartCoroutine(reproduction.TickRepoNeedDown());
    }

    internal GameObject InstantiateNewCreature(GameObject fish, Vector3 pos, Quaternion rot)
    {
        return Instantiate(fish, pos, rot);
    }

    void RunDebuggingCode()
    {
        debug_CurEnergy = energy.curEnergy;
        debug_CurSpeed = movement.curSpeed;
        debug_CurHunger = hunger.curHunger;
        debug_CurrentState = currentState.ToString();
        debug_CurRepoNeed = reproduction.curRepoNeed;
        debug_CurAge = age.curAge;
    }

    void SetStats()
    {
        //Energy
        energy.maxEnergy = creatureStats.maxEnergy;
        energy.AddToCurEnergy(energy.maxEnergy);

        //Movement
        movement.acceleration = creatureStats.acceleration;
        movement.maxSpeed = creatureStats.maxSpeed;
        movement.rotationalSpeed = creatureStats.rotationalSpeed;

        //Hunger
        hunger.maxHunger = creatureStats.maxHunger;
        hunger.AddToCurhunger(creatureStats.maxHunger);
        hunger.prey = creatureStats.prey;

        //Prey
        preyIdentifier._id = creatureStats.id;
        preyIdentifier._category = creatureStats.category;

        //Reproduction
        reproduction.maxRepoNeed = creatureStats.maxRepoNeed;
        reproduction.AddToCurRepoNeed(creatureStats.maxRepoNeed);

        //Age
        age.maxAge = creatureStats.maxAge;
        age.AddToCurAge(age.maxAge);
    }

    void CreateNavData()
    {
        if (isNavActive)
        {
            creatureNav = new CreatureNav();
            creatureNav.meshCollider = GetComponentInChildren<MeshCollider>();
            creatureNav.navTransform = transform;
        }
        else
        {
            creatureNav = null;
        }
    }

    void InitializeState(CreatureBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
        currentState.SetStatReferences(this);
    }

    void InitilizeHuntingSphereCol()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = creatureStats.huntingDistance;
        sphereCollider.enabled = false;
        sphereCollider.isTrigger = true;
    }

    void HandleEnergy()
    {
        if(currentState != idleState)
        {
            energy.SubtractCurEnergy(movement.curSpeed * Time.deltaTime);
            if(currentState != idleState && energy.curEnergy <= 0)
            {
                currentState = idleState;
            }
        }
    }
}
