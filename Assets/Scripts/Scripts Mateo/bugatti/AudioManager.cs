using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;

    private bool isNarrationPlaying = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClip(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null || isNarrationPlaying)
            return;

        StartCoroutine(PlayNarration(clip, volume));
    }

    private IEnumerator PlayNarration(AudioClip clip, float volume)
    {
        isNarrationPlaying = true;
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.Play();

        yield return new WaitForSeconds(clip.length);

        isNarrationPlaying = false;
    }

    public bool IsNarrationPlaying()
    {
        return isNarrationPlaying;
    }
}
