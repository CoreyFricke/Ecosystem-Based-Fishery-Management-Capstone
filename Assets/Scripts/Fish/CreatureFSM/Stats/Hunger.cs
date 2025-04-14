using System.Collections;
using UnityEngine;

public class Hunger
{
    internal float curHunger { get; private set; }
    internal float maxHunger { get; set; }
    internal int[] prey {  get; set; }
    internal Vector3 lastPreyLocation { get; set; }

    internal GameObject curPrey { get; set; }

    internal void AddToCurhunger(float value)
    {
        curHunger += value;

        if (curHunger > maxHunger)
        {
            curHunger = maxHunger;
        }
    }

    internal void SubtractCurHunger(float value)
    {
        curHunger -= value;

        if (curHunger < 0)
        {
            curHunger = 0;
        }
    }

    //Can't use Thread.Sleep(), Unity is primarily single-threaded: Sleeps entire application, use for pause?
    internal IEnumerator TickHungerDown()
    {
        while( curHunger > 0 )
        {
            curHunger -= 0.5f;
            yield return new WaitForSeconds(1);
        }
    }

}
