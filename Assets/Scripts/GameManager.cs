using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CardDeck deck;
    [SerializeField] private PlayerHandUI playerHandUI;
    [SerializeField] private DealerHandUI dealerHandUI;

    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private DealerHand dealerHand;

    [Header("Button")]
    public Button startButton;
    public Button hitButton;
    public Button standButton;

    public void StartNewRound()
    {
        playerHand.ClearHand();
        playerHand.NewTurn();
        dealerHand.ClearHand();

        playerHandUI.ClearHand();
        dealerHandUI.ClearHand();

        deck.InitDeck();
        deck.Shuffle();

        deck.DealToPlayer(1);
        deck.DealToDealer(1);
        deck.DealToPlayer(1);
        deck.DealToDealer(1);

        startButton.interactable = false;
        hitButton.interactable = true;
        standButton.interactable = true;
    }

    public void StartDealerTurn()
    {
        hitButton.interactable = false;
        standButton.interactable = false;

        while (dealerHand.CalculateHandValue() < 17)
            deck.DealToDealer(1);

        CheckWinner();
    }

    private void CheckWinner()
    {
        if (playerHand.CalculateHandValue() > 21)
            Debug.Log("lose");
        else
        {
            if (dealerHand.CalculateHandValue() > 21)
                Debug.Log("win");
            else
            {
                if (playerHand.CalculateHandValue() > dealerHand.CalculateHandValue())
                    Debug.Log("win");
                else if (playerHand.CalculateHandValue() < dealerHand.CalculateHandValue())
                    Debug.Log("lose");
                else
                    Debug.Log("push");
            }
        }

        startButton.interactable = true;
    }

    public void PlayerHit()
    {
        deck.DealToPlayer(1);

        if (playerHand.isBust)
            hitButton.interactable = false;
    }
}
