using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image image;

    public void Init(CardDataSO cardData)
    {
        image.sprite = cardData.cardSprite;
    }
}
