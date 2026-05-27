using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipSystem : MonoBehaviour
{
    public int playerChip;
    public int currentBet;
    public int currentInsuranceBet;

    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private ChipViewManager chipViewManager;
    [SerializeField] private TMP_Text playerChipText;

    [SerializeField] private ChipDataSO chip1;
    [SerializeField] private ChipDataSO chip5;
    [SerializeField] private ChipDataSO chip10;
    [SerializeField] private ChipDataSO chip25;
    [SerializeField] private ChipDataSO chip100;
    [SerializeField] private ChipDataSO chip500;
    [SerializeField] private ChipDataSO chip1000;
    [SerializeField] private ChipDataSO chip5000;

    public List<ChipDataSO> chipList = new();
    
    public void WinBet()
    {
        if (playerHand.handType == HandType.Blackjack)
        {
            playerChip += Mathf.RoundToInt(currentBet * 2.5f);
        }
        else
        {
            playerChip += currentBet * 2;
        }

        currentBet = 0;
        currentInsuranceBet = 0;

        RefreshChipUI();
    }

    public void LoseBet()
    {
        currentBet = 0;
        currentInsuranceBet = 0;

        RefreshChipUI();
    }

    public void Push()
    {
        playerChip += currentBet;
        currentBet = 0;
        currentInsuranceBet = 0;

        RefreshChipUI();
    }
    public void Surrender()
    {
        playerChip += Mathf.RoundToInt(currentBet * 0.5f);
        currentBet = 0;
        currentInsuranceBet = 0;

        RefreshChipUI();
    }

    public void PayInsurance()
    {
        playerChip += currentInsuranceBet * 3;
        currentInsuranceBet = 0;

        RefreshChipUI();
    }

    public void AdjustBet(int amount)
    {
        if (playerChip < amount)
            return;

        if (currentBet + amount < 0)
            return;

        currentBet += amount;
        playerChip -= amount;


        RefreshChipUI();
    }

    public void AdjustInsurance(int amount)
    {
        if (playerChip < amount)
            return;

        if (currentInsuranceBet + amount < 0)
            return;

        if(currentInsuranceBet + amount > currentBet / 2)
            return;

        currentInsuranceBet += amount;
        playerChip -= amount;

        playerChipText.text = playerChip.ToString();
    }

    public void BetAll()
    {
        currentBet += playerChip;
        playerChip = 0;

        RefreshChipUI();
    }

    public void CancelBet()
    {
        playerChip += currentBet;
        currentBet = 0;

        RefreshChipUI();
    }

    public void MaxInsurance()
    {
        playerChip -= currentBet / 2 - currentInsuranceBet;
        currentInsuranceBet = currentBet / 2;

        playerChipText.text = playerChip.ToString();
    }

    public void CancelInsurance()
    {
        playerChip += currentInsuranceBet;
        currentInsuranceBet = 0;

        playerChipText.text = playerChip.ToString();
    }

    public void DoubleDown()
    {
        playerChip -= currentBet;
        currentBet += currentBet;

        RefreshChipUI();
    }

    private void UpdateChipList()
    {
        chipList.Clear();
        int remaining = currentBet;

        while (remaining >= 5000)
        {
            chipList.Add(chip5000);
            remaining -= 5000;
        }
        while (remaining >= 1000)
        {
            chipList.Add(chip1000);
            remaining -= 1000;
        }
        while (remaining >= 500)
        {
            chipList.Add(chip500);
            remaining -= 500;
        }
        while (remaining >= 100)
        {
            chipList.Add(chip100);
            remaining -= 100;
        }
        while (remaining >= 25)
        {
            chipList.Add(chip25);
            remaining -= 25;
        }
        while (remaining >= 10)
        {
            chipList.Add(chip10);
            remaining -= 10;
        }
        while (remaining >= 5)
        {
            chipList.Add(chip5);
            remaining -= 5;
        }
        while (remaining >= 1)
        {
            chipList.Add(chip1);
            remaining -= 1;
        }
    }

    private void RefreshChipUI()
    {
        UpdateChipList();
        chipViewManager.RefreshChipView(chipList);
        playerChipText.text = playerChip.ToString();
    }
}
