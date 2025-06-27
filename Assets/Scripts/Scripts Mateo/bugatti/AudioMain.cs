using UnityEngine;
using System.Collections.Generic;

public class AudioMain : MonoBehaviour
{
    [SerializeField] protected List<AudioClip> clips;
    [SerializeField] protected float volume = 1f;

    protected void Play()
    {
        if (AudioManager.Instance == null || clips.Count == 0)
            return;

        if (AudioManager.Instance.IsNarrationPlaying())
            return;

        AudioClip selected = clips[Random.Range(0, clips.Count)];
        AudioManager.Instance.PlayClip(selected, volume);
    }
}
