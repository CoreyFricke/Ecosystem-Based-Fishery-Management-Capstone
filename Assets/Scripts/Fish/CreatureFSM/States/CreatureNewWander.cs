using UnityEngine;

public class CreatureNewWander : CreatureBaseState
{
    private Movement movement;
    private Hunger hunger;
    Reproduction reproduction;

    public override void EnterState(CreatureStateManager creature)
    {
        movement.SetCurSpeed(0);
    }

    public override void UpdateState(CreatureStateManager creature)
    {
        if (hunger.curHunger <= hunger.maxHunger * 0.85f)
        {
            creature.SwitchState(creature.huntingState);
        }
        else if (reproduction.curRepoNeed <= reproduction.maxRepoNeed * 0.50f)
        {
            creature.SwitchState(creature.lookingForMateState);
        }
    }

    public override void FixedUpdateState(CreatureStateManager creature)
    {
        movement.Accelerate(creature);

        //Nav Data Dependant
        if (creature.creatureNav != null)
        {
            if (creature.creatureNav.noRaysActive)
                movement.VariableRotation(creature);

            creature.creatureNav.GenerateNaviagtionRays();

            if (creature.creatureNav.noRaysActive == false)
                movement.CheckRays(creature, creature.creatureNav.currentRay, creature.creatureNav.raycastHit);
        }
    }
    public override void SetStatReferences(CreatureStateManager creature)
    {
        movement = creature.movement;
        hunger = creature.hunger;
        reproduction = creature.reproduction;
    }

    public override void DrawGizmo(CreatureStateManager creature)
    {
        if (creature.creatureNav != null)
            creature.creatureNav.RaycastGizmos();
    }

    //--------------------------------------------------Pile - Does nothing in this state ------------------------------------------------------------

    public override void ExitState(CreatureStateManager creature)
    {
    }

    public override void OnTriggerEnter(CreatureStateManager creature, Collider other)
    {
    }
}

