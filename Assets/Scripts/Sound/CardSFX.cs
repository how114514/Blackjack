using UnityEngine;

public class CardSFX : MonoBehaviour
{
    public static CardSFX Instance;

    [SerializeField] private AudioSource sfxSource;

    [Header("Card SFX")]
    [SerializeField] private AudioClip dealClip;
    [SerializeField] private AudioClip flipClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayDeal() => Play(dealClip);
    public void PlayFlip() => Play(flipClip);

    private void Play(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.pitch = Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(clip);
    }
}