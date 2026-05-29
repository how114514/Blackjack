using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    [SerializeField] private float spacing;

    public List<CardView> cardViews = new();

    //根据当前手牌数量重新布局卡牌，使其均匀分布在UI区域内
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

            StartCoroutine(cardViews[i].MoveAnimation(targetPos));
        }
    }

    //清空手牌UI，销毁所有卡牌视图并清空列表
    public void ClearHand()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        cardViews.Clear();
    }
}
