using System.Collections;
using UnityEngine;

public class Age
{
    internal float curAge { get; private set; }
    internal float maxAge { get; set; }
    internal void AddToCurAge(float value)
    {
        curAge += value;

        if (curAge > maxAge)
        {
            curAge = maxAge;
        }
    }

    //Can't use Thread.Sleep(), Unity is primarily single-threaded: Sleeps entire application, use for pause?
    internal IEnumerator TickAgeDown()
    {
        while (curAge > 0)
        {
            curAge -= 0.5f;
            yield return new WaitForSeconds(1);
        }
    }
}
