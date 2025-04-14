using UnityEngine;

public class PlantGrownState : PlantBaseState
{
    float reproduceTimer;

    GameObject seed;
    bool generatedSeed = false;

    Transform childPlant;

    public override void EnterState(PlantStateManager plant)
    {
        childPlant = plant.gameObject.transform.GetChild(0);
        childPlant.transform.localScale = new Vector3(1f, 1f, 1f);

        plant.isEaten = false;

        plant.curveHolder.transform.Rotate(new Vector3(0, Random.Range(-180, 180), 0));
        reproduceTimer = 1f;
        seed = plant.seed;
    }

    public override void UpdateState(PlantStateManager plant)
    {
        if(plant.isReproducing)
        {
            if (reproduceTimer > 0)
                reproduceTimer -= Time.deltaTime;

            if (!generatedSeed && reproduceTimer < 0)
            {
                Reproduce(plant);
                generatedSeed = true;
            }

            if (seed != null && seed.GetComponent<SplineWalker>().progress >= 1f)
            {
                plant.DestroyGameObject(seed);
                seed = plant.seed;
                RecalculateTrajectory(plant);
            }

            if (seed != null && seed.GetComponent<SplineWalker>().colTransform != null)
            {
                if (seed.GetComponent<SplineWalker>().colTransform.tag == "PlantLand")
                {
                    CreateNewPlant(plant);
                }
                else if (seed.GetComponent<SplineWalker>().colTransform.tag != "PlantLand")
                {
                    plant.DestroyGameObject(seed);
                    seed = plant.seed;
                    RecalculateTrajectory(plant);
                }
            }
        }
    }

    void RecalculateTrajectory(PlantStateManager plant)
    {
        plant.curveHolder.transform.Rotate(new Vector3(0, Random.Range(-180, 180), 0));
        generatedSeed = false;
        reproduceTimer = 0.5f;
    }

    void Reproduce(PlantStateManager plant)
    {
        seed = plant.InstantiateItem(seed, plant.transform.position, Quaternion.identity);
        if(seed.GetComponent<SplineWalker>() != null)
        {
            SplineWalker splineWalker = seed.GetComponent<SplineWalker>();
            splineWalker.spline = plant.GetComponentInChildren<BezierSpline>();
            splineWalker.SetDuration(1f);
        }
    }

    void CreateNewPlant(PlantStateManager plant)
    {
        plant.InstantiateItem(plant.plantToSpread, seed.transform.position, Quaternion.identity);
        plant.DestroyGameObject(seed);
        seed = null;
    }
}
