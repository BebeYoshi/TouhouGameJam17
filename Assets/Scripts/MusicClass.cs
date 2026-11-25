using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClass : MonoBehaviour
{
    private static MusicClass instance;

    public AudioSource _audioSource;
    public AudioSource _sfxSource;

    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip deathClip;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (_sfxSource != null && clip != null)
        {
            _sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayDamageSFX()
    {
        PlaySFX(damageClip);
    }

    public void PlayDeathSFX()
    {
        PlaySFX(deathClip);
    }
}
