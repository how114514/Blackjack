using UnityEngine;
using UnityEngine.UI;

public class ChipView : MonoBehaviour
{
    [SerializeField] private Image image;

    public void Init(ChipDataSO chipData)
    {
        image.sprite = chipData.chipSprite;
    }
}
