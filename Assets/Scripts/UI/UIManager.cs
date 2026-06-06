using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject hitButton;
    [SerializeField] private GameObject doubleDownButton;
    [SerializeField] private GameObject standButton;
    [SerializeField] private GameObject insuranceButton;
    [SerializeField] private GameObject surrenderButton;

    [Header("Area")]
    [SerializeField] private GameObject betArea;
    [SerializeField] private GameObject insuranceArea;
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        betArea.SetActive(false);
        insuranceArea.SetActive(false);

        DisablePlayerActionButtons();
    }

    //关闭玩家操作按钮
    public void DisablePlayerActionButtons()
    {
        hitButton.SetActive(false);
        doubleDownButton.SetActive(false);
        standButton.SetActive(false);
        surrenderButton.SetActive(false);
        insuranceButton.SetActive(false);
    }

    //开启玩家操作按钮
    public void EnablePlayerActionButtons(bool canInsurance)
    {
        hitButton.SetActive(true);
        doubleDownButton.SetActive(true);
        standButton.SetActive(true);
        surrenderButton.SetActive(true);

        if (canInsurance)
            insuranceButton.SetActive(true);
    }

    public void OpenBettingArea()
    {
        betArea.SetActive(true);
        startButton.SetActive(false);
    }

    public void CloseBettingArea()
    {
        betArea.SetActive(false);
    }

    public void EnableStartButton()
    {
        startButton.SetActive(true);
    }

    internal void OpenInsuranceArea()
    {
        insuranceArea.SetActive(true);
    }

    internal void CloseInsuranceArea()
    {
        insuranceArea.SetActive(false);
    }

    internal void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void OnPlayerHit()
    {
        doubleDownButton.SetActive(false);
        insuranceButton.SetActive(false);
        surrenderButton.SetActive(false);
    }
}
