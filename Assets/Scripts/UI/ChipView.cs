using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChipView : MonoBehaviour
{
    [SerializeField] private Image image;

    //初始化筹码视图
    public void Init(ChipDataSO chipData)
    {
        image.sprite = chipData.chipSprite;
    }

    //将筹码移动到目标位置
    public IEnumerator MoveTo(Transform target)
    {
        RectTransform rect = transform as RectTransform;
        Vector2 startPos = rect.position;
        Vector2 targetPos = target.position;

        float time = 0f;
        float duration = 0.1f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            t = Mathf.SmoothStep(0, 1, t);

            rect.position = Vector2.Lerp(startPos, targetPos, t);

            yield return null;
        }

        rect.position = targetPos;
    }
}
