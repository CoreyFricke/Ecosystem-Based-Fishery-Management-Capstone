using UnityEngine;

public class PlantGrowingState : PlantBaseState
{
    //float growRate = 0.1f;
    //float maxSize = 1f;

    private float growTimer = 10f;

    Transform childPlant;
    public override void EnterState(PlantStateManager plant)
    {
        childPlant = plant.gameObject.transform.GetChild(0);
        childPlant.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        plant.isEaten = true;
        plant.isClaimed = false;
    }

    public override void UpdateState(PlantStateManager plant)
    {
        //childPlant.transform.localScale += new Vector3(growRate, growRate, growRate) * Time.deltaTime;

        //if(childPlant.transform.localScale.x > maxSize)
        //{
        //    plant.SwitchState(plant.plantGrownState);
        //}

        growTimer -= Time.deltaTime;
        if (growTimer <= 0)
        {
            plant.SwitchState(plant.plantGrownState);
        }
    }
}
