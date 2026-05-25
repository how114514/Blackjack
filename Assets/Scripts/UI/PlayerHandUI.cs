using System.Collections.Generic;
using UnityEngine;

public class PlayerHandUI : MonoBehaviour
{
    [SerializeField] private float spacing = 100f;

    private List<RectTransform> cards = new();

    public void AddCard(RectTransform card)
    {
        cards.Add(card);
        Layout();
    }

    private void Layout()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            float startX = -(cards.Count - 1) * spacing * 0.5f;
            cards[i].anchoredPosition = new Vector2(startX + i * spacing, 0);
        }
    }

    internal void ClearHand()
    {
        cards.Clear();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
