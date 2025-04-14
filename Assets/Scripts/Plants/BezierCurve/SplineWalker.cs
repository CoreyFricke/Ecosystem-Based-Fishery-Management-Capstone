using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    public BezierSpline spline;
    private float duration;
    public float progress;

    public Collider colTransform;

    private void Update()
    {
        progress += Time.deltaTime / duration;
        if (progress > 1f)
        {
            progress = 1f;
        }
        transform.localPosition = spline.GetPoint(progress);
    }

    private void OnTriggerEnter(Collider other)
    {
        colTransform = other;
    }

    internal float GetDuration()
    {
        return duration;
    }

    internal void SetDuration(float num)
    {
        duration = num;
    }
}
