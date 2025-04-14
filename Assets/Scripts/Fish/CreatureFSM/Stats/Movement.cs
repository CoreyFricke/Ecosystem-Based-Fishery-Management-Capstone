using UnityEngine;
public class Movement
{
    internal float curSpeed { get; private set; }
    internal float rotationalSpeed { get; set; }
    internal float maxSpeed { get; set; }
    internal float acceleration { get; set; }

    private float randomTimer = 3.5f;
    private float rotationTimer = 2.5f;

    internal void SetCurSpeed(float value)
    {
        curSpeed = value;
    }

    //TODO: Change to coroutine to elminate UnityEngine dependancy? (Time.deltaTime)
    internal void Accelerate(CreatureStateManager creature)
    {
        curSpeed += acceleration * Time.deltaTime;
        creature.transform.position += creature.transform.forward * (curSpeed * Time.deltaTime);

        if (curSpeed > maxSpeed)
        {
            curSpeed = maxSpeed;
        }

        SetZRotation(creature);
    }

    //Nav Data dependant, applies rotations based on active ray
    internal void CheckRays(CreatureStateManager creature, int ray, RaycastHit hit)
    {
        Vector3 creatureVec3 = creature.transform.position;
        Vector3 hitPoint = hit.point;

        switch (ray)
        {
            //Back
            case 0:
                break;
            //Down
            case 1:
                SetCurSpeed(Vector3.Distance(creatureVec3, hitPoint));
                creature.transform.Rotate(1 * rotationalSpeed * Time.deltaTime, 0, 0);
                break;
            //Right
            case 2:
                SetCurSpeed(Vector3.Distance(creatureVec3, hitPoint));
                creature.transform.Rotate(0, -1 * rotationalSpeed * Time.deltaTime, 0);
                break;
            //Front
            case 3:
                SetCurSpeed(Vector3.Distance(creatureVec3, hitPoint));
                creature.transform.Rotate(0, -1 * rotationalSpeed * Time.deltaTime, 0);
                break;
            //Up
            case 4:
                SetCurSpeed(Vector3.Distance(creatureVec3, hitPoint));
                creature.transform.Rotate(-1 * rotationalSpeed * Time.deltaTime, 0, 0);
                break;
            //Left
            case 5:
                SetCurSpeed(Vector3.Distance(creatureVec3, hitPoint));
                creature.transform.Rotate(0, 1 * rotationalSpeed * Time.deltaTime, 0);
                break;
        }
    }

    //NavData dependant, wont rotate unless no rays are active
    internal void VariableRotation(CreatureStateManager creature)
    {
        randomTimer -= Time.deltaTime;

        if (randomTimer < 0f)
        {
            rotationTimer -= Time.deltaTime;
            creature.transform.Rotate(Random.Range(-3, 1) * (Time.deltaTime * rotationalSpeed), Random.Range(-3, 1) * (Time.deltaTime * rotationalSpeed), 0);

            if (rotationTimer < 0f)
            {
                randomTimer = Random.Range(1, 7);
                rotationTimer = Random.Range(0.5f, 3f);
            }
        }
    }

    //Keeps the fish facing upwards
    void SetZRotation(CreatureStateManager creature)
    {
        Vector3 currentRoation = creature.transform.rotation.eulerAngles;
        creature.transform.rotation = Quaternion.Slerp(creature.transform.rotation, Quaternion.Euler(new Vector3(currentRoation.x, currentRoation.y, 0)), 1 * Time.deltaTime);
    }
}
