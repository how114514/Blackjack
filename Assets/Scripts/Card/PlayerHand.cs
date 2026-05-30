using UnityEngine;

public class PlayerHand : Hand
{
    [SerializeField] private ScreenFlash screenFlash;
    [SerializeField] private SFXManager sFX;

    public override void EvaluateHand()
    {
        int value = CalculateHandValue();

        if (cards.Count == 2 && value == 21)
        { 
            handType = HandType.Blackjack;
            screenFlash.BlackjackFlash();
            sFX.PlayBlackjack();
        }
        else if (value > 21)
        {
            handType = HandType.Bust;
            screenFlash.BustFlash();
            sFX.PlayLose();
        }
        else
            handType = HandType.Normal;
    }
}