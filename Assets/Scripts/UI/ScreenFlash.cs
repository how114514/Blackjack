using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public static ScreenFlash instance;

    [SerializeField] private Image flashImage;

    private Coroutine current;

    private void Awake()
    {
        instance = this;
    }

    public void BustFlash()
    {
        Play(Color.red, 0.25f, 1f);
    }

    public void WinFlash()
    {
        Play(Color.white, 0.2f, 1f);
    }

    public void BlackjackFlash()
    {
        Play(new Color(1f, 0.85f, 0.2f), 0.3f, 1f);
    }

    private void Play(Color color, float maxAlpha, float duration)
    {
        if (current != null)
            StopCoroutine(current);

        current = StartCoroutine(FlashRoutine(color, maxAlpha, duration));
    }

    private IEnumerator FlashRoutine(Color baseColor, float maxAlpha, float duration)
    {
        float t = 0f;
        float half = duration * 0.3f;

        while (t < half)
        {
            t += Time.deltaTime;
            float k = t / half;

            Color c = baseColor;
            c.a = Mathf.Lerp(0, maxAlpha, k);
            flashImage.color = c;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = t / duration;

            Color c = baseColor;
            c.a = Mathf.Lerp(maxAlpha, 0, k);
            flashImage.color = c;

            yield return null;
        }

        flashImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
    }
}