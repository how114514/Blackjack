using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipSystem : MonoBehaviour
{
    public int playerChip;
    public int currentBet;
    public int currentInsuranceBet;

    [SerializeField] private PlayerHand playerHand;
    public ChipViewManager chipViewManager;
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

    public IEnumerator WinBet()
    {
        int amount = CalculatePayout();
        yield return StartCoroutine(ResolveBet(amount));
    }

    private int CalculatePayout()
    {
        if (playerHand.handType == HandType.Blackjack)
            return Mathf.RoundToInt(currentBet * 2.5f);
        else
            return currentBet * 2;
    }

    public IEnumerator LoseBet()
    {
        currentBet = 0;
        currentInsuranceBet = 0;

        RefreshChipUI();

        yield return null;
    }

    public IEnumerator Push()
    {
        int amount = currentBet;
        yield return StartCoroutine(ResolveBet(amount));
    }

    public IEnumerator Surrender()
    {
        int amount = Mathf.RoundToInt(currentBet * 0.5f);
        yield return StartCoroutine(ResolveBet(amount));
    }

    public IEnumerator PayInsurance()
    {
        int amount = currentInsuranceBet * 3;

        currentInsuranceBet = 0;

        yield return StartCoroutine(ResolveBet(amount));
    }

    private IEnumerator ResolveBet(int amount)
    {
        List<ChipDataSO> chips = BuildChipList(amount);

        yield return StartCoroutine(chipViewManager.SpawnIncomeChips(chips));

        int oldValue = playerChip;

        playerChip += amount;

        currentBet = 0;
        currentInsuranceBet = 0;

        yield return StartCoroutine(RefreshChipUIAnimated(oldValue, playerChip));
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

    private List<ChipDataSO> BuildChipList(int amount)
    {
        List<ChipDataSO> list = new List<ChipDataSO>();

        int remaining = amount;

        while (remaining >= 5000)
        {
            list.Add(chip5000);
            remaining -= 5000;
        }
        while (remaining >= 1000)
        {
            list.Add(chip1000);
            remaining -= 1000;
        }
        while (remaining >= 500)
        {
            list.Add(chip500);
            remaining -= 500;
        }
        while (remaining >= 100)
        {
            list.Add(chip100);
            remaining -= 100;
        }
        while (remaining >= 25)
        {
            list.Add(chip25);
            remaining -= 25;
        }
        while (remaining >= 10)
        {
            list.Add(chip10);
            remaining -= 10;
        }
        while (remaining >= 5)
        {
            list.Add(chip5);
            remaining -= 5;
        }
        while (remaining >= 1)
        {
            list.Add(chip1);
            remaining -= 1;
        }

        return list;
    }

    public IEnumerator RefreshChipUIAnimated(int from, int to)
    {
        UpdateChipList();
        chipViewManager.RefreshChipView(chipList);

        yield return StartCoroutine(AnimateChipText(from, to));
    }

    private void RefreshChipUI()
    {
        UpdateChipList();
        chipViewManager.RefreshChipView(chipList);
        playerChipText.text = playerChip.ToString();
    }

    public IEnumerator AnimateChipText(int from, int to)
    {
        float duration = 1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float lerp = t / duration;

            int value = Mathf.RoundToInt(Mathf.Lerp(from, to, lerp));

            playerChipText.text = value.ToString();

            yield return null;
        }

        playerChipText.text = to.ToString();
    }
}
