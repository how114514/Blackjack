using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    [SerializeField] private float spacing;

    public List<CardView> cardViews = new();

    public void LayoutAll()
    {
        int total = cardViews.Count;

        if (total == 0)
            return;

        float startX = -((total - 1) * spacing) * 0.5f;

        for (int i = 0; i < total; i++)
        {
            RectTransform rect = cardViews[i].transform as RectTransform;

            Vector2 targetPos = new Vector2(startX + i * spacing, 0);

            cardViews[i].StartCoroutine(cardViews[i].MoveAnimation(targetPos));
        }
    }

    public void ClearHand()
    {
        cardViews.Clear();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
