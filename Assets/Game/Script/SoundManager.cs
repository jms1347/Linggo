using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst
    {
        get; private set;
    }
    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(Inst);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public AudioMixer mixer;
    public AudioSource bgSound;
    public AudioClip[] bglist;

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if(arg0.name == bglist[i].name)
            {
                BgSoundPlay(bglist[i]);
            }
        }
    }
    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];

        bgSound.clip = clip;
        bgSound.loop = true;
        //bgSound.volume = 1f;
        bgSound.Play();
    }

    public void BGSoundVolume(float val)
    {

        mixer.SetFloat("BGSoundVolume", Mathf.Log10(val) * 20);
            }

    public void SFXVolume(float val)
    {

        mixer.SetFloat("SFX", Mathf.Log10(val) * 20);
    }
}
