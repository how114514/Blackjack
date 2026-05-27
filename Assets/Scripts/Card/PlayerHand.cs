using UnityEngine;

public class PlayerHand : Hand
{
    [SerializeField] private GameManager gameManager;

    public override void AddCard(CardDataSO card)
    {
        base.AddCard(card);

        if(handType == HandType.Bust)
        {
            gameManager.DisablePlayerActionButtons();
        }
    }
}