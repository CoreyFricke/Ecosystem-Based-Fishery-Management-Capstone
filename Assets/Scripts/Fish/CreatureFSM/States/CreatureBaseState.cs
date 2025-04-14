
using UnityEngine;

public abstract class CreatureBaseState
{
    public abstract void EnterState(CreatureStateManager creature);

    public abstract void UpdateState(CreatureStateManager creature);

    public abstract void FixedUpdateState(CreatureStateManager creature);

    public abstract void ExitState(CreatureStateManager creature);

    public abstract void DrawGizmo(CreatureStateManager creature);

    public abstract void SetStatReferences(CreatureStateManager creature);

    public abstract void OnTriggerEnter(CreatureStateManager creature, Collider other);
}
