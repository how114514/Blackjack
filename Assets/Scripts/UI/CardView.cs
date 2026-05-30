using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image image;

    //卡牌翻转动画
    public IEnumerator FlipAnimation(CardDataSO cardData)
    {
        float duration = 0.2f;

        Vector3 scale = image.rectTransform.localScale;

        while (image.rectTransform.localScale.x > 0)
        {
            image.rectTransform.localScale -= new Vector3(Time.deltaTime / duration, 0, 0);
            yield return null;
        }

        image.sprite = cardData.cardSprite;

        while (image.rectTransform.localScale.x < scale.x)
        {
            image.rectTransform.localScale += new Vector3(Time.deltaTime / duration, 0, 0);
            yield return null;
        }

        image.rectTransform.localScale = scale;
    }

    //卡牌移动动画
    public IEnumerator MoveAnimation(Vector2 targetPos)
    {
        RectTransform rect = transform as RectTransform;
        Vector2 startPos = rect.anchoredPosition;

        float time = 0f;
        float duration = 0.2f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            t = Mathf.SmoothStep(0, 1, t);

            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            yield return null;
        }

        rect.anchoredPosition = targetPos;
    }

    //卡牌旋转动画
    public IEnumerator RotateAnimation()
    {
        RectTransform rect =
            transform as RectTransform;

        Quaternion startRot = Quaternion.Euler(0, 0, 180f);

        Quaternion endRot = Quaternion.identity;

        rect.localRotation = startRot;

        float time = 0f;
        float duration = 0.2f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            rect.localRotation = Quaternion.Lerp(startRot, endRot, t);

            yield return null;
        }

        rect.localRotation = endRot;
    }
}
