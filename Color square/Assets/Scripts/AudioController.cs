using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum MusicSnapshots
{
    game,
    dead,
    menu
}

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [Space(15)]
    [SerializeField] private AudioSource buttonAudio;
    [SerializeField] private AudioSource moneySound;
    [SerializeField] private AudioSource loseSound;
    [SerializeField] private AudioSource whooshAudio;
    [Space(15)]
    [SerializeField] private AudioMixerSnapshot gameSnapshot;
    [SerializeField] private AudioMixerSnapshot deadSnapshot;
    [SerializeField] private AudioMixerSnapshot menuSnapshot;

    [HideInInspector] public bool audioIsMute;

    private AudioSource music;
    private float[] samples = new float[64];

    void Start()
    {
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        playMusic();
        changeSnapshot(MusicSnapshots.menu, 0);
    }

    public void playMusic()
    {
        if (!music.isPlaying) music.Play();
    }

    public void pauseMusic()
    {
        music.Pause();
    }

    public void changeSnapshot(MusicSnapshots snapshot, float timeToTransition)
    {
        switch (snapshot)
        {
            case MusicSnapshots.game: gameSnapshot.TransitionTo(timeToTransition); break;
            case MusicSnapshots.dead: deadSnapshot.TransitionTo(timeToTransition); break;
            case MusicSnapshots.menu: menuSnapshot.TransitionTo(timeToTransition); break;
        }
    }

    public void mutingAllAudio(bool mute)
    {
        audioIsMute = mute;
        audioMixer.SetFloat("masterVolume", mute ? -80 : 0);
    }

    public void mutingMusic(bool mute)
    {
        audioMixer.SetFloat("musicVolume", mute ? -80 : 0);
    }

    public void mutingEffects(bool mute)
    {
        audioMixer.SetFloat("effectsVolume", mute ? -80 : 0);
    }

    public float getMusicBassVolume()
    {              
        return samples[0];
    }

    public void playMoneySound(float pitch)
    {
        moneySound.pitch = pitch;
        moneySound.Play();
    }

    public void playLoseSound(float pitch)
    {
        loseSound.pitch = pitch;
        loseSound.Play();
    }

    public void playWhooshSound(float pitch)
    {
        whooshAudio.pitch = pitch;
        whooshAudio.Play();
    }

    public void playButtonSound(float pitch)
    {
        buttonAudio.pitch = pitch;
        buttonAudio.Play();
    }

    void Update()
    {
        music.GetSpectrumData(samples, 0, FFTWindow.Rectangular);
    }
}
