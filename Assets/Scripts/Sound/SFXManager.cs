using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip dealClip;
    [SerializeField] private AudioClip chipClip;
    [SerializeField] private AudioClip flipClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayDeal()
    {
        Play(dealClip);
    }

    public void PlayChip()
    {
        Play(chipClip);
    }

    public void PlayFlip()
    {
        Play(flipClip);
    }

    private void Play(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.Stop();

        sfxSource.clip = clip;
        sfxSource.pitch = Random.Range(0.95f, 1.05f);

        sfxSource.Play();
    }
}