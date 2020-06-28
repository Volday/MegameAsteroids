using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio2dController : MonoBehaviour
{
    public enum AudioClipType
    {
        thrustSound,
        shootSound,
        bigAsteroidExplosion,
        mediumAsteroidExplosion,
        smallAsteroidExplosion
    }

    [Range(0, 1)]
    public float masterVolume = 1;

    public Dictionary<AudioClipType, int> listsOfAudios = new Dictionary<AudioClipType, int>();
    public Audio[] audios;

    List<AudioSource> audioSourcesToRemove = new List<AudioSource>();
    List<Audio> audioSourcesContinuous = new List<Audio>();
    public delegate bool ActionToCheck();

    private void Awake()
    {
        for (int i = 0; i < audios.Length; i++)
        {
            listsOfAudios.Add(audios[i].audioType, i);
        }
    }

    private void Update()
    {
        //удаление неактивных источников звука
        for (int i = 0; i < audioSourcesToRemove.Count; i++)
        {
            if(!audioSourcesToRemove[i].isPlaying){
                Destroy(audioSourcesToRemove[i]);
                audioSourcesToRemove.RemoveAt(i);
                i--;
            }
        }

        //остановка воспроизведения зацикленных звуков, если делегат ActionToCheck вернул false
        for (int i = 0; i < audioSourcesContinuous.Count; i++)
        {
            if (!audioSourcesContinuous[i].GetActionToCheck()())
            {
                audioSourcesContinuous[i].GetAudioSource().loop = false;
                audioSourcesContinuous.RemoveAt(i);
            }
        }
    }

    //Запускает зацикленный источник звука
    public void PlayAudioContinuous(AudioClipType _audioType, ActionToCheck _toCheck)
    {
        Audio audio = audios[listsOfAudios[_audioType]];
        if (audio.GetAudioSource() == null) {
            audio = PlayAudio(_audioType);
            audio.SetActionToCheck(_toCheck);
            audio.GetAudioSource().loop = true;
            audioSourcesContinuous.Add(audio);
        }
    }

    //Для каждого нового звукового эффекта создаётся новый источник звука
    public Audio PlayAudio(AudioClipType _audioType) {
        Audio audio = audios[listsOfAudios[_audioType]];
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        audio.SetAudioSource(newAudioSource);
        newAudioSource.clip = audio.GetAudioClip();
        newAudioSource.pitch = audio.GetPitch();
        newAudioSource.volume *= masterVolume;
        newAudioSource.Play();
        audioSourcesToRemove.Add(newAudioSource);
        return audio;
    }

    [Serializable]
    public class Audio
    {
        public AudioClipType audioType;
        public AudioClip audioClip;
        [Range(0, 1)]
        public float volume = 1;
        private ActionToCheck checkContinuous;
        private AudioSource audioSource;

        public float maxPitch = 1;
        public float minPitch = 1;

        public void SetActionToCheck(ActionToCheck _action) {
            checkContinuous = _action;
        }

        public ActionToCheck GetActionToCheck()
        {
            return checkContinuous;
        }

        public void SetAudioSource(AudioSource _audioSource) {
            audioSource = _audioSource;
            audioSource.volume = volume;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        public AudioClip GetAudioClip() {
            audioSource.volume = volume;
            return audioClip;
        }

        public float GetPitch()
        {
            return UnityEngine.Random.Range(minPitch, maxPitch);
        }
    }
}
