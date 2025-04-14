using System.Collections;
using UnityEngine;

public class Reproduction
{
    internal float curRepoNeed { get; private set; }
    internal float maxRepoNeed { get; set; }
    internal GameObject curMate { get; set; }

    internal void AddToCurRepoNeed(float value)
    {
        curRepoNeed += value;

        if (curRepoNeed > maxRepoNeed)
        {
            curRepoNeed = maxRepoNeed;
        }
    }

    internal void SubtractFromRepoNeed(float value)
    {
        curRepoNeed -= value;

        if (curRepoNeed < 0)
        {
            curRepoNeed = 0;
        }
    }

    //Can't use Thread.Sleep(), Unity is primarily single-threaded: Sleeps entire application, use for pause?
    internal IEnumerator TickRepoNeedDown()
    {
        while (curRepoNeed > 0)
        {
            curRepoNeed -= 0.5f;
            yield return new WaitForSeconds(1);
        }
    }
}
