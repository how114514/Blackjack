public class PlayerHand : Hand
{
    public bool isBust = false;

    public override void AddCard(CardDataSO card)
    {
        base.AddCard(card);

        if (CalculateHandValue() > 21)
            isBust = true;
    }

    public void NewTurn()
    {
        isBust = false;
    }
}