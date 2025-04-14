
using TMPro;
using UnityEngine;

public class CreatureLookingForMateState : CreatureBaseState
{
    SphereCollider sphereCollider;
    Movement movement;
    Reproduction reproduction;

    private float matingDistance = 1f;
    private float rotationalSpeed = 4.0f;

    private bool mateFound = false;
    private Vector3 mateLocation;

    public override void EnterState(CreatureStateManager creature)
    {
        sphereCollider.enabled = true;
        creature.isMating = false;
        mateFound = false;
    }

    public override void FixedUpdateState(CreatureStateManager creature)
    {
        movement.Accelerate(creature);

        if (mateFound && Vector3.Distance(creature.transform.position, mateLocation) <= matingDistance)
        {
            mateFound = false;
            creature.SwitchState(creature.matingState);
        }

        //Nav Data Dependant
        if (creature.creatureNav != null)
        {
            if (mateFound && creature.creatureNav.noRaysActive)
            {
                Vector3 newDirection = Vector3.RotateTowards(creature.transform.forward, mateLocation - creature.transform.position, rotationalSpeed * Time.deltaTime, 0.0f);
                creature.transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else if (!mateFound && creature.creatureNav.noRaysActive)
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
        reproduction = creature.reproduction;
    }

    public override void OnTriggerEnter(CreatureStateManager creature, Collider other)
    {
        ExecuteTrigger(creature, other);
    }

    private void ExecuteTrigger(CreatureStateManager creature, Collider other)
    {
        if(!mateFound && other.transform.gameObject.GetComponent<PreyIdentifier>() != null)
        {
            PreyIdentifier mate = other.transform.gameObject.GetComponent<PreyIdentifier>();

            if(mate._id == creature.GetComponent<PreyIdentifier>()._id)
            {
                mateFound = true;
                Mate(creature, other);
            }
        }
    }

    void Mate(CreatureStateManager creature, Collider mate)
    {
        if(mate.GetComponent<CreatureStateManager>() != null)
        {
            CreatureStateManager mateManger = mate.GetComponent<CreatureStateManager>();

            if (mateManger.isMating == false && mateManger.currentState == mateManger.lookingForMateState)
            {
                creature.isMating = true;
                mateManger.isMating = true;
                mateManger.SwitchState(mateManger.matingState);
                reproduction.curMate = mate.gameObject;
                sphereCollider.enabled = false;
                mateLocation = mate.transform.position;
            }
            else
            {
                mateFound = false;
            }
        }
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
