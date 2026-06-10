using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipViewManager : MonoBehaviour
{
    [Header("Chip View")]
    [SerializeField] private ChipView chipPrefab;
    [SerializeField] private List<ChipView> chipViews = new();

    [Header("Chip Areas")]
    [SerializeField] private Transform chip1Area;
    [SerializeField] private Transform chip5Area;
    [SerializeField] private Transform chip10Area;
    [SerializeField] private Transform chip25Area;
    [SerializeField] private Transform chip100Area;
    [SerializeField] private Transform chip500Area;
    [SerializeField] private Transform chip1000Area;
    [SerializeField] private Transform chip5000Area;
    [SerializeField] private List<Transform> chipAreas = new();
    private Dictionary<int, Transform> chipAreaMap;

    [Header("Chip Targets")]
    [SerializeField] private Transform playerIncomeTarget;
    [SerializeField] private Transform playerPayoutTarget;

    

    private void Awake()
    {
        chipAreaMap = new Dictionary<int, Transform>()
        {
            { 1, chip1Area }, { 5, chip5Area },
            { 10, chip10Area }, { 25, chip25Area },
            { 100, chip100Area }, { 500, chip500Area },
            { 1000, chip1000Area }, { 5000, chip5000Area }
        };
    }

    //根据筹码数据列表刷新筹码视图，先销毁现有的筹码视图，然后根据数据创建新的筹码视图并堆叠在对应区域内
    public IEnumerator RefreshChipView(List<ChipDataSO> list)
    {
        DestroyChip();

        yield return null;

        foreach (ChipDataSO chip in list)
        {
            Transform targetArea = chipAreaMap[chip.value];

            ChipView chipView = Instantiate(chipPrefab, targetArea);
            chipView.Init(chip);
            chipViews.Add(chipView);
        }

        SFXManager.Instance.PlayChip();

        yield return StartCoroutine(StackChips());
    }

    //将筹码移动到屏幕上方位置，模拟玩家支付筹码的动画效果
    public IEnumerator PayoutAnimation()
    {
        foreach (ChipView chip in chipViews)
        {
            yield return StartCoroutine(chip.MoveTo(playerPayoutTarget));
        }

        DestroyChip();
    }

    //销毁所有筹码视图，清空列表并删除对应的游戏对象
    private void DestroyChip()
    {
        chipViews.Clear();   
        
        foreach (Transform area in chipAreas)
        {
            foreach (Transform child in area)
            {
                Destroy(child.gameObject);
            }
        }
    }

    //将筹码在对应区域内堆叠，每个筹码之间有一定的间距
    private IEnumerator StackChips()
    {
        foreach (Transform area in chipAreas)
        {
            int index = 0;

            foreach (Transform child in area)
            {
                RectTransform rect = child as RectTransform;

                rect.localPosition = new Vector2(0, index * 7f);

                index++;
            }
        }

        yield return null;
    }

    //将筹码从屏幕上方位置移动到屏幕下方位置，模拟玩家赢得筹码的动画效果
    public IEnumerator SpawnIncomeChips(List<ChipDataSO> chips)
    {
        List<ChipView> spawned = new();

        foreach (var chip in chips)
        {
            ChipView chipView = Instantiate(chipPrefab, playerPayoutTarget);
            chipView.Init(chip);

            spawned.Add(chipView);

            RectTransform rect = chipView.transform as RectTransform;
            rect.position = playerPayoutTarget.position;

            StartCoroutine(chipView.MoveTo(playerIncomeTarget));

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (var chip in spawned)
        {     
            Destroy(chip.gameObject);
        }

        spawned.Clear();
    }
}
