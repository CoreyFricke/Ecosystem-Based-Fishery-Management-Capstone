using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private RectTransform graphContainer;

    internal void ShowGraph(List<List<int>> valueList_List, Color[] colors)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMax = 100f; // top of graph
        float xSize = 30f;

        for(int i = 1;  i < graphContainer.childCount; i++)
        {
            graphContainer.GetChild(i).gameObject.SetActive(false);
        }

        for(int i = 0;  i < valueList_List.Count; i++)
        {
            GameObject lastCircleGameObject = null;
            Color curColor = colors[i];
            List<int> valueList = valueList_List[i];
            for (int x = 0; x < valueList.Count; x++)
            {
                float xPos = xSize + x * xSize;
                float yPos = (valueList[x] / yMax) * graphHeight;
                GameObject circleGameObject = CreateCircle(new Vector2(xPos, yPos), curColor);

                if (lastCircleGameObject != null)
                {
                    CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, curColor);
                }
                lastCircleGameObject = circleGameObject;
            }
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, Color color)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        return gameObject;
    }

    private void CreateDotConnection(Vector2 dotPosA, Vector2 dotPosB, Color color)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        Vector2 dir = (dotPosB - dotPosA).normalized;
        float dirAngle = Mathf.Atan(dir.y/dir.x) * 180f/Mathf.PI;

        float distance = Vector2.Distance(dotPosA, dotPosB);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPosA + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, dirAngle);
    }
}
