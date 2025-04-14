public class Energy
{
    internal float curEnergy { get; private set; }
    internal float maxEnergy { get; set; }

    internal void AddToCurEnergy(float value)
    {
        curEnergy += value;

        if(curEnergy > maxEnergy)
        {
            curEnergy = maxEnergy;
        }
    }

    internal void SubtractCurEnergy(float value)
    {
        curEnergy -= value;

        if (curEnergy < 0)
        {
            curEnergy = 0;
        }
    }
}
