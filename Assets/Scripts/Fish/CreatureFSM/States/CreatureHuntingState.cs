using UnityEngine;

public class CreatureHuntingState : CreatureBaseState
{
    SphereCollider sphereCollider;
    Movement movement;
    Hunger hunger;

    private float eatingDistance = 1f;
    private float rotationalSpeed = 4.0f;

    private bool preyFound = false;
    private Vector3 preyLocation;

    public override void EnterState(CreatureStateManager creature)
    {
        sphereCollider.enabled = true;
    }

    public override void FixedUpdateState(CreatureStateManager creature)
    {
        movement.Accelerate(creature);

        if(preyFound && Vector3.Distance(creature.transform.position, preyLocation) <= eatingDistance)
        {
            preyFound = false;
            creature.SwitchState(creature.eatingState);
        }

        //Nav Data Dependant
        if (creature.creatureNav != null)
        {
            if (preyFound && creature.creatureNav.noRaysActive)
            {
                Vector3 newDirection = Vector3.RotateTowards(creature.transform.forward, preyLocation - creature.transform.position, rotationalSpeed * Time.deltaTime, 0.0f);
                creature.transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else if (!preyFound && creature.creatureNav.noRaysActive)
            {
                movement.VariableRotation(creature);
            }

            creature.creatureNav.GenerateNaviagtionRays();
            if (creature.creatureNav.noRaysActive == false)
                movement.CheckRays(creature, creature.creatureNav.currentRay, creature.creatureNav.raycastHit);
        }
    }

    public override void SetStatReferences(CreatureStateManager creature)
    {
        sphereCollider = creature.sphereCollider;
        movement = creature.movement;
        hunger = creature.hunger;
    }

    public override void OnTriggerEnter(CreatureStateManager creature, Collider other)
    {
        ExecuteTrigger(creature, other);
    }

    private void ExecuteTrigger(CreatureStateManager creature, Collider other)
    {
        if (!preyFound && other.transform.gameObject.GetComponent<PreyIdentifier>() != null)
        {
            PreyIdentifier prey = other.transform.gameObject.GetComponent<PreyIdentifier>();

            for (int i = 0; i < hunger.prey.Length; i++)
            {
                if (prey._id == hunger.prey[i])
                {
                    preyFound = true;

                    switch (prey._category)
                    {
                        case "Plant":
                            HuntPlant(other);
                            break;

                        case "Fish":
                            HuntFish(creature, other);
                            break;
                    }
                }
            }
        }
    }

    void HuntPlant(Collider other)
    {
        PlantStateManager plantManager = other.transform.gameObject.GetComponent<PlantStateManager>();

        if (plantManager.isEaten == false && plantManager.isClaimed == false)
        {
            plantManager.isClaimed = true;

            hunger.curPrey = other.gameObject;
            sphereCollider.enabled = false;
            preyLocation = other.transform.position;
        }
        else
        {
            preyFound = false;
        }
    }

    void HuntFish(CreatureStateManager creature, Collider other) 
    {
        preyLocation = other.transform.position;
        hunger.curPrey = other.gameObject;
        sphereCollider.enabled = false;

        //while (Vector3.Distance(creature.transform.position, preyLocation) > 1f)
        //{
        //    preyLocation = other.transform.position;
        //}
    }

    //--------------------------------------------------Pile - Does nothing in this state ------------------------------------------------------------

    public override void UpdateState(CreatureStateManager creature)
    {
    }
    public override void DrawGizmo(CreatureStateManager creature)
    {
        if (creature.creatureNav != null)
            creature.creatureNav.RaycastGizmos();
    }
    public override void ExitState(CreatureStateManager creature)
    {
        sphereCollider.enabled = false;
    }
}
