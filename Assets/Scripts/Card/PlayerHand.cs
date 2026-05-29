using UnityEngine;

public class PlayerHand : Hand
{
    [SerializeField] private GameFlowController gameManager;

    public override void AddCard(CardDataSO card)
    {
        base.AddCard(card);

        if(handType == HandType.Bust)
        {
            gameManager.DisablePlayerActionButtons();
        }
    }
}