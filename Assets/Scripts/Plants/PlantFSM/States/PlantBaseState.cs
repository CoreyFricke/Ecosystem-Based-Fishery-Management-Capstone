using UnityEngine;

public abstract class PlantBaseState
{
    public abstract void EnterState(PlantStateManager plant);

    public abstract void UpdateState(PlantStateManager plant);
}
