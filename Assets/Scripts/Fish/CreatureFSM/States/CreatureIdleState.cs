using UnityEngine;

public class CreatureIdleState : CreatureBaseState
{
    private Energy energy;

    public override void EnterState(CreatureStateManager creature)
    {
        energy = creature.energy;
    }

    public override void UpdateState(CreatureStateManager creature)
    {
        energy.AddToCurEnergy(10 * Time.deltaTime);

        if (energy.curEnergy >= energy.maxEnergy)
        {
            creature.SwitchState(creature.newWanderState);
        }
    }

    public override void SetStatReferences(CreatureStateManager creature)
    {
        energy = creature.energy;
    }

    //--------------------------------------------------Pile - Does nothing in this state ------------------------------------------------------------

    public override void ExitState(CreatureStateManager creature)
    {
    }
    public override void FixedUpdateState(CreatureStateManager creature)
    {
    }

    public override void DrawGizmo(CreatureStateManager creature)
    {
    }

    public override void OnTriggerEnter(CreatureStateManager creature, Collider other)
    {
    }
}
