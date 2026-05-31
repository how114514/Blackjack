using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [SerializeField] private AudioSource sfxSource;

    [Header("Card SFX")]
    [SerializeField] private AudioClip dealClip;
    [SerializeField] private AudioClip chipClip;
    [SerializeField] private AudioClip flipClip;

    [Header("Result SFX")]
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip blackjackClip;
    [SerializeField] private AudioClip pushClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayChip() => Play(chipClip);
    public void PlayWin() => Play(winClip);
    public void PlayLose() => Play(loseClip);
    public void PlayBlackjack() => Play(blackjackClip);
    public void PlayPush() => Play(pushClip);

    private void Play(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.pitch = Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(clip);
    }
}