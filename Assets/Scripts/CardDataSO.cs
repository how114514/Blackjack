using UnityEngine;

[CreateAssetMenu(menuName = "Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public Suit cardSuit;
    public Rank cardRank;
    public int cardValue;
    public Sprite cardSprite;
}
