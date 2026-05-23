using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CardDeck deck;
    [SerializeField] private PlayerHandUI playerHand;
    [SerializeField] private DealerHandUI dealerHand;

    [ContextMenu("Start New Round")]
    public void StartNewRound()
    {
        playerHand.ClearHand();
        dealerHand.ClearHand();

        deck.InitDeck();
        deck.Shuffle();
    }
}
