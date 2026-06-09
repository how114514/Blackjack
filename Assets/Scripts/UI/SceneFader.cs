using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float duration = 0.5f;

    public async Awaitable FadeIn()
    {
        await fadeImage
            .DOFade(1f, duration)
            .AsyncWaitForCompletion();
    }

    public async Awaitable FadeOut()
    {
        await fadeImage
            .DOFade(0f, duration)
            .AsyncWaitForCompletion();
    }
}