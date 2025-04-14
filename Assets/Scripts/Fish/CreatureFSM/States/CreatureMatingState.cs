
using UnityEngine;

public class CreatureMatingState : CreatureBaseState
{
    private Movement movement;
    private Reproduction reproduction;
    private float timeToMate = 2f;

    public override void DrawGizmo(CreatureStateManager creature)
    {
    }

    public override void EnterState(CreatureStateManager creature)
    {
        movement.SetCurSpeed(0);
        creature.isMating = true;
    }

    public override void ExitState(CreatureStateManager creature)
    {
    }

    public override void FixedUpdateState(CreatureStateManager creature)
    {
        timeToMate -= Time.deltaTime;

        if (timeToMate <= 0)
        {
            timeToMate = 0;
            ProduceOspring(creature);
            reproduction.AddToCurRepoNeed(reproduction.maxRepoNeed);
            creature.RestartRepoTick();
            creature.SwitchState(creature.newWanderState);
        }
    }

    public override void OnTriggerEnter(CreatureStateManager creature, Collider other)
    {
    }

    public override void SetStatReferences(CreatureStateManager creature)
    {
        movement = creature.movement;
        reproduction = creature.reproduction;
    }

    public override void UpdateState(CreatureStateManager creature)
    {
    }

    void ProduceOspring(CreatureStateManager creature)
    {
        for(int i = 0; i < GameManager.Instance.creatureLimits.Length; i++)
        {
            if (GameManager.Instance.creatureLimits[i].creatureIDToLimit == creature.gameObject.GetComponent<PreyIdentifier>()._id)
            {
                for (int x = 0; x < Random.Range(1, 3); x++)
                {
                    if (GameManager.Instance.creatureLimits[i].currentPopulation < GameManager.Instance.creatureLimits[i].populationLimit)
                    {
                        GameObject newFish = creature.InstantiateNewCreature(creature.gameObject, creature.transform.position, Quaternion.identity);
                        GameManager.Instance.creatureLimits[i].currentPopulation++;

                        if(creature.gameObject.GetComponent<PreyIdentifier>()._id == 0)
                        {
                            newFish.transform.parent = GameObject.Find("ShallowFishHolder").transform;
                            newFish.name = "ShallowFish";
                        }
                        else if(creature.gameObject.GetComponent<PreyIdentifier>()._id == 2)
                        {
                            newFish.transform.parent = GameObject.Find("DeepFishHolder").transform;
                            newFish.name = "DeepFish";
                        }
                        else if (creature.gameObject.GetComponent<PreyIdentifier>()._id == 6)
                        {
                            newFish.transform.parent = GameObject.Find("VeryDeepHolder").transform;
                            newFish.name = "VeryDeepFish";
                        }
                        else if (creature.gameObject.GetComponent<PreyIdentifier>()._id == 7)
                        {
                            newFish.transform.parent = GameObject.Find("FourthHolder").transform;
                            newFish.name = "FourthFish";
                        }
                    }
                }
                break;
            }
        }
    }
}
