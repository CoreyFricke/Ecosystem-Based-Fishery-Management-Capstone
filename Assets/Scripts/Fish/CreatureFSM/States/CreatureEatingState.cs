using UnityEngine;
public class CreatureEatingState : CreatureBaseState
{
    private GameObject curPrey;
    private Hunger hunger;
    private float timeToEat = 2f;
    public override void DrawGizmo(CreatureStateManager creature)
    {
    }

    public override void EnterState(CreatureStateManager creature)
    {
    }

    public override void ExitState(CreatureStateManager creature)
    {
    }

    public override void FixedUpdateState(CreatureStateManager creature)
    {
        timeToEat -= Time.deltaTime;

        if(timeToEat <= 0) {
            timeToEat = 0;
            eatPrey();
            hunger.AddToCurhunger(hunger.maxHunger);
            creature.RestartHungerTick();
            creature.SwitchState(creature.newWanderState);
        }
    }

    public override void OnTriggerEnter(CreatureStateManager creature, Collider other)
    {
    }

    public override void SetStatReferences(CreatureStateManager creature)
    {
        hunger = creature.hunger;
        curPrey = hunger.curPrey;
    }

    public override void UpdateState(CreatureStateManager creature)
    {
    }

    void eatPrey()
    {
        if(curPrey != null)
        {
            switch (curPrey.GetComponent<PreyIdentifier>()._category)
            {
                case "Plant":
                    EatPlant();
                    break;

                case "Fish":
                    EatFish();
                    break;
            }
        }
    }

    void EatPlant()
    {
        PlantStateManager plantManager = curPrey.GetComponent<PlantStateManager>();
        plantManager.isEaten = true;
        plantManager.SwitchState(plantManager.plantGrowingState);
    }

    void EatFish()
    {

    }
}
