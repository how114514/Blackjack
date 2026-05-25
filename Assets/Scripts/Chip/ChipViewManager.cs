using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipViewManager : MonoBehaviour
{
    [SerializeField] private ChipView chipPrefab;

    [SerializeField] private Transform chip1Area;
    [SerializeField] private Transform chip5Area;
    [SerializeField] private Transform chip10Area;
    [SerializeField] private Transform chip25Area;
    [SerializeField] private Transform chip100Area;
    [SerializeField] private Transform chip500Area;
    [SerializeField] private Transform chip1000Area;
    [SerializeField] private Transform chip5000Area;

    [SerializeField] private List<Transform> chipAreas = new();

    public void RefreshChipView(List<ChipDataSO> list)
    {
        DestoryChip();

        foreach (ChipDataSO chip in list)
        {
            Transform targetArea = null;

            switch (chip.value)
            {
                case 1:
                    targetArea = chip1Area;
                    break;
                case 5:
                    targetArea = chip5Area;
                    break;
                case 10:
                    targetArea = chip10Area;
                    break;
                case 25:
                    targetArea = chip25Area;
                    break;
                case 100:
                    targetArea = chip100Area;
                    break;
                case 500:
                    targetArea = chip500Area;
                    break;
                case 1000:
                    targetArea = chip1000Area;
                    break;
                case 5000:
                    targetArea = chip5000Area;
                    break;
            }

            ChipView chipView = Instantiate(chipPrefab, targetArea);
            chipView.Init(chip);
            StartCoroutine(StackChips());
        }
    }

    private void DestoryChip()
    {
        foreach (Transform area in chipAreas)
        {
            foreach (Transform child in area)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private IEnumerator StackChips()
    {
        yield return null;

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
    }
}
