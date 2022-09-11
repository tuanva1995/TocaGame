using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
#pragma warning disable 0649
public class AudioController : Singleton<AudioController>
{
    private float originVol;
    private bool isSfxMute, isMusicMute;
    private List<AudioSource> sfxList = new List<AudioSource>();
    [SerializeField] private AudioSource sfxSource, musicSource;
    public bool Music
    {
        set
        {
            PlayerPrefs.SetInt("music_audio", value ? 1 : 0);
            isMusicMute = !value;
            if (isMusicMute)
            {
                Tween.Volume(musicSource, 0f, 1f, 0, Tween.EaseOut, Tween.LoopType.None, null, () => { musicSource.Stop(); }, false);
            }
            else
            {
                musicSource.Play();
                Tween.Volume(musicSource, originVol, 1f, 0, Tween.EaseIn, Tween.LoopType.None, null, null, false);
            }
        }
        get { return PlayerPrefs.GetInt("music_audio", 1) == 1; }
    }
    public bool SFX
    {
        set
        {
            PlayerPrefs.SetInt("sfx_audio", value ? 1 : 0);
            isSfxMute = !value;
        }
        get { return PlayerPrefs.GetInt("sfx_audio", 1) == 1; }
    }
    protected override void OnRegistration()
    {
        originVol = musicSource.volume;
        isMusicMute = !Music;
        if (isMusicMute) musicSource.Stop();
        isSfxMute = !SFX;
    }
    public void PlaySfx(AudioClip clip, bool isLoop = false, float duration = 0)
    {
        if (!isSfxMute)
        {
            if (isLoop)
            {
                GameObject sfx = Instantiate(sfxSource.gameObject, transform);
                var _sfxSource = sfx.GetComponent<AudioSource>();
                _sfxSource.clip = clip;
                _sfxSource.loop = true;
                _sfxSource.Play();
                sfxList.Add(_sfxSource);
                int ratio = (int)(duration / clip.length);
                float _duration = ratio * clip.length;
                StartCoroutine(RemoveSfxFromList(_duration, _sfxSource));
                Destroy(sfx, _duration);
            }
            else
                sfxSource.PlayOneShot(clip);
        }
    }
    IEnumerator RemoveSfxFromList(float delay, AudioSource target)
    {
        yield return new WaitForSecondsRealtime(delay);
        sfxList.Remove(target);
    }
    public void SetSfxVolume(float amount)
    {
        foreach (var sfx in sfxList)
        {
            sfx.volume = amount;
        }
    }

    public void PlayMusic(AudioClip clip, bool isLoop)
    {
        musicSource.clip = clip;
        musicSource.loop = isLoop;
        if (!isMusicMute)
            musicSource.Play();
    }
}
